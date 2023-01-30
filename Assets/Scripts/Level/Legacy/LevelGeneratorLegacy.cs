using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorLegacy : MonoBehaviour
{
    public LevelData[] LevelChunks;
    public LevelData InitialChunk;
    public Vector3 SpawnOrigin;
    public int ChunkSpawnInitially = 10; // Chunks that will spawn when the game start

    private LevelData previousChunk;
    private Vector3 spawnPosition;

    private void OnEnable()
    {
        LevelExit.OnExit += PickAndSpawnChunk;
    }

    private void OnDestroy()
    {
        LevelExit.OnExit -= PickAndSpawnChunk;
    }


    private void Start()
    {
        previousChunk = InitialChunk;

        //for(int i = 0; i < ChunkSpawnInitially; i++)
        //{
        //    PickAndSpawnChunk();
        //}
    }


    private void Update()
    {
        // Debug Spawn
        if (Input.GetKeyDown(KeyCode.T))
        {
            PickAndSpawnChunk();
        }
    }

    private LevelData NextChunk()
    {
        List<LevelData> allowedChunks = new List<LevelData>(); // Level chunks that are allow to pick
        LevelData nextChunk = null;
        LevelDirection nextDirection = LevelDirection.North; // Initialize to north first, will overwrite later

        switch (previousChunk.ExitDirection)
        {
            case LevelDirection.North:
                nextDirection = LevelDirection.South; // Next direction entry will be south
                spawnPosition = spawnPosition + new Vector3(0f, 0f, previousChunk.Size.y); // Line up to the previous chunk
                break;
            case LevelDirection.East:
                nextDirection = LevelDirection.West;
                spawnPosition = spawnPosition + new Vector3(previousChunk.Size.x, 0f, 0f);
                break;
            case LevelDirection.South:
                nextDirection = LevelDirection.North;
                spawnPosition = spawnPosition + new Vector3(0f, 0f, -previousChunk.Size.y);
                break;
            case LevelDirection.West:
                nextDirection = LevelDirection.East;
                spawnPosition = spawnPosition + new Vector3(-previousChunk.Size.x, 0f, 0f);
                break;
        }

        // Find matchs levels data
        for(int i = 0; i < LevelChunks.Length; i++)
        {
            if(LevelChunks[i].EntryDirection == nextDirection)
            {
                allowedChunks.Add(LevelChunks[i]);
            }
        }

        nextChunk = allowedChunks[Random.Range(0, allowedChunks.Count)];
        return nextChunk;
    }

    private void PickAndSpawnChunk()
    {
        //Debug.Log("Position: " + spawnPosition + ", Origin: " + SpawnOrigin);
        LevelData spawnChunk = NextChunk();
        GameObject objectChunk = spawnChunk.LevelChunks[Random.Range(0, spawnChunk.LevelChunks.Length)]; // Possible one chunk has multiple level design
        //Debug.Log(objectChunk.name + ": " + objectChunk.transform.Find("ExitPoint").position);
        //spawnPosition.x = objectChunk.transform.Find("ExitPoint").position.x;

        if(previousChunk.Size != spawnChunk.Size)
        {
            spawnPosition.z = spawnChunk.Size.y / 2;
        }

        previousChunk = spawnChunk;
        Instantiate(objectChunk, spawnPosition + SpawnOrigin, Quaternion.identity);
    }

    public void UpdateSpawnOrigin(Vector3 origin)
    {
        SpawnOrigin += origin;
    }
}
