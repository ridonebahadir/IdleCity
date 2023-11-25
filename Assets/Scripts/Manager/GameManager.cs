using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   public BuildManager buildManager;
   public UIManager uIManager;

   public List<AgentBase> enemies;
   public List<AgentBase> soldiers;
   public Domination dominationArea;
   private void Awake()
   {
      if(Instance == null)
      {
         Instance = this;
      }
      
     
   }
   
   
   // public Transform CloseAgentEnemy(Transform who)
   // {
   //    return enemies.OrderBy(go => (who.position - go.transform.position).sqrMagnitude).First().transform;
   // }
   // public Transform CloseAgentSoldier(Transform who)
   // {
   //    return soldiers.OrderBy(go => (who.position - go.transform.position).sqrMagnitude).First().transform;
   // }

   public void RemoveList(AgentBase agentBase,AgentType agentType)
   {
      if (agentType==AgentType.Enemy)
      {
         enemies.Remove(agentBase);
      }

      if (agentType==AgentType.Soldier)
      {
         soldiers.Remove(agentBase);
      }
   }
   
}
