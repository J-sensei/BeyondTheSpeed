using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float MoveSmoothTime;
    public float RotateSmoothTime;

    public Vector3 MoveOffset;
    public Vector3 RotateOffset;

    public Transform Target;

    private CarController carController;
    private Vector3 pos = new(0, 20.0f, -30f);
    private Vector3 rot = new(20f, 0, 0);

    private void Start()
    {
        carController = Target.GetComponent<CarController>();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    StartCoroutine(Shake(0.05f, 0.02f));
        //}
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.start) return;
        CameraMovement();
        CameraRotation();
    }

    void CameraMovement()
    {
        //Vector3 targetPosition = Target.TransformPoint(MoveOffset);
        //transform.position = Vector3.Lerp(transform.position, targetPosition, MoveSmoothTime * Time.deltaTime);
        Vector3 targetPos = Target.position + MoveOffset;
        //targetPos.x = 0;
        transform.position = Vector3.Lerp(transform.position, targetPos, MoveSmoothTime * Time.deltaTime);
    }

    void CameraRotation()
    {
        Vector3 direction = Target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction + RotateOffset, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, MoveSmoothTime * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rot, MoveSmoothTime * Time.deltaTime);
    }

    Vector3 shakeOffset;
    public IEnumerator Shake(float duration, float magnitude)
    {
        shakeOffset = Vector3.zero;
        float elapsed = .0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            shakeOffset = new Vector3(x, y, transform.position.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;

        //transform.localPosition = originalPosition;
    }
}
