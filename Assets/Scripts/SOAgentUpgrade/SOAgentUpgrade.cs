
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "SOAgentUpgrade", menuName = "Agent/SOAgentUpgrade")]
public class SOAgentUpgrade : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public Sprite nextIcon;
    public int cost;
    //public List<int> levelBorders;
    public int level; 
    public int stage;
    public int stageCount;
    public List<Sprite> Sprites;
    
    public DefaultValueUpgrade DefaultValueUpgrade;

    public Sprite SetSprite()
    {
        return Sprites[level-1];
    }
    public void DefaultData()
    {
        cost = DefaultValueUpgrade.cost;
        level = DefaultValueUpgrade.stage;
        stage = DefaultValueUpgrade.level;
    }
}
[Serializable]
public struct DefaultValueUpgrade
{
    public int cost; 
    public int stage; 
    public int level;
   
}
