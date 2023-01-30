using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad()]
public class WaypointGizmo
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmos(Waypoint waypoint, GizmoType gizmoType)
    {
        if((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.blue * 0.5f;
        }

        Gizmos.DrawSphere(waypoint.transform.position, 0.5f);
        Gizmos.color = Color.white;

        // Waypoint width line (AI walkable radius line)
        Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.WaypointWidth /2f),
                        waypoint.transform.position - (waypoint.transform.right * waypoint.WaypointWidth / 2f));

        if(waypoint.Previous != null)
        {
            Gizmos.color = Color.red;
            Vector3 offsetFrom = waypoint.transform.right * waypoint.WaypointWidth / 2f;
            Vector3 offsetTo = waypoint.Previous.transform.right * waypoint.Previous.WaypointWidth / 2f;

            Gizmos.DrawLine(waypoint.transform.position + offsetFrom, waypoint.Previous.transform.position + offsetTo);
        }

        if (waypoint.Next != null)
        {
            Gizmos.color = Color.green;
            Vector3 offsetFrom = -waypoint.transform.right * waypoint.WaypointWidth / 2f;
            Vector3 offsetTo = -waypoint.Next.transform.right * waypoint.Next.WaypointWidth / 2f;

            Gizmos.DrawLine(waypoint.transform.position + offsetFrom, waypoint.Next.transform.position + offsetTo);
        }
    }
}
