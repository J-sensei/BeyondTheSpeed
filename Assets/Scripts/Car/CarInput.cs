using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle various inputs of a car
/// </summary>
public class CarInput : MonoBehaviour
{
    #region Serializable Fields
    [Header("Configuration")]
    [Tooltip("Use keyboard and mouse and input if debug mode is set to true")]
    [SerializeField] private bool DebugMode;
    [SerializeField] private float DriftAxisChangeSpeed = 2f;
    [SerializeField] private float ThrottleAxisChangeSpeed = 5f;

    [Header("Debug Keys")]
    [Space]
    [SerializeField] private KeyCode DriftLeftKey = KeyCode.Q;
    [SerializeField] private KeyCode DriftRightKey = KeyCode.E;
    [SerializeField] private KeyCode NitroBoostKey = KeyCode.F;
    //[SerializeField] private string ThrottleAxis = "Vertical";

    [Header("Touch Inputs")]
    [Space]
    [SerializeField] private TouchControl ThrottleButton;
    [SerializeField] private TouchControl DriftLeftButton;
    [SerializeField] private TouchControl DriftRightButton;
    [SerializeField] private TouchControl NitroBoostButton;
    [SerializeField] private Joystick Joystick;
    #endregion

    #region Private Fields
    private float verticalAxis = 0f;
    private float driftAxis = 0f;
    private bool nitro;
    #endregion

    #region Properties
    public float VerticalAxis => verticalAxis;
    public float DriftAxis => driftAxis;
    public bool Nitro => nitro;
    #endregion

    private void Update()
    {
        if (DebugMode)
        {
            DebugInputs();
        }
        else
        {
            TouchInputs();
        }
    }

    private void DebugInputs()
    {
        //verticalAxis = Input.GetAxis(ThrottleAxis);
        //verticalAxis = Mathf.Clamp(verticalAxis, 0f, 1f);
        //verticalAxis = 1f; // Auo acceleration
        //if (Input.GetKeyDown(KeyCode.Space) verticalAxis = 0f;
        nitro = Input.GetKey(NitroBoostKey);
        if (Input.GetKey(KeyCode.Space))
        {
            verticalAxis = UpdateAxis(verticalAxis, true, false, ThrottleAxisChangeSpeed);
        }
        else
        {
            verticalAxis = UpdateAxis(verticalAxis, false, true, ThrottleAxisChangeSpeed);
        }

        bool left = Input.GetKey(DriftLeftKey);
        bool right = Input.GetKey(DriftRightKey);
        driftAxis = UpdateAxis(driftAxis, left, right, DriftAxisChangeSpeed);
    }

    float threshold = 0.5f;
    private void TouchInputs()
    {
        //verticalAxis = UpdateAxis(verticalAxis, true, ThrottleButton.Pressed, ThrottleAxisChangeSpeed);
        //verticalAxis = Mathf.Clamp(verticalAxis, 0f, 1f);
        //nitro = NitroBoostButton.Pressed;

        //bool left = DriftLeftButton.Pressed;
        //bool right = DriftRightButton.Pressed;
        //driftAxis = UpdateAxis(driftAxis, left, right, DriftAxisChangeSpeed);
        nitro = false;
        Vector2 input = Joystick.Input;
        if (input.y < 0f)
        {
            //verticalAxis = UpdateAxis(verticalAxis, true, false, ThrottleAxisChangeSpeed);
            if(input.y < -threshold)
            {
                verticalAxis = input.y;
            }
        }
        else
        {
            verticalAxis = 1f;
            if (input.y > threshold)
            {
                nitro = true;
            }
        }

        //if(input.x < -threshold)
        //{
        //    driftAxis = -1f;
        //}
        //else if(input.x > threshold)
        //{
        //    driftAxis = 1f;
        //}
        //else
        //{
        //    driftAxis = 0f;
        //}

        driftAxis = input.x;
    }

    /// <summary>
    /// Update an axis value
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="changeSpeed"></param>
    private float UpdateAxis(float axis, bool left, bool right, float changeSpeed)
    {
        // Drift axis
        if (right)
        {
            axis += changeSpeed * Time.deltaTime;
        }
        else if (left)
        {
            axis -= changeSpeed * Time.deltaTime;
        }
        else
        {
            if (driftAxis > 0)
            {
                axis -= changeSpeed * Time.deltaTime;
            }
            else
            {
                axis += changeSpeed * Time.deltaTime;
            }

            axis = Mathf.Abs(axis) < 0.1f ? 0 : axis;
        }

        return Mathf.Clamp(axis, -1f, 1f);
    }
    
    bool countDoubleTap = false;
    float doubleTapTimer = 0f;
    public bool IsDoubleTap()
    {
        bool result = false;
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!countDoubleTap)
            {
                countDoubleTap = true;
                doubleTapTimer = 0f;
            }
            else
            {
                result = true;
            }
        }

        if (countDoubleTap)
        {
            doubleTapTimer += Time.deltaTime;
            if (doubleTapTimer > 0.5f) countDoubleTap = false;
        }
        return result;
    }
}
