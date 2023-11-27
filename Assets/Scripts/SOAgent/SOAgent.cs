using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "SOAgentSkill", menuName = "Agent/SOAgentSkill")]
public class SOAgent : ScriptableObject
{
    public AgentType agentType;
    public float speed;
    public float diggSpeed;
    public float attackRate;
    public int health;
    public int damage; 
    public float attackDistance;
    public int cost;
    public int reward;
}
