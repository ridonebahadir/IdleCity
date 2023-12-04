using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SOSkillSettings", order = 1)]
public class SOSkillSettings : ScriptableObject
{
    [Header("Freeze Water")] 
    public float freezeWaterActiveTime;
    public float freezeWaterCoolTime;
    
    [Header("Heal Allies")] 
    public float healPercent;
    public float healCoolTime;
    
    [Header("Slow Enemies")] 
    public float slowPercent;
    public float slowActiveTime;
    public float slowCoolTime;
    
    [Header("Attack Buff")] 
    public float attackPercent;
    public float attackActiveTime;
    public float attackCoolTime;
    
    [Header("Deal Damage")] 
    public float dealDamagePercent;
    public float dealDamageCoolTime;
    
    [Header("Increase Gold")] 
    public float increaseGolPercent;
    public float increaseGoldActiveTime;
    public float increaseGoldCoolTime;
    

}