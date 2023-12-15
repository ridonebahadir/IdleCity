using System.Collections;
using System.Collections.Generic;
using Agent;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "SOAgentSkill", menuName = "Agent/SOAgentSkill")]
public class SOAgent : ScriptableObject
{
    public AgentType agentType;
    public float speed;
    public float diggSpeed;
    public float attackRate;
    public float health;
    public float damage; 
    public float attackDistance;
    public float cost;
    public float reward;
}
