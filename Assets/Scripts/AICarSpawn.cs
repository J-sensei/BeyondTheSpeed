using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarSpawn : MonoBehaviour
{
    public AICarController[] AICars;
    public Waypoint StartWaypoint;
    public bool Reverse = false;

    public void Spawn()
    {
        AICarController spawnedCar = Instantiate(AICars[Random.Range(0, AICars.Length)], transform.position, transform.rotation);
        //AICarController spawnedCar = Instantiate(AICars[3], transform.position, Quaternion.identity);

        if (Reverse) spawnedCar.Reverse = true;

        spawnedCar.waypoint = StartWaypoint;
    }
}
