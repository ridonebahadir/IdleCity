
using System;
using System.Collections.Generic;

using Agent;

using LeonBrave;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
   public SOGameSettings soGameSettings;
   
   public GameObject damageTextPrefab;
   public static GameManager Instance;
  
   public UIManager uIManager;
   public Domination.Domination dominationArea;
   public Camera mainCamera;

   public List<AgentBase> enemies;
   public List<AgentBase> soldiers;
   
   [Space(10)]
   [Header("REWARD")]
   [SerializeField] private float goldCount;
   [SerializeField] private float goldRate;
   [SerializeField] private RectTransform goldCountPos;
   public Transform coinTarget;
   public Transform diamondTarget;
   
   [Header("PANELS")] 
   [SerializeField] GameObject winPanel;
   [SerializeField] GameObject failPanel;
   [SerializeField] private Button restWin;
   [SerializeField] private Button restFail;
   [SerializeField] private Button restButton;
   
   
   
   
   private void Awake()
   {
      Application.targetFrameRate = 300;
      SetText();
      if(Instance == null)
      {
         Instance = this;
      }
      
   }

   private void Start()
   {
      restWin.onClick.AddListener(SceneRest);
      restFail.onClick.AddListener(SceneRest);
      restButton.onClick.AddListener(SceneRest);
      
      for (var i = 0; i < 3; i++)
      {
         randomEnemyTurnList.Add(Random.Range(1, 5));
      }
      uIManager.xpTextCount.text =soGameSettings.xp.ToString();
      uIManager.diamondTextCount.text =soGameSettings.diamond.ToString();
      //Vector3 pos = Camera.main.ViewportToWorldPoint(goldCountPos.position);
      //coinTarget.position = Camera.main.WorldToViewportPoint(pos);
   }

   private int _dieEnemyCount; 
   [SerializeField] private List<int> randomEnemyTurnList = new List<int>();
   [SerializeField] private GameObject diamondPrefab;
   
   public void RemoveList(AgentBase agentBase,AgentType agentType)
   {
      if (agentType==AgentType.Enemy)
      {
         enemies.Remove(agentBase);
         _dieEnemyCount++;
         DiamondSpawn(agentBase.transform.position);
      }

      if (agentType==AgentType.Soldier)
      {
         soldiers.Remove(agentBase);
      }
   }

   private void DiamondSpawn(Vector3 startPos)
   {
      var isValueInList = CheckIfValueInList(_dieEnemyCount);
      if (!isValueInList) return;
      var obj = Instantiate(diamondPrefab,startPos,Quaternion.identity).GetComponent<Coin>();
      obj.gameManager = this;
      obj.singletonHandler = SingletonHandler.Instance;
      obj.Inıt(startPos,1,diamondTarget,false);
   }

   bool CheckIfValueInList(int valueToCheck)
   {
      foreach (var item in randomEnemyTurnList)
      {
         if (item == valueToCheck)
         {
            return true;
         }
      }
      return false;
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
      soGameSettings.xp += (int)(goldRate * Time.deltaTime);
   }
   public void SetGoldRate(float a)
   {
      goldRate += a;
   }
   private void SetText()
   {
      uIManager.goldTextCount.text =Mathf.Floor(goldCount).ToString();
      uIManager.timeText.text =goldRate.ToString("f1")+"/s";
      uIManager.xpTextCount.text =soGameSettings.xp.ToString();
      uIManager.diamondTextCount.text =soGameSettings.diamond.ToString();
   }
   public void GetReward(float value)
   {
      goldCount += value;
      SetText();
      
   }

   public void GetXpReward(int value)
   {
      soGameSettings.xp += value;
      
      
   }

   public void GetDiamondReward(int value)
   {
      soGameSettings.diamond += value;
     
   }
   public float GetGold => goldCount;
   public float GetGoldRate => goldRate;

   public SmallTrigger GetFurthestAllie()
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
      return furthest.small;
   }

   public SmallTrigger GetFurhestEnemies()
   {
      if (enemies == null || enemies.Count == 0)  return null;
      var far = float.MinValue;
      AgentBase furthest = null;
      foreach (AgentBase obj in enemies)
      {
         if (obj == null) continue;
         var zPoz = obj.transform.position.z;
         if (!(zPoz > far)) continue;
         far = zPoz;
         furthest = obj;
      }
      return furthest.small;
   }
   
   public void WinPanelOpen()
   {
      soGameSettings.level++;
      winPanel.SetActive(true);
   }

   public void FailPanelOpen()
   {
      failPanel.SetActive(true);
   }
   private void SceneRest()
   {
      var activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
      SceneManager.LoadScene(activeSceneIndex);
   }
}
