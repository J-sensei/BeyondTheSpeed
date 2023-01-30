using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarScore : MonoBehaviour
{
    private bool scoreCounted = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !scoreCounted && !GameManager.Instance.gameOver)
        {
            scoreCounted = true;
            float distance = Vector3.Distance(transform.position, other.transform.position);
            
            if(distance < 6.5f)
            {
                //GameManager.Instance.AddScore(1);
                //Debug.Log("Pass distance: " + distance);
                //Debug.Log("Pass distance: " + distance);
                //GameManager.Instance.fuel += 5f;
                GameManager.Instance.fuel += GameManager.Instance.fuelGain;
                AudioManager.Instance.PlayClip(AudioManager.Instance.AddFuelClip);

                if (GameManager.Instance.isNoScore) GameManager.Instance.isNoScore = false;
            }

            if (distance < 5.0f)
            {
                GameManager.Instance.carController.CurrentNitroBar += 5f;
            }
        }
    }
}
