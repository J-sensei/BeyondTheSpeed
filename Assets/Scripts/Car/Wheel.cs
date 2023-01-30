using TMPro;
using UnityEngine;

[System.Serializable]
public class Wheel
{
    [SerializeField] private WheelCollider wheelCollider;
    [SerializeField] private Transform wheelMesh;
    [SerializeField] private ParticleSystem smokeEffect;
    [SerializeField] private TrailRenderer tireSkid;
    public bool IsTorqueApply;
    public float slipThreshold = 0.4f;
    public float skidThreshold = 0.4f;
    public Vector3 TrailOffset = new Vector3(0.1f, 0.05f, 0f);

    private float forwardSlip;
    private float sidewaySlip;

    private WheelHit hit;
    private bool onGround;
    private TrailRenderer trail;

    public WheelCollider WheelCollider => wheelCollider;
    public Transform WheelMesh => wheelMesh;
    public float CurrentMaxSlip
    {
        get
        {
            return Mathf.Max(forwardSlip, sidewaySlip);
        }
    }

    /// <summary>
    /// Is wheel touch on object
    /// </summary>
    public bool OnGround { get { return onGround; } }

    public void Update()
    {
        UpdateMeshTransform(wheelCollider, wheelMesh);
        RenderEffect();
    }

    private void RenderEffect()
    {
        if(smokeEffect != null)
        {
            if (CurrentMaxSlip > slipThreshold)
            {
                smokeEffect.Emit(1);
            }
            else
            {
                smokeEffect.Stop();
            }
        }

        if (tireSkid != null)
        {
            if (CurrentMaxSlip > skidThreshold)
            {
                tireSkid.emitting = true;
            }
            else
            {
                tireSkid.emitting = false;
            }
        }
    }

    private void RenderParticles()
    {
        if(WheelCollider.isGrounded && CurrentMaxSlip > slipThreshold)
        {
            ParticleSystem particle = EffectManager.Instance.TyreSmoke;
            Vector3 point = WheelCollider.transform.position;
            point.y = hit.point.y;
            particle.transform.position = point;
            particle.Play();

            if (trail == null)
            {
                //Get free or create trail.
                Vector3 hitPoint = WheelCollider.transform.position;
                hitPoint.y = hit.point.y;
                trail = EffectManager.Instance.GetTrail(hitPoint);
                trail.transform.SetParent(WheelCollider.transform);
                trail.transform.localPosition += TrailOffset;
            }
        }
    }

    private void UpdateMeshTransform(WheelCollider collider, Transform transform)
    {
        Vector3 pos;
        Quaternion quat;
        collider.GetWorldPose(out pos, out quat);
        transform.position = pos;
        transform.rotation = quat;
    }

    public void FixedUpdate()
    {
        if (wheelCollider.GetGroundHit(out hit))
        {
            var prevForwar = forwardSlip;
            var prevSide = sidewaySlip;

            forwardSlip = (prevForwar + Mathf.Abs(hit.forwardSlip)) / 2;
            sidewaySlip = (prevSide + Mathf.Abs(hit.sidewaysSlip)) / 2;

            onGround = true;
        }
        else
        {
            forwardSlip = 0;
            sidewaySlip = 0;
            onGround = false;
        }
    }

    public static void DebugText(Wheel[] wheels, DebugText debugText)
    {
        string forwardText = "Forward Slips - ";
        string sidewayText = "Sideway Slips - ";
        string currentText = "Max Slips - ";
        string motorText = "Motor Torque - ";
        string brakeText = "Brake Torque - ";
        string steerText = "Steer Angle - ";
        string rpmsText = "RPMs - ";

        for (int i = 0; i < wheels.Length; i++)
        {
            forwardText += wheels[i].wheelCollider.gameObject.name + ": " + wheels[i].forwardSlip.ToString("n2") + " Grounded: " + wheels[i].OnGround +  " ";
            sidewayText += wheels[i].wheelCollider.gameObject.name + ": " + wheels[i].sidewaySlip.ToString("n2") + " ";
            currentText += wheels[i].wheelCollider.gameObject.name + ": " + wheels[i].CurrentMaxSlip.ToString("n2") + " Slip: " + (wheels[i].CurrentMaxSlip > wheels[i].slipThreshold) + " ";
            motorText += wheels[i].wheelCollider.gameObject.name + ": " + wheels[i].WheelCollider.motorTorque.ToString("n2") + " ";
            brakeText += wheels[i].wheelCollider.gameObject.name + ": " + wheels[i].WheelCollider.brakeTorque.ToString("n2") + " ";
            steerText += wheels[i].wheelCollider.gameObject.name + ": " + wheels[i].WheelCollider.steerAngle.ToString("n2") + " ";
            rpmsText += wheels[i].wheelCollider.gameObject.name + ": " + wheels[i].WheelCollider.rpm.ToString("n2") + " ";
        }

        debugText.ForwardSlips.text = forwardText;
        debugText.SidewaySlip.text = sidewayText;
        debugText.MaxSlip.text = currentText;
        debugText.WheelMotorTorque.text = motorText;
        debugText.WheelBrakeTorque.text = brakeText;
        debugText.WheelAngle.text = steerText;
        debugText.WheelRPMs.text = rpmsText;
    }
}
