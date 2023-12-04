using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   public BuildManager buildManager;
   public UIManager uIManager;
   public Domination.Domination dominationArea;

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

      //StartCoroutine(GoldSystem());

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

   private void Update()
   {
      SetText();
      goldCount += goldRate * Time.deltaTime;
   }

   private IEnumerator GoldSystem()
   {
      
      while (true)
      {
         for (int i = 0; i < goldRate; i++)
         {
            goldCount++;
            SetText();
           //WaitForSeconds waitForSeconds = new(1/goldRate);
            yield return new WaitForSeconds(1/goldRate);
         }
        
         //yield return waitForSeconds;
      }
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
}
