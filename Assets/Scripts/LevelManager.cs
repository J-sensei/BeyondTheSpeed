using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Road Game Objects")]
    [Space]
    [Tooltip("First road placed in the scene")]
    [SerializeField] private RoadData InitialRoad;
    [Tooltip("Roads available possible to spawn")]
    [SerializeField] private RoadData[] RoadDatas;
    [SerializeField] private int MaxRoads = 10;

    /// <summary>
    /// Last Road Spawned
    /// </summary>
    private RoadData lastRoad;
    /// <summary>
    /// Queue holding roads available in the level
    /// </summary>
    private Queue<GameObject> currentRoads = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        lastRoad = InitialRoad; // Assign the last road
        currentRoads.Enqueue(lastRoad.gameObject); // Added initial road to the queue
        //Debug.Log("Last Road waypoint: " + lastRoad.Waypoints.Length);
        for (int i = 0; i < 5; i++)
        {
            Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.T))
        //{
        //    Spawn();
        //}
    }

    public void Spawn()
    {
        // Find the endpoint location of the road to be the location to spawn the next road
        Transform spawnTransform = lastRoad.transform.Find("End");

        // Road to spawn
        RoadData roadSpawn;

        // NEED TO OPTIMISE
        // Calculate eligible road to spawn based on the difficulty
        roadSpawn = RoadDatas[0];

        // NEEED TO OPTIMISE
        // Do until the sum angle of the road will be spawn do not exceed the threshold
        //do
        //{
        //    roadSpawn = roadsToSpawn[Random.Range(0, roadsToSpawn.Length)];
        //}
        //while (Mathf.Abs(sumAngle + roadSpawn.TurnAngle) >= MaximumAngleThreshold);

        GameObject gameObjectSpawn = roadSpawn.gameObject; // Get the game object from the road to spawn
        GameObject spawnedObject = Instantiate(gameObjectSpawn, spawnTransform.position, spawnTransform.rotation); // Instantiate the object

        // Set the road object to always active if the visibility is false
        if (!spawnedObject.activeSelf)
        {
            spawnedObject.SetActive(true);
        }

        // Configure the road based on the current difficulty 
        //ConfigureDifficulty(spawnedObject);

        // Configure Waypoints
        //Waypoint end = lastRoad.transform.Find(WAYPOINTS_NAME).Find(WAYPOINT_END_NAME).GetComponent<Waypoint>(); // Get the last waypoint node of the last road
        //Waypoint start = spawnedObject.transform.Find(WAYPOINTS_NAME).Find(WAYPOINT_START_NAME).GetComponent<Waypoint>(); // Get the start waypoint node of the road that just spawned

        //if (end != null && start != null)
        //{
        //    // Link the nodes
        //    end.Next = start;
        //    start.Previous = end;
        //}

        RoadData spawnedRoad = spawnedObject.GetComponent<RoadData>();
        for (int i = 0; i < lastRoad.Waypoints.Length; i++)
        {
            Waypoint end = lastRoad.Waypoints[i].transform.Find("End").GetComponent<Waypoint>();
            Waypoint start = spawnedRoad.Waypoints[i].transform.Find("Start").GetComponent<Waypoint>();

            if (end != null && start != null)
            {
                // Link the nodes
                end.Next = start;
                start.Previous = end;
            }
        }

        //// Add the road angles 
        //if (roadAngles.Count < MaximumRoadAngleThreshold)
        //{
        //    roadAngles.Enqueue(roadSpawn.TurnAngle);
        //}
        //else
        //{
        //    // Dequeue the last road angle as the reaches the maximum road angle threshold
        //    roadAngles.Dequeue();
        //    roadAngles.Enqueue(roadSpawn.TurnAngle);
        //}

        // Calculate the sum angle
        //if (roadAngles.Count > 0)
        //{
        //    sumAngle = 0f;
        //    foreach (float angle in roadAngles)
        //    {
        //        sumAngle += angle;
        //    }
        //}

        if(spawnedRoad.Spawners != null)
        {
            for (int i = 0; i < spawnedRoad.Spawners.Length; i++)
            {
                int random = Random.Range(0, 2);
                if(random == 1)
                {
                    spawnedRoad.Spawners[i].Spawn();
                }
            }
        }

        // Assign the roads to the queue
        currentRoads.Enqueue(spawnedObject);
        lastRoad = spawnedObject.GetComponent<RoadData>();

        // Take the last road from the queue and destroy if number of current roads is exceed to the threshold
        if (currentRoads.Count > MaxRoads)
        {
            Destroy(currentRoads.Dequeue());
        }
    }

    protected override void AwakeSingleton()
    {
        
    }
}
