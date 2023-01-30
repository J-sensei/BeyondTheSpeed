using System.Linq;
using UnityEngine;

public class RoadData : MonoBehaviour
{
    [Range(0f, 10f)]
    [Tooltip("Diffculty value to determine how difficult the road will be")]
    public int Difficulty = 0;
    [Tooltip("The angle from start to end, angle of the turns. Negative = left; Positive = right")]
    public float TurnAngle = 0f;

    public int WaypointNum = 6;
    public GameObject[] Waypoints;

    /// <summary>
    /// Mesh collider that needs to flip
    /// </summary>
    public GameObject[] FlipMeshColliders;
    public AICarSpawn[] Spawners;

    private void Start()
    {
        Waypoints = new GameObject[WaypointNum];
        for(int i = 0; i < Waypoints.Length; i++)
        {
            Transform waypoint = transform.Find("Waypoints " + (i + 1).ToString());
            if (waypoint != null)
                Waypoints[i] = waypoint.gameObject;
            else
                Debug.Log("[" + name + "][RoadData] Waypoint " + (i + 1).ToString() + " is null");
        }

        // Flip the mesh collider to make the collider work
        if(FlipMeshColliders != null && FlipMeshColliders.Length > 0)
        {
            for(int i = 0; i < FlipMeshColliders.Length; i++)
            {
                MeshFilter mesh = FlipMeshColliders[i].GetComponent<MeshFilter>();
                mesh.mesh.SetIndices(mesh.mesh.GetIndices(0).Concat(mesh.mesh.GetIndices(0).Reverse()).ToArray(), MeshTopology.Triangles, 0);
                FlipMeshColliders[i].GetComponent<MeshCollider>().sharedMesh = mesh.mesh;
            }
        }
    }
}
