using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStatusUI : MonoBehaviour
{
    public CarController carController;
    public TMP_Text DistanceTravel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DistanceTravel.text = (carController.DistanceTravelled * 0.1f).ToString("N0") + "M";
    }
}
