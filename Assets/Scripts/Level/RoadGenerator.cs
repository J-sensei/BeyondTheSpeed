using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    #region Constants
    /// <summary>
    /// Name of the endpoint game object in the road, use to find the child game object of the end point road
    /// </summary>
    const string ENDPOINT_NAME = "Endpoint";
    /// <summary>
    /// Name of the speed trap game object in the road
    /// </summary>
    const string SPEEDTRAP_NAME = "SpeedTrap";
    /// <summary>
    /// Name of the coins game object in the road
    /// </summary>
    const string COIN_NAME = "Coins";
    /// <summary>
    /// Name of the AI Cars game object, holding multiple AI Cars available for a level chunk
    /// </summary>
    const string AI_CAR_NAME = "AI Cars";
    /// <summary>
    /// Name of the wapoint parent in the road
    /// </summary>
    const string WAYPOINTS_NAME = "Waypoints";
    /// <summary>
    /// Game object name of the first waypoint node
    /// </summary>
    const string WAYPOINT_START_NAME = "Start";
    /// <summary>
    /// Game object name of th last waypoint node
    /// </summary>
    const string WAYPOINT_END_NAME = "End";
    #endregion

    #region Serialize Fields
    [Header("Road Game Objects")][Space]
    [Tooltip("First road placed in the scene")]
    [SerializeField] private GameObject InitialRoad;
    [Tooltip("Roads available possible to spawn")]
    [SerializeField]  private RoadData[] RoadDatas;

    [Header("Generator Configuration")][Space]
    [Tooltip("Initial number of road to spawn when the game start")]
    [SerializeField] private int InitialNumberOfRoadsSpawn = 4;
    [Tooltip("Maximum number of road to stay in the level, last road in the level will be destroy if current roads exceed this number")]
    [SerializeField] private int MaximumRoadThreshold = 6;
    [Tooltip("Maximum angle of turn the roads threshold, use to avoid spawn circle turns")]
    [SerializeField] private float MaximumAngleThreshold = 180f;
    [Tooltip("Maximum number of recent road to calculate the sum of angle")]
    [SerializeField] private int MaximumRoadAngleThreshold = 2;
    #endregion

    #region Private Fields
    /// <summary>
    /// Last Road Spawned
    /// </summary>
    private GameObject lastRoad;
    /// <summary>
    /// Roads choosen to spawn
    /// </summary>
    private RoadData[] roadsToSpawn;
    /// <summary>
    /// Queue holding roads available in the level
    /// </summary>
    private Queue<GameObject> currentRoads = new Queue<GameObject>();
    /// <summary>
    /// Queue holding roads angle
    /// </summary>
    private Queue<float> roadAngles = new Queue<float>();
    /// <summary>
    /// Total of the angles (Calculated from Road Angles queue)
    /// </summary>
    private float sumAngle = 0f;
    /// <summary>
    /// Game Manager object
    /// </summary>
    private GameManager gameManager;
    #endregion
    
    void Start()
    {
        gameManager = GameObject.Find(GameObjectNames.GAME_MANAGER).GetComponent<GameManager>(); // Find the game manager in the scene

        lastRoad = InitialRoad; // Assign the last road
        currentRoads.Enqueue(lastRoad); // Added initial road to the queue

        // Initial chunk of roads to spawn
        for (int i = 0; i < InitialNumberOfRoadsSpawn; i++)
        {
            Spawn();
        }
    }

    /// <summary>
    /// Spawn a new road in the level
    /// </summary>
    public void Spawn()
    {
        // Find the endpoint location of the road to be the location to spawn the next road
        Transform spawnTransform = lastRoad.transform.Find(ENDPOINT_NAME); 

        // Road to spawn
        RoadData roadSpawn;

        // NEED TO OPTIMISE
        // Calculate eligible road to spawn based on the difficulty
        //roadsToSpawn = RoadDatas.Where(x => x.Difficulty <= gameManager.DifficultyManager.LevelDifficulty).ToArray();

        // NEEED TO OPTIMISE
        // Do until the sum angle of the road will be spawn do not exceed the threshold
        do
        {
            roadSpawn = roadsToSpawn[Random.Range(0, roadsToSpawn.Length)];
        } 
        while (Mathf.Abs(sumAngle + roadSpawn.TurnAngle) >= MaximumAngleThreshold);

        GameObject gameObjectSpawn = roadSpawn.gameObject; // Get the game object from the road to spawn
        GameObject spawnedObject = Instantiate(gameObjectSpawn, spawnTransform.position, spawnTransform.rotation); // Instantiate the object

        // Set the road object to always active if the visibility is false
        if (!spawnedObject.activeSelf)
        {
            spawnedObject.SetActive(true);
        }

        // Configure the road based on the current difficulty 
        ConfigureDifficulty(spawnedObject);

        // Configure Waypoints
        Waypoint end = lastRoad.transform.Find(WAYPOINTS_NAME).Find(WAYPOINT_END_NAME).GetComponent<Waypoint>(); // Get the last waypoint node of the last road
        Waypoint start = spawnedObject.transform.Find(WAYPOINTS_NAME).Find(WAYPOINT_START_NAME).GetComponent<Waypoint>(); // Get the start waypoint node of the road that just spawned

        if (end != null && start != null)
        {
            // Link the nodes
            end.Next = start;
            start.Previous = end;
        }

        // Add the road angles 
        if (roadAngles.Count < MaximumRoadAngleThreshold)
        {
            roadAngles.Enqueue(roadSpawn.TurnAngle);
        }
        else
        {
            // Dequeue the last road angle as the reaches the maximum road angle threshold
            roadAngles.Dequeue();
            roadAngles.Enqueue(roadSpawn.TurnAngle);
        }

        // Calculate the sum angle
        if (roadAngles.Count > 0)
        {
            sumAngle = 0f;
            foreach (float angle in roadAngles)
            {
                sumAngle += angle;
            }
        }

        // Assign the roads to the queue
        currentRoads.Enqueue(spawnedObject);
        lastRoad = spawnedObject;

        // Take the last road from the queue and destroy if number of current roads is exceed to the threshold
        if(currentRoads.Count > MaximumRoadThreshold)
        {
            Destroy(currentRoads.Dequeue());
        }
    }

    /// <summary>
    /// Configure a road difficulty
    /// </summary>
    /// <param name="road"></param>
    private void ConfigureDifficulty(GameObject road)
    {
        //// Configure the speed trap
        //if (gameManager.DifficultyManager.GetSpawn("Speed Trap"))
        //{
        //    Transform speedTrap = road.transform.Find(SPEEDTRAP_NAME);
        //    speedTrap.gameObject.SetActive(true);

        //    int speedLimit = gameManager.DifficultyManager.GetSpeed();
        //    speedTrap.Find("SpeedTrapCollider").GetComponent<SpeedTrapSnap>().SpeedLimit = speedLimit; // Set the speed limit
        //    speedTrap.Find("Speed Signs").Find(speedLimit.ToString()).gameObject.SetActive(true);
        //    gameManager.DifficultyManager.Reset("Speed Trap");
        //}
        //else
        //{
        //    road.transform.Find(SPEEDTRAP_NAME).gameObject.SetActive(false);
        //}

        //// Configure the coins
        //if (gameManager.DifficultyManager.GetSpawn("Coin"))
        //{
        //    road.transform.Find(COIN_NAME).gameObject.SetActive(true);
        //    gameManager.DifficultyManager.Reset("Coin");
        //}
        //else
        //{
        //    road.transform.Find(COIN_NAME).gameObject.SetActive(false);
        //}

        //// Configure AI Car
        //if (gameManager.DifficultyManager.GetSpawn("AI"))
        //{
        //    road.transform.Find("Test AI Car").gameObject.SetActive(true);
        //    gameManager.DifficultyManager.Reset("AI");
        //}
        //else
        //{
        //    road.transform.Find("Test AI Car").gameObject.SetActive(false);
        //}
    }
}
