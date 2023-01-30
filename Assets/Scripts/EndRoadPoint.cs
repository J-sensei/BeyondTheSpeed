using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoadPoint : MonoBehaviour
{
    /// <summary>
    /// Determine if player already triggered the exit
    /// </summary>
    private bool triggered = false;

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !triggered)
        {
            LevelManager.Instance.Spawn(); // Spawn a new chunk of road
            triggered = true;

            // Make the collider become invisible wall
            //Collider col = gameObject.GetComponent<Collider>();
            //col.isTrigger = false;
        }
    }
}
