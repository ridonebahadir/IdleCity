using System;
using System.Collections;
using System.Collections.Generic;
using Agent;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "SOAgentSkill", menuName = "Agent/SOAgentSkill")]
public class SOAgent : ScriptableObject
{
    public AgentType agentType;
    public float speed;
    public float attackRate;
    public float health;
    public float damage; 
    public float attackDistance;
    public float cost;
    public float reward; 
    public float digSpeed;
    public DefaultValue DefaultValue;

    public void DefaultData()
    {
        health = DefaultValue.health;
        damage = DefaultValue.damage;
        digSpeed = DefaultValue.digSpeed;
    }
   
}

[Serializable]
public struct DefaultValue
{
    public float health;
    public float damage;
    public float digSpeed;
}