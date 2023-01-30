using UnityEngine;

/// <summary>
/// Use to trigger certain events when player is leaving a level chunk
/// </summary>
public class EndpointExit : MonoBehaviour
{
    /// <summary>
    /// Determine if player already triggered the exit
    /// </summary>
    private bool triggered = false;
    private RoadGenerator generator;

    private void Start()
    {
        // Reference to the road generator to use the spawn feature
        //generator = GameObject.Find(GameObjectNames.GAME_MANAGER)
        //                      .GetComponent<GameManager>()
        //                      .RoadGeneratorManager;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" && !triggered)
        {
            generator.Spawn(); // Spawn a new chunk of road
            triggered = true;

            // Make the collider become invisible wall
            Collider col = gameObject.GetComponent<Collider>();
            col.isTrigger = false;
        }
    }
}
