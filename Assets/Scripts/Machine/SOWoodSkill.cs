using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SOWood", order = 1)]
public class SOWoodSkill : ScriptableObject
{
    public SOWaterSkill waterSkill;
    public float machineSpeed;
    public float humanCount; 
    public GameObject outPutObj;
    public GameObject inPutObj;
    public float defaultTime;
    public int earnMoneyRate; //Satarken kaçar adeet satılacağı

}