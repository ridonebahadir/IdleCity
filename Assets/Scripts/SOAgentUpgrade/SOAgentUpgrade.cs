
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SOAgentUpgrade", menuName = "Agent/SOAgentUpgrade")]
public class SOAgentUpgrade : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public Sprite nextIcon;
    public int cost;
    public List<int> levelBorders;
    internal int multipher;
    internal int levelCount;
    
    public DefaultValueUpgrade DefaultValueUpgrade;
    
    public void DefaultData()
    {
        cost = DefaultValueUpgrade.cost;
        multipher = DefaultValueUpgrade.multipher;
        levelCount = DefaultValueUpgrade.levelCount;
    }
}
[Serializable]
public struct DefaultValueUpgrade
{
    public int cost;
    public int multipher;
    public int levelCount;
   
}
