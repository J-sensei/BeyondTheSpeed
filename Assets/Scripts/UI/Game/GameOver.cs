using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public CarController carController;
    public GameObject resultScreen;
    public TMP_Text scoreText;

    private void Start()
    {
        //resultScreen.SetActive(true);
    }

    void Update()
    {
        if(GameManager.Instance.gameOver && carController.KPH <= 20f && !resultScreen.activeSelf)
        {
            scoreText.text = (carController.DistanceTravelled * 0.1f).ToString("N0") + "M";
            resultScreen.SetActive(true);
        }

        if(resultScreen.activeSelf && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            GameManager.restart = true;
            SceneManager.LoadScene(0);
        }
    }
}
