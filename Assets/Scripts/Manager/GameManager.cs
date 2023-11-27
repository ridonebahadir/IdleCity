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
   public Domination.Domination dominationArea;

   public List<AgentBase> enemies;
   public List<AgentBase> soldiers;
   
   [Space(10)]
   [Header("REWARD")]
   [SerializeField] private int goldCount;
   [SerializeField] private int goldRate;

   
   private void Awake()
   {
      SetText();
      if(Instance == null)
      {
         Instance = this;
      }

      StartCoroutine(GoldSystem());
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

   private IEnumerator GoldSystem()
   {
      WaitForSeconds waitForSeconds = new(1);
      while (true)
      {
         goldCount+=goldRate;
         SetText();
         yield return waitForSeconds;
      }
   }

   public void SetGoldRate(int a)
   {
      goldRate += a;
   }
   private void SetText()
   {
     
      uIManager.goldTextCount.text ="Gold =" + goldCount;
      uIManager.timeText.text ="Rate =" + goldRate;
   }

   public void GetReward(int value)
   {
      goldCount += value;
      SetText();
   }
}
