using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateInfo
{
    public int deathCounter { get; set; }
    public Vector2 OverworldPosition { get; set; }
    public List<string> CompletedLevels { get; set; }
    public List<string> UnlockedAnimals { get; set; }
    public List<string> UnlockedCrystals { get; set; }
    public int Money { get; set; }
    public GameStateInfo()
    {
        CompletedLevels = new List<string>();
        UnlockedAnimals = new List<string>();
        UnlockedCrystals = new List<string>();
        Money = 0;
        deathCounter = 0;
        OverworldPosition = new Vector2(-8.0f, 0.1f);
    }
    public static Vector2 _defaultPosition = new Vector2(-8.0f, 0.1f);
}
