using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Agent;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   public GameObject damageTextPrefab;
   public static GameManager Instance;
   public BuildManager buildManager;
   public UIManager uIManager;
   public Domination.Domination dominationArea;
   public Camera mainCamera;
   // public Transform enemyPoint;
   // public Transform soldierPoint;

   public List<AgentBase> enemies;
   public List<AgentBase> soldiers;
   
   [Space(10)]
   [Header("REWARD")]
   [SerializeField] private float goldCount;
   [SerializeField] private float goldRate;
   
   private void Awake()
   {
      SetText();
      if(Instance == null)
      {
         Instance = this;
      }
   }
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

   public void GoDominationArea(bool isEnemy)
   {
      if (isEnemy)
      {
         foreach (var item in enemies)
         {
            item.SetStartTarget();
         }
         
      }
      else
      {
         foreach (var item in soldiers)
         {
            
            item.SetStartTarget();
         }
      }
   }
   private void Update()
   {
      SetText();
      goldCount += goldRate * Time.deltaTime;
   }
   public void SetGoldRate(float a)
   {
      goldRate += a;
   }
   private void SetText()
   {
      uIManager.goldTextCount.text ="Gold =" +(int)goldCount;
      uIManager.timeText.text ="Rate =" + goldRate;
   }
   public void GetReward(float value)
   {
      goldCount += value;
      SetText();
   }
   public float GetGold => goldCount;
   public float GetGoldRate => goldRate;

   public AgentBase GetFurthestAllie()
   {
      if (soldiers == null || soldiers.Count == 0)  return null;
      var far = float.MinValue;
      AgentBase furthest = null;
      foreach (AgentBase obj in soldiers)
      {
         if (obj == null) continue;
         var zPoz = obj.transform.position.z;
         if (!(zPoz > far)) continue;
         far = zPoz;
         furthest = obj;
      }
      return furthest;
   }
}
