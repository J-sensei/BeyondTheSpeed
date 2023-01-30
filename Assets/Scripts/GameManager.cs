using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public bool isNoScore = true;
    public float noScoreTimer = 0f;

    private int score = 0;
    public static bool restart = false;
    public CarController carController;
    public bool start = false;
    public float fuel = 50f;
    public float maxFuel = 100f;
    public float fuelSpend = 20f;
    public float fuelGain = 25f;
    public int level = 1;
    public bool gameOver = false;

    public GameObject[] hubs;
    public GameObject[] menus;
    public GameObject tipsHub;

    public int Score { get { return score; } }
    public float AICarSpeedModifier { get; private set; } = 1.0f;

    public void AddScore(int score)
    {
        this.score += score;
        //Debug.Log("New Score: " + this.score + "(" + score + ")");
    }

    public void Update()
    {
        if (isNoScore)
        {
            noScoreTimer += Time.deltaTime;
        }

        if(isNoScore && noScoreTimer > 7.5f && !gameOver)
        {
            tipsHub.SetActive(true);
        }
        else
        {
            tipsHub.SetActive(false);
        }

        if (!start)
        {
            if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) || restart)
            {
                for(int i = 0; i < hubs.Length; i++)
                {
                    hubs[i].SetActive(true);
                }

                for (int i = 0; i < menus.Length; i++)
                {
                    menus[i].SetActive(false);
                }

                start = true;
            }
            return;
        }

        fuel -= fuelSpend * Time.deltaTime;
        if (fuel <= 0f) gameOver = true;

        if(fuel >= maxFuel)
        {
            level++;
            maxFuel = maxFuel * 1.5f;
            fuel = maxFuel / 2;
            fuelSpend = fuelSpend * 1.7f;
            fuelGain *= 1.5f;
            AudioManager.Instance.PlayClip(AudioManager.Instance.LevelUpClip);
            AICarSpeedModifier += 0.1f;
            AICarSpeedModifier = Mathf.Clamp(AICarSpeedModifier, 0f, 10f);
            carController.MinimumKPH += 10;
        }
    }

    private void FixedUpdate()
    {
        
    }

    protected override void AwakeSingleton()
    {
        
    }
}
