using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FuelUpdate : MonoBehaviour
{
    private ProgressBar _progressBar;
    public TMP_Text currentLevel;
    public TMP_Text nextLevel;
    void Start()
    {
        _progressBar = GetComponent<ProgressBar>();
    }

    // Update is called once per frame
    void Update()
    {
        _progressBar.current = (int)GameManager.Instance.fuel;
        _progressBar.maximum = (int)GameManager.Instance.maxFuel;

        currentLevel.text = GameManager.Instance.level.ToString();
        nextLevel.text = (GameManager.Instance.level + 1).ToString();
    }
}
