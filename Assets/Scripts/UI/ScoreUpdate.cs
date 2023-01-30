using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUpdate : MonoBehaviour
{
    public TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = GameManager.Instance.Score.ToString();
    }
}
