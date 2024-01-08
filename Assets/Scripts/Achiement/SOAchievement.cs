using System;
using LeonBrave;
using UnityEngine;


[CreateAssetMenu(fileName = "SOAchievement", menuName = "SOAchievement")]
public class SOAchievement : ScriptableObject
{
    public Sprite icon;
    public string missionInfo;
    public int value;
    public int maxValue;
    public int reward;
    public bool did;
    public DefaultAchievement DefaultAchievement;

    public void SetValue(int addValue)
    {
        value += addValue;
        if (value>=maxValue)
        {
            value = maxValue;
        }
    }

    public bool ControlMission()
    {
        if (did) return false;
        return value>=maxValue;
    }
    
    public void DefaultData()
    {
        value = DefaultAchievement.value;
        did = DefaultAchievement.did;

    }
}
[Serializable]
public struct DefaultAchievement
{
    public int value;
    public bool did;

}
   

