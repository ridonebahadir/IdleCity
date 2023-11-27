using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
   
   [Space(10)]
   [Header("SOAGENT")]
   public SOAgent soldierSO;
   public SOAgent soldierArcherSO;
   public SOAgent soldierDiggerSO;
   
   
   private int _soldierCost;
   private int _soldierArcherCost;
   private int _soldierDiggerCost;
   
  
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
      _soldierCost = soldierSO.cost;
      _soldierArcherCost = soldierArcherSO.cost;
      _soldierDiggerCost = soldierDiggerSO.cost;

      uIManager.soldierCostText.text = _soldierCost+"G";
      uIManager.soldierArcherCostText.text = _soldierArcherCost+"G";
      uIManager.soldierDiggerCostText.text = _soldierDiggerCost+"G";

      StartCoroutine(ControlGold(_soldierCost,uIManager.spawnSoldier,uIManager.soldierImage));
      StartCoroutine(ControlGold(_soldierArcherCost,uIManager.spawnSoldierArcher,uIManager.soldierArcherImage));
      StartCoroutine(ControlGold(_soldierDiggerCost,uIManager.spawnSoldierDigger,uIManager.soldierDiggerImage));
   }

   IEnumerator ControlGold(int cost,Button button,Image image)
   {
      WaitForSeconds waitForSeconds = new(1);
      while (true)
      {
         if (goldCount<cost)
         {
            button.interactable = false;
            var value =(goldCount/cost);
            image.DOFillAmount(value, 0.5f);
            //image.fillAmount = value;
         }
         else
         {
            image.DOFillAmount(1, 0.5f).OnComplete(() =>
            {
               button.interactable = true;
            });
            
         }

         yield return waitForSeconds;
      }
   }
   private void Update()
   {
      // ControlGold(_soldierCost,uIManager.spawnSoldier,uIManager.soldierImage);
      // ControlGold(_soldierArcherCost,uIManager.spawnSoldierArcher,uIManager.soldierArcherImage);
      // ControlGold(_soldierDiggerCost,uIManager.spawnSoldierDigger,uIManager.soldierDiggerImage);
      
   }

   // private void ControlGold(int cost,Button button,Image image)
   // {
   //    if (goldCount<cost)
   //    {
   //       button.interactable = false;
   //       var value =(goldCount /cost);
   //       image.fillAmount = value;
   //    }
   //    else
   //    {
   //       image.fillAmount = 1;
   //       button.interactable = true;
   //    }
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
