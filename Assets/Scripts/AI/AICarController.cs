using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 5f;
    public float TurningSpeed = 50f;
    public float BreakSpeed = 12f;


    [Header("Waypoint Destination")]
    public Vector3 Destination;
    public bool DestinationReached;
    public Waypoint waypoint;
    public bool GoToNextWaypoint = true;
    public bool Reverse = false;
    public bool ShouldDrive = true;

    private void Start()
    {
        LocateDestination(waypoint.GetPosition());
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.gameOver || !GameManager.Instance.start) return;
        Drive();

        if (DestinationReached && !Reverse)
        {
            if (GoToNextWaypoint && waypoint.Next != null)
            {
                waypoint = waypoint.Next;
            }
            else if(waypoint.Previous != null)
            {
                // Loop back
                //Destroy(gameObject);
                transform.position = waypoint.Previous.GetPosition();
                waypoint = waypoint.Previous.Next;
                //waypoint = waypoint.Previous;
            }
            else
            {
                Destroy(gameObject);
            }

            if(waypoint != null)
            {
                LocateDestination(waypoint.GetPosition());
            }
        }
        else if (DestinationReached && Reverse)
        {
            if (waypoint.Previous != null)
            {
                waypoint = waypoint.Previous;
            }
            else
            {
                //Destroy(gameObject);
                //Debug.Log("Reverse but previous waypoint is null - destroyyed the object");
            }

            if (waypoint != null)
            {
                LocateDestination(waypoint.GetPosition());
            }
        }
    }

    public void LocateDestination(Vector3 destination)
    {
        Destination = destination;
        DestinationReached = false;
    }

    public void Drive()
    {
        if(transform.position != Destination && ShouldDrive)
        {
            Vector3 direction = Destination - transform.position;
            direction.y = 0;

            float distance = direction.magnitude;

            if(distance >= BreakSpeed)
            {
                DestinationReached = false;

                // Rotate AI Vehicle
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, TurningSpeed * Time.fixedDeltaTime);

                // Move AI Vehicle
                transform.Translate(Vector3.forward * MoveSpeed * GameManager.Instance.AICarSpeedModifier * Time.fixedDeltaTime);
            }
            else
            {
                DestinationReached = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    Destroy(this); // Destroy the AI behavior when colliding with the player
        //}
        Destroy(this);
    }
}
