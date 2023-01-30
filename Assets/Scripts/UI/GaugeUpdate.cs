using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUpdate : MonoBehaviour
{
    public float MaxAngle;
    public float MinAngle;

    public Transform Needle;
    public float CurrentValue;
    public float MaxValue;

    public Text kphTesxt;
    public Text gearText;
    int initFontSize;

    public CarController CarController;

    private void Start()
    {
        MaxValue = 8000f;
        initFontSize = gearText.fontSize;
    }

    private void Update()
    {
        CurrentValue = CarController.EngineRPM;

        Needle.eulerAngles = new Vector3(0f, 0f, GetGaugeRotation());
        kphTesxt.text = CarController.KPH.ToString("N0");
        gearText.text = CarController.CurrentGear.ToString();

        if (CarController.DebugInCutOff)
        {
            gearText.fontSize = (int)Mathf.Lerp(gearText.fontSize, initFontSize * 1.5f, 15f * Time.deltaTime);
            gearText.color = Color.red;
        }
        else
        {
            gearText.fontSize = (int)Mathf.Lerp(gearText.fontSize, initFontSize, 15f * Time.deltaTime);
            gearText.color = Color.white;
        }
    }

    float GetGaugeRotation()
    {
        float totalAngle = MinAngle - MaxAngle;
        float normalized = CurrentValue / MaxValue;

        return MinAngle - normalized * totalAngle;
    }
}
