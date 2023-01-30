using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaypointManager : EditorWindow
{
    /// <summary>
    /// Under window tab, AI Waypoint Editor Tools section will be available 
    /// </summary>
    [MenuItem("Window/AI Waypoint Editor Tools")]
    public static void ShowWindow()
    {
        GetWindow<WaypointManager>("AI Waypoint Editor Tools"); // Open new window and name it AI Waypoint Editor Tools

    }

    [Tooltip("Parent of all waypoints of a section")]
    public Transform WaypointsParent;

    private void OnGUI()
    {
        SerializedObject serializedObj = new SerializedObject(this);

        EditorGUILayout.PropertyField(serializedObj.FindProperty("WaypointsParent")); // Create field for Waypoints Parent
        
        if(WaypointsParent == null)
        {
            // Render a message box
            EditorGUILayout.HelpBox("Assign a waypoints parent to start", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            CreateWaypoint();
            EditorGUILayout.EndVertical();
        }

        serializedObj.ApplyModifiedProperties();
    }

    private void CreateWaypoint()
    {
        // Render a button and if its pressed by user
        if(GUILayout.Button("Create Waypoint"))
        {
            GameObject newWaypointObject = new GameObject("Waypoint " + WaypointsParent.childCount, typeof(Waypoint));
            newWaypointObject.transform.SetParent(WaypointsParent, false);

            Waypoint waypoint = newWaypointObject.GetComponent<Waypoint>();

            // Automatically assign previous / next waypoint
            if(WaypointsParent.childCount > 1)
            {
                waypoint.Previous = WaypointsParent.GetChild(WaypointsParent.childCount - 2).GetComponent<Waypoint>();
                waypoint.Previous.Next = waypoint;

                waypoint.transform.position = waypoint.Previous.transform.position;
                waypoint.transform.forward = waypoint.Previous.transform.forward;
            }
                
            Selection.activeGameObject = waypoint.gameObject; // Select it automatically
        }
    }
}
