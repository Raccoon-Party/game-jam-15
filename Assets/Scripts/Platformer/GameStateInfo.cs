using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameStateInfo
{
    public long lastUpdated;
    public int deathCounter;
    public Vector2 OverworldPosition;
    public List<string> CompletedLevels;
    public List<string> UnlockedAnimals;
    public List<string> UnlockedCrystals;
    public int Money;
    public GameStateInfo()
    {
        CompletedLevels = new List<string>();
        UnlockedAnimals = new List<string>();
        UnlockedCrystals = new List<string>();
        Money = 0;
        deathCounter = 0;
        OverworldPosition = _defaultPosition;
    }
    public static Vector2 _defaultPosition = new Vector2(-8.0f, 0.1f);
}
