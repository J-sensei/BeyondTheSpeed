using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    public CarController carController;
    //public SpeedTrapManager speedTrapManager;

    [Header("Debug Texts")]
    public TMP_Text ForwardSlips;
    public TMP_Text SidewaySlip;
    public TMP_Text MaxSlip;
    public TMP_Text WheelMotorTorque;
    public TMP_Text WheelBrakeTorque;
    public TMP_Text WheelAngle;
    public TMP_Text WheelRPMs;

    public TMP_Text Speed;
    public TMP_Text Gears;

    public TMP_Text LastSpeedTrap;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Wheel.DebugText(carController.Wheels, this);

        //Speed.text = "Top Speed: " + ((carController.CutOffRPM - carController.CutOffset) * (carController.GearsRatio[carController.GearsRatio.Length-1] * carController.GearDifferential)) / 64.82388059701493f + 
        //                "RPM: " + carController.EngineRPM + ", KPH: " + carController.KPH + ", Current Motor Torque: " + carController.DebugCurrentTorque;
        Gears.text = "Gear: " + carController.CurrentGear + ", Ratio: " + 
                        carController.CurrentFinalGearRatio + "Max Slip: " + carController.CurrentMaxSlip.ToString("N2") + ", Cut Off: " + 
                        carController.DebugInCutOff + ", CutOff Timer: " + carController.DebugCutOffTimer;

        //speedTrapManager.ShowDebug(this);
    }
}
