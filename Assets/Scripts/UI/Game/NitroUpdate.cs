using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroUpdate : MonoBehaviour
{
    public CarController carController;
    private ProgressBar progressBar;
    // Start is called before the first frame update
    void Start()
    {
        progressBar = GetComponent<ProgressBar>();
        progressBar.maximum = (int)carController.MaxNitroBar;
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.current = (int)carController.CurrentNitroBar;
    }
}
