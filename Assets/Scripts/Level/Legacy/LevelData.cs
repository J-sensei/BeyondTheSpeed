using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    public Vector2 Size = new Vector2(10f, 10f); // X, Z instead of X, Y

    // Level Chunk Data
    public GameObject[] LevelChunks;
    public LevelDirection EntryDirection;
    public LevelDirection ExitDirection;
}

public enum LevelDirection
{
    North, East, South, West
}