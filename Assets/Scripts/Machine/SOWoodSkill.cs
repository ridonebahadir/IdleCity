using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SOWood", order = 1)]
public class SOWoodSkill : ScriptableObject
{
    public SOWaterSkill waterSkill;
    
    public GameObject outPutObj;
    public GameObject inPutObj;
    
    [Header("Machine Settings")]
    public float defaultTime;
    public float machineSpeed;
    public float humanCount; 
    
    [Header("Earn Money Settings")]
    public float earnMoneyTime; //Satarken beklenen süre
    public int earnMoneyRate; //Satarken kaçar adeet satılacağı

}