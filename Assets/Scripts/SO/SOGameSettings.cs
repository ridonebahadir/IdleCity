using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SOGameSettings", order = 1)]
public class SOGameSettings : ScriptableObject
{
    public int level; 
    public int totalXp;
    public int totalDiamond;

    public DefaultGameSettings DefaultGameSettings;
    
    public void DefaultData()
    {
        level = DefaultGameSettings.level;
        totalXp = DefaultGameSettings.totalXp;
        totalDiamond = DefaultGameSettings.totalDiamond;
    }
}

[Serializable]
public struct DefaultGameSettings
{
    public int level;
    public int totalXp;
    public int totalDiamond;
}
