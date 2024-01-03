using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SOGameSettings", order = 1)]
public class SOGameSettings : ScriptableObject
{
    public int level;
    public int xp;
    public int diamond;

    public DefaultGameSettings DefaultGameSettings;
    
    public void DefaultData()
    {
        level = DefaultGameSettings.level;
        xp = DefaultGameSettings.xp;
        diamond = DefaultGameSettings.diamond;
    }
}

[Serializable]
public struct DefaultGameSettings
{
    public int level;
    public int xp;
    public int diamond;
}
