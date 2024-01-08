
using System;
using System.Collections.Generic;

using Agent;
using DG.Tweening;
using LeonBrave;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
   public SOGameSettings soGameSettings;
   public List<SOAchievement> SoAchievements;
   
   public GameObject damageTextPrefab;
   public static GameManager Instance;
  
   public UIManager uIManager;
   public Domination.Domination dominationArea;
   public Camera mainCamera;

   public List<AgentBase> enemies;
   public List<AgentBase> soldiers;
   private bool _isGame = true;
   
   
   [Space(10)]
   [Header("REWARD")]
   [SerializeField] private float goldCount;
   [SerializeField] private float xpCount;
   [SerializeField] private float diamondCount;
   [SerializeField] private float goldRate;
   [SerializeField] private RectTransform goldCountPos;

   [SerializeField] private float totalGold;
   [SerializeField] private float totalDiemond;
   
   public Transform coinTarget;
   public Transform diamondTarget;
   
   [Header("PANELS")] 
   [SerializeField] GameObject winPanel; 
   [SerializeField] TextMeshProUGUI xpEarnText;
   [SerializeField] TextMeshProUGUI xpEarnFailText;
   [SerializeField] TextMeshProUGUI diemondEarnText;
   [SerializeField] TextMeshProUGUI diemondEarnFailText;
   [SerializeField] GameObject failPanel;
   [SerializeField] private Button restWin;
   [SerializeField] private Button restFail;
   [SerializeField] private Button restButton;
   
   
   [Space(10)] [Header("REWARD")] 
   public TextMeshProUGUI goldTextCount;
   public TextMeshProUGUI xpTextCount;
   public TextMeshProUGUI diamondTextCount;
   public TextMeshProUGUI timeText;

   [Space(10)] [Header("SO ENEMY UPGRADE")] 
   [SerializeField] private SOAgentUpgrade enemyMeleeUpgrade;
   [SerializeField] private SOAgentUpgrade enemyArcherUpgrade;
   [SerializeField] private SOAgentUpgrade enemyDiggerUpgrade;

   [Space(10)] [Header("MainMenuReward")]
   public TextMeshProUGUI totalXpTextCount;
   public TextMeshProUGUI totalDiamondTextCount;
   
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

      SetTotalXp((int)xpCount);
      SetTotalDiamond((int)diamondCount);
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
      if (!_isGame) return;
      SetText();
      goldCount += goldRate * Time.deltaTime;
      xpCount +=goldRate * Time.deltaTime;
   }
      
   public void SetGoldRate(float a)
   {
      goldRate += a;
   }
   private void SetText()
   {
      goldTextCount.text =Mathf.Floor(goldCount).ToString();
      xpTextCount.text =Mathf.Floor(xpCount).ToString();
      diamondTextCount.text =Mathf.Floor(diamondCount).ToString();
      timeText.text =goldRate.ToString("f2")+"/s";
      
   }
   
   public void GetReward(float value)
   {
      goldCount += value;
      SetText();
      
   }

   private Tween _tweenXp;
   private Tween _tweenDiamond;
   public void GetXpReward(int value)
   {
      xpCount += value;
      if (_tweenXp!=null) return;
      xpTextCount.color = Color.yellow;
      _tweenXp=xpTextCount.rectTransform.DOPunchScale(Vector3.one*0.75f,0.25f).OnComplete(() =>
      {
         xpTextCount.color = Color.white;
         _tweenXp = null;
      });
      
   }

   public void GetDiamondReward(int value)
   {
      diamondCount += value;
      if (_tweenDiamond!=null) return;
      diamondTextCount.color = Color.yellow;
      _tweenDiamond=diamondTextCount.rectTransform.DOPunchScale(Vector3.one*0.75f,0.25f).OnComplete(() =>
      {
         diamondTextCount.color = Color.white;
         _tweenDiamond = null;
      });
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
      _isGame = false;
      soGameSettings.level++;
      winPanel.SetActive(true);
      xpEarnText.SetText(Mathf.Floor(xpCount).ToString());
      diemondEarnText.SetText(Mathf.Floor(diamondCount).ToString());
      SetTotalXp((int)xpCount);
      SetTotalDiamond((int)diamondCount);
   }

   public void FailPanelOpen()
   {
      _isGame = false;
      failPanel.SetActive(true);
      xpEarnFailText.SetText(Mathf.Floor(xpCount).ToString());
      diemondEarnFailText.SetText(Mathf.Floor(diamondCount).ToString());
      SetTotalXp((int)xpCount);
      SetTotalDiamond((int)diamondCount);
   }

   public void SetTotalXp(int value)
   {
      soGameSettings.totalXp += value;
      totalXpTextCount.SetText(soGameSettings.totalXp.ToString());
     
      OnXpChange?.Invoke();
   }

   public void SetTotalDiamond(int value)
   {
      soGameSettings.totalDiamond += value;
      totalDiamondTextCount.SetText(soGameSettings.totalDiamond.ToString());
      
      OnDiamondChange?.Invoke();
   }
   
   private void SceneRest()
   {
      OnSceneRest?.Invoke();
      enemyMeleeUpgrade.DefaultData();
      enemyArcherUpgrade.DefaultData();
      enemyDiggerUpgrade.DefaultData();
      var activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
      SceneManager.LoadScene(activeSceneIndex);
   }
   private void OnApplicationQuit()
   {
      soGameSettings.DefaultData();
      foreach (var item in SoAchievements)
      {
       item.DefaultData();  
      }

   }

   public Action OnXpChange;
   public Action OnDiamondChange;
   public Action OnSceneRest;
}
