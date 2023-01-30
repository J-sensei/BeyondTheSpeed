using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedomaterUpdate : MonoBehaviour
{
    public CarController carController;
    public ProgressBar rpmBar;
    public TMP_Text kphText;

    void Start()
    {
        rpmBar.maximum = (int)carController.MaxRPM;
        rpmBar.current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        rpmBar.current = (int)carController.EngineRPM;
        kphText.text = carController.KPH.ToString("N0") + "KM/h";

        rpmBar.color = Color.Lerp(Color.yellow, Color.red, (float)rpmBar.current / (float)(rpmBar.maximum * 1.5f));
    }
}
