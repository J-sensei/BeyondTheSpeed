using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Configuration of a car
/// </summary>
[System.Serializable]
public class CarConfig
{

    [Header("General Configuration")]
    [Space]
    [Tooltip("Car Drive Type, determine the forces apply to the wheels")]
    public DriveType DriveType = DriveType.RWD;
    [Tooltip("Center of Mass of the vehicle")]
    public Vector3 CenterOfMass = new Vector3(0f, -0.5f, 0f);

    [Header("Drift Settings")]
    [Space]
    [Tooltip("Speed of the car turn the body to drift")]
    public float DriftAngleChangeSpeed = 25f;
    [Tooltip("Change Speed of the car turn the body to drift")]
    public float AngularVelocityChangeSpeed = 5f;
    [Tooltip("Maximum Drift Angle allowed")]
    public float MaxDriftAngle = 60f;
    [Tooltip("Speed to turn the car while drifting (Handling 1 - 2)")]
    public float MaxAngularVelocity = 1.5f;

    [Header("Engine")]
    [Space]
    [Tooltip("Force apply to the wheels (Acceleration)")]
    public float MotorTorque = 700f;
    [Tooltip("Force apply to the brake (Braking)")]
    public float BrakeTorque = 1000f;
    [Tooltip("Graph to evaluate motor torque based on the engine RPM")]
    public AnimationCurve MotorTorqueFromRPM;
    [Tooltip("Minimum engine RPM")]
    public float MinRPM = 700f;
    [Tooltip("Maximum engine RPM can reach")]
    public float MaxRPM = 7000f;
    [Tooltip("RPM start to cut off and lose the RPM")]
    public float CutOffRPM = 6600f;
    [Tooltip("Amount of RPM lose during cut off")]
    public float CutOffset = 500f; // RPM will try to reach CutOffRPM - CutOffset
    [Tooltip("Speed changing current RPM to the target / desired RPM")]
    public float RPMChangeSpeed = 15f;

    [Header("Gear")]    
    [Space]
    [Tooltip("Gear Ratios used for the gear shift")]
    public float[] GearsRatio = new float[] { 3.83f, 2.36f, 1.67f, 1.312f, 1f };
    [Tooltip("Final Gear Ratio apply to gear ratios")]
    public float GearDifferential = 3.56f;
    [Tooltip("RPM to shift the gear up")]
    public float RPMToShiftUp = 6500f;
    [Tooltip("RPM to shift the gear down")]
    public float RPMToShiftDown = 4500f;
    [Tooltip("If slip value exceed this, gear shift will not happen (when drifting no gear shift will happen)")]
    public float GearShiftSlip = 0.5f;

    [Header("Nitro Boost")]
    [Space]
    [Tooltip("Maximum amount nitro bar, in second")]
    public float MaxNitroBar = 2f;
    [Tooltip("Minimum Nitro amount to trigger the nitro boost")]
    public float MinNitroBarToUse = 1.5f;
    [Tooltip("Amount of nitro boost bar increase")]
    public float NitroIncreaseModifier = 0.01f;
    public float NitroSpeedBoost = 10f;
}

/// <summary>
/// Type of wheels force apply to a car
/// </summary>
public enum DriveType
{
    /// <summary>
    /// All Wheels Drive
    /// </summary>
    AWD,
    /// <summary>
    /// Front Wheels Drive
    /// </summary>
    FWD,
    /// <summary>
    /// Rear Wheels Drive
    /// </summary>
    RWD
}

