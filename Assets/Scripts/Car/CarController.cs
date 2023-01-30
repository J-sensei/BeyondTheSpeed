using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Wheel[] wheels = new Wheel[4];
    [SerializeField] private CarConfig config;

    [Header("Back Fire")]
    [Space]
    [Tooltip("Probability to trigger the back fire events")]
    [Range(0f, 1f)]
    [SerializeField] private float BackFireProbability = 0.2f;

    [Header("Particles")]
    public Material BrakeMaterial;

    [Header("Gravity")]
    public float DownwardForce = 9.8f;

    public float MinimumKPH { get; set; } = 20;

    /// <summary>
    /// Trigger when engine is cutting off
    /// </summary>
    public System.Action BackFireAction;
    /// <summary>
    /// Trigger when nitro boost is active
    /// </summary>
    public System.Action NitroBackFireAction;

    /// <summary>
    /// Car Inputs
    /// </summary>
    private CarInput inputs;

    /// <summary>
    /// Rigidbody of the car
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// Vertical input of the car, for the throttle movement
    /// </summary>
    private float vertical;
    /// <summary>
    /// Drifting input of the car, for the drifting movement
    /// </summary>
    private float driftAxis;

    /// <summary>
    /// Motor torque evenlly distributed to the moving wheels
    /// </summary>
    private int motorTorqueDivide;
    /// <summary>
    /// Current engine RPM of the car
    /// </summary>
    private float rpm;
    /// <summary>
    /// Current amount of Motor Torque generate from the engine RPM
    /// </summary>
    private float currentMotorTorque;
    /// <summary>
    /// Current gear index
    /// </summary>
    private int currentGear = 0;
    /// <summary>
    /// Current Timer to record the cut off time left
    /// </summary>
    private float cutOffTimer;
    /// <summary>
    /// Determine whether the engine is cutting off
    /// </summary>
    private bool inCutOff;

    // Nitro 
    /// <summary>
    /// Input to trigger the nitro boost
    /// </summary>
    private bool nitroInput;
    /// <summary>
    /// Determine the nitro boost is trigering
    /// </summary>
    private bool nitroBoost;
    /// <summary>
    /// Current amount of nitro bar
    /// </summary>
    private float currentNitroBar = 0f;
    /// <summary>
    /// Current amount nitro bar
    /// </summary>
    public float CurrentNitroBar { get { return currentNitroBar; } set { currentNitroBar = value; } }
    public float MaxNitroBar { get { return config.MaxNitroBar; } }

    // Calculating distance
    private Vector3 lastPosition;
    public float DistanceTravelled { get; private set; }
    public float DistanceMoved { get; private set; }

    // Collision
    private bool colliding = false;
    /// <summary>
    /// Position point of the car that is collided
    /// </summary>
    private Vector3 collisionPosition;
    /// <summary>
    /// Is the car colliding with other objects?
    /// </summary>
    public bool Colliding { get { return colliding; } }

    /// <summary>
    /// Array of wheels attached to the car
    /// </summary>
    public Wheel[] Wheels { get { return wheels; } }
    /// <summary>
    /// Current RPM of the engine
    /// </summary>
    public float EngineRPM { get { return rpm; } }
    /// <summary>
    /// Current speed of the car, in KM/h (Kilometer per hour)
    /// </summary>
    public float KPH { get { return rb.velocity.magnitude * 3.6f; } }
    /// <summary>
    /// Current gear number
    /// </summary>
    public float CurrentGear { get { return currentGear + 1; } }
    /// <summary>
    /// Current gear ratio * differential apply to the car
    /// </summary>
    public float CurrentFinalGearRatio { get { return config.GearsRatio[currentGear] * config.GearDifferential; } }
    /// <summary>
    /// Maximum slip from the car, calculated using all wheels
    /// </summary>
    public float CurrentMaxSlip { 
        get
        {
            float result = wheels[0].CurrentMaxSlip;
            for (int i = 1; i < wheels.Length; i++)
            {
                if (result < wheels[i].CurrentMaxSlip)
                {
                    result = wheels[i].CurrentMaxSlip;
                }
            }
            return result;
        } 
    }

    /// <summary>
    /// Determine if the car is drifting
    /// </summary>
    public bool IsDrifting
    {
        get
        {
            Vector3 driftValue = transform.InverseTransformVector(rb.velocity);
            return Mathf.Abs(driftValue.x) >= 10f;
        }
    }

    public Vector3 LocalVelocity { get { return transform.InverseTransformVector(rb.velocity); } }

    /// <summary>
    /// Determine if all the car wheels is sticking to the ground
    /// </summary>
    public bool IsGrounded
    {
        get
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                if (!wheels[i].OnGround)
                    return false;
            }
            return true;
        }
    }
    public float MaxRPM { get { return config.MaxRPM; } }
    /// <summary>
    /// The car is currently using the nitro boost
    /// </summary>
    public bool NitroBoost { get { return nitroBoost; } }
    /// <summary>
    /// Position point of the car that is collided
    /// </summary>
    public Vector3 CollisionPosition { get { return collisionPosition; } }

    #region Debug Properties
    public float DebugCurrentTorque { get { return currentMotorTorque; } }
    public bool DebugInCutOff { get { return inCutOff; } }
    public float DebugCutOffTimer { get { return cutOffTimer; } }
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the rigidbody component
        rb.centerOfMass = config.CenterOfMass; // Apply the center of mass

        inputs = GetComponent<CarInput>(); // Get the car inputs

        UpdateDriveType(config.DriveType);
        lastPosition = transform.position;
        currentNitroBar = 100f;
    }

    private void Update()
    {
        // Give player ability to control the car when the game is not over
        //if (!GameManager.Instance.gameOver)
        //{
        //    UpdateInput();
        //}
        //else
        //{
        //    vertical = 0f;
        //    driftAxis = 0f;
        //}

        UpdateInput(); // Test

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].Update();
        }

        // Brake Light emission
        if (vertical < 1)
        {
            BrakeMaterial.EnableKeyword("_EMISSION");
        }
        else
        {
            BrakeMaterial.DisableKeyword("_EMISSION");
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.start) return;
        // Calculate distance
        if (!GameManager.Instance.gameOver)
        {
            DistanceMoved = Vector3.Distance(lastPosition, transform.position);
            DistanceTravelled += DistanceMoved;
            lastPosition = transform.position;
        }

        if (!IsGrounded)
            rb.AddForce(new Vector3(0f, 1f, 0f) * rb.mass);

        UpdateEngine();
        UpdateNitroBoost();
        GearShift();

        if(!GameManager.Instance.gameOver)
            Drift();

        // Update the wheels physics
        for(int i = 0; i < wheels.Length; i++)
        {
            wheels[i].FixedUpdate();
        }

        // If the car is not grounded, apply negative velocity of y to push it downward
        if (!IsGrounded)
        {
            //Vector3 velocity = rb.velocity;
            //    //velocity.y = -1f;
            //    rb.velocity = velocity;
            rb.AddForce(1, -DownwardForce * rb.mass ,1);
        }
    }

    /// <summary>
    /// Update the car drive type apply to the wheels
    /// </summary>
    /// <param name="driveType"></param>
    private void UpdateDriveType(DriveType driveType)
    {
        switch (driveType)
        {
            case DriveType.RWD:
                wheels[2].IsTorqueApply = true;
                wheels[3].IsTorqueApply = true;
                motorTorqueDivide = 2;
                break;
            case DriveType.FWD:
                wheels[0].IsTorqueApply = true;
                wheels[1].IsTorqueApply = true;
                motorTorqueDivide = 2;
                break;
            case DriveType.AWD:
                for (int i = 0; i < wheels.Length; i++)
                    wheels[i].IsTorqueApply = true;
                motorTorqueDivide = 4;
                break;
        }
    }

    /// <summary>
    /// Update the inputs of the car
    /// </summary>
    private void UpdateInput()
    {
        vertical = inputs.VerticalAxis;
        driftAxis = inputs.DriftAxis;
        nitroInput = inputs.Nitro;
    }

    /// <summary>
    /// Update the RPM logic and apply the motor torque to the wheels
    /// </summary>
    private void UpdateEngine()
    {
        // If the engine is cutting off
        if (inCutOff)
        {
            if(cutOffTimer > 0)
            {
                cutOffTimer -= Time.fixedDeltaTime; // Decrease timer
                // Try to reach cut off rpm
                rpm = Mathf.Lerp(rpm, (config.CutOffRPM - config.CutOffset), config.RPMChangeSpeed * Time.fixedDeltaTime);
            }
            else
            {
                inCutOff = false; // Not in cut off anymore if timer is out
            }
        }

        float minRPM = 0;
        for (int i = 0 + 1; i < wheels.Length; i++)
        {
            minRPM += wheels[i].WheelCollider.rpm;
        }

        minRPM /= motorTorqueDivide; // Average rpm from wheels

        // Try to reach rpm to these if not in cut off
        if (!inCutOff)
        {
            // Calculate RPM for normal scenario
            float targetRPM = ((minRPM + 20) * config.GearsRatio[currentGear] * config.GearDifferential);
            targetRPM = Mathf.Abs(targetRPM);
            targetRPM = Mathf.Clamp(targetRPM, config.MinRPM, config.MaxRPM);

            rpm = Mathf.Lerp(rpm, targetRPM, config.RPMChangeSpeed * Time.fixedDeltaTime);
        }

        // Set the RPM to Cut Off
        if (rpm >= config.CutOffRPM)
        {
            BackFire();
            inCutOff = true;
            cutOffTimer = 0.1f;
            return;
        }

        // Set the brake force
        if (KPH < MinimumKPH) vertical = 1f;
        float brakeForce = (1 - vertical) * config.BrakeTorque;
        if(GameManager.Instance.gameOver) brakeForce = config.BrakeTorque;

        // Calculate and apply current motor torque
        currentMotorTorque = 0;
        // If car is accelerating
        if (vertical > 0 && !GameManager.Instance.gameOver)
        {
            float motorTorqueFromRpm = config.MotorTorqueFromRPM.Evaluate(rpm * 0.001f); // Evaluate the animation curve
            // Calculate the current motor torque
            currentMotorTorque = vertical * 
                                (motorTorqueFromRpm * 
                                ((config.MotorTorque / motorTorqueDivide) * (config.GearsRatio[currentGear] * 
                                config.GearDifferential)));

            // If rpm is bigger than the maximum RPM for this car, no torque will be apply
            if (Mathf.Abs(minRPM) * (config.GearsRatio[currentGear] * config.GearDifferential) > config.MaxRPM)
            {
                currentMotorTorque = 0;
            }

            //If the rpm of the wheel is less than the max rpm engine * current ratio,
            //then apply the current torque for wheel, else not torque for wheel.
            float maxWheelRPM = (config.GearsRatio[currentGear] * config.GearDifferential) * rpm;

            //if (driftAxis > 0.1f || driftAxis < -0.1f) currentMotorTorque *= 2f;

            //if (KPH > 300f) currentMotorTorque = 0;
            for (int i = 0; i < wheels.Length; i++)
            {
                // Apply the motor torque is the RPM of the wheels is lower enough
                if (wheels[i].WheelCollider.rpm <= maxWheelRPM && wheels[i].IsTorqueApply)
                {
                    wheels[i].WheelCollider.motorTorque = currentMotorTorque;
                }
                else
                {
                    wheels[i].WheelCollider.motorTorque = 0;
                }

                wheels[i].WheelCollider.brakeTorque = brakeForce; // Brake force
            }
        }
        else // Not moving forward
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].WheelCollider.motorTorque = 0; // No force will be apply to the wheel colliders
                wheels[i].WheelCollider.brakeTorque = brakeForce; // Brake force
            }
        }
    }

    /// <summary>
    /// Trigger when the RPM is cutting off, exhaust pipe will fire up
    /// </summary>
    private void BackFire()
    {
        if (Random.Range(0f, 1f) <= BackFireProbability)
        {
            if(BackFireAction != null)
            {
                BackFireAction.Invoke();
            }
        }
    }

    /// <summary>
    /// Shift up / down the gears
    /// </summary>
    private void GearShift()
    {
        if (!IsGrounded) return; // If the wheels is not touching the ground, dont do the gear shifting

        bool forwardIsSlip = false; // Determine is car is too slippy to apply the gear shift

        // If too slippy, prevent the gear change
        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i].CurrentMaxSlip > config.GearShiftSlip)
            {
                forwardIsSlip = true;
                break;
            }
        }

        float prevRatio = 0;
        float newRatio = 0;

        if (!forwardIsSlip && rpm > config.RPMToShiftUp && currentGear >= 0 && currentGear < config.GearsRatio.Length - 1) // Shift up
        {
            prevRatio = config.GearsRatio[currentGear] * config.GearDifferential;
            currentGear++;
            newRatio = config.GearsRatio[currentGear] * config.GearDifferential;

            // Play gear shift sound
            //AudioManager.Instance.GearShift.Play();
        }
        else if (rpm < config.RPMToShiftDown && currentGear > 0 && (rpm <= config.MinRPM || currentGear != 0))
        {
            prevRatio = config.GearsRatio[currentGear] * config.GearDifferential;
            currentGear--;
            newRatio = config.GearsRatio[currentGear] * config.GearDifferential;
        }

        // Change the RPM when gear shifted
        if (!Mathf.Approximately(prevRatio, 0) && !Mathf.Approximately(newRatio, 0))
        {
            rpm = Mathf.Lerp(rpm, rpm * (newRatio / prevRatio), config.RPMChangeSpeed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Drift the car left or right
    /// </summary>
    private void Drift()
    {
        var vel = Vector3.Lerp(rb.angularVelocity,
                        new Vector3(rb.angularVelocity.x, driftAxis * config.MaxAngularVelocity, rb.angularVelocity.z),
                        config.AngularVelocityChangeSpeed * Time.fixedDeltaTime);
        // Clamp
        float angle = transform.rotation.eulerAngles.y;
        //Debug.Log(rb.velocity);
        bool inRange = ((angle >= 270f && angle <= 360f) || (angle >= 0f && angle <= 90f) || angle < 0f);

        if (outControl)
        {
            vel.y = outControlVel;

            if (inRange) outControl = false;
        }

        rb.angularVelocity = vel;
        //Debug.Log(inRange + " Angle: " + angle);

        // Make the car overturn and rotate if out of control
        if (!inRange && !outControl)
        {
            outControl = true;
            if (rb.velocity.x < 0)
                outControlVel = -config.MaxAngularVelocity * 4;
            else if (rb.velocity.x > 0)
                outControlVel = config.MaxAngularVelocity * 4;
            else
                outControlVel = 0f;

            Vector3 temp = rb.velocity;
            temp.y = 0f;
            rb.velocity = temp;
        }
    }
    bool outControl = false;
    float outControlVel;

    /// <summary>
    /// Boost the car speed
    /// </summary>
    private void UpdateNitroBoost()
    {
        // Increase the nitro bar when drifting
        //if (IsDrifting) currentNitroBar += config.NitroIncreaseModifier;

        // Condition to trigger the nitro boost
        if(nitroInput && !nitroBoost && currentNitroBar > config.MinNitroBarToUse && !GameManager.Instance.gameOver)
        {
            //NitroPressSound.Play();
            AudioManager.Instance.NitroPress.Play();
            nitroBoost = true;
        }

        //if (nitroBoost)
        if(nitroBoost && !GameManager.Instance.gameOver)
        {
            currentNitroBar -= 10f * Time.fixedDeltaTime; // descrease the nitro bar by the fixed delta time (every second)
            // Trigger the Nitro Back Fire Event
            if (NitroBackFireAction != null)
                NitroBackFireAction.Invoke();

            // Adding force to the rb when nitro boost is valid
            rb.AddForce(new Vector3(0f, 0f, transform.forward.normalized.z) * config.NitroSpeedBoost * rb.mass);

            // When nitro bar is run out, nitro boost will set to false
            if (currentNitroBar < 0 || inputs.VerticalAxis < 0f) nitroBoost = false;
        }
        else
        {
            nitroBoost = false;
        }

        currentNitroBar = Mathf.Clamp(currentNitroBar, 0f, config.MaxNitroBar); // Clamp the nitro bar
    }

    void OnCollisionEnter(Collision collision)
    {
        //AudioManager.Instance.PlayCrashSound(KPH);
        //if (KPH > 80f)
        //{
        //    EffectManager.Instance.PlayParticle(Instantiate(EffectManager.Instance.BoomText), CollisionPosition);
        //}

        //rb.velocity = rb.velocity * 0.9f; // Slow the car abit whenever crashing to something

        // Prevent the car go up
        if(rb != null)
        {
            Vector3 velocity = rb.velocity;
            if (velocity.y > 0)
            {
                velocity.y = 0f;
                rb.velocity = velocity;
            }
        }

        //Debug.Log(collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude > 2)
        {
            AudioManager.Instance.PlayCrashSound(KPH);
            GameManager.Instance.gameOver = true;
        }
    }

    // When keep colliding
    private void OnCollisionStay(Collision collision)
    {
        ContactPoint contact = collision.contacts[0]; // Get the contact point
        collisionPosition = contact.point;

        // Prevent the car go up when colliding
        //Vector3 velocity = rb.velocity;
        //if (velocity.y > 0)
        //{
        //    velocity.y = -velocity.y;
        //    rb.velocity = velocity;
        //}

        colliding = true;

        // Rotate the car when colliding to prevent stuck into the wall
        if(vertical > 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.position - contact.point, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.005f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (rb == null) return;

        var centerPos = transform.position;
        var velocity = transform.position + (Vector3.ClampMagnitude(rb.velocity, 4));
        var forwardPos = transform.TransformPoint(Vector3.forward * 4);

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(centerPos, 0.2f);
        Gizmos.DrawLine(centerPos, velocity);
        Gizmos.DrawLine(centerPos, forwardPos);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(forwardPos, 0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(velocity, 0.2f);

        Gizmos.color = Color.white;
    }
}
