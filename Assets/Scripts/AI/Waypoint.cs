using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Waypoint Status")]
    [Tooltip("Previous waypoint")]
    public Waypoint Previous;
    [Tooltip("Next waypoint that AI will go")]
    public Waypoint Next;

    [Range(0f, 100f)]
    [Tooltip("Width of radius of the waypoint AI will choose to go")]
    public float WaypointWidth = 1f;

    public Vector3 GetPosition()
    {
        Vector3 min = transform.position + transform.right * WaypointWidth / 2f;
        Vector3 max = transform.position - transform.right * WaypointWidth / 2f;

        return Vector3.Lerp(min, max, Random.Range(0f, 1f)); // Randomly get position between min and max
    }
}
