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
   [SerializeField]
   public float goldCount;
   [SerializeField] private float goldRate;
   
 
  
   
   
  

   
  
   private void Awake()
   {
      SetText();
      if(Instance == null)
      {
         Instance = this;
      }

      StartCoroutine(GoldSystem());

   }

   private void Start()
   {
      


      
      // StartCoroutine(ControlGold(_soldierCost,uIManager.spawnSoldier,uIManager.soldierImage));
      // StartCoroutine(ControlGold(_soldierArcherCost,uIManager.spawnSoldierArcher,uIManager.soldierArcherImage));
      // StartCoroutine(ControlGold(_soldierDiggerCost,uIManager.spawnSoldierDigger,uIManager.soldierDiggerImage));


      

     
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
   public float GetGold => goldCount;
}
