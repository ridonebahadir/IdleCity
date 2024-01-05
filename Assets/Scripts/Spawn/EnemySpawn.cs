
using System;
using System.Collections;
using Agent;
using DG.Tweening;
using LeonBrave;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawn : MonoBehaviour
{
    private GameManager _gameManager;
    private Domination.Domination _domination;
    [SerializeField] private int waveCount;
    private SingletonHandler _singletonHandler;
    private int gameLevel;
    
    [Space(10)]
    [Header("Enemy Upgrade")]
    [SerializeField] private SOAgentUpgrade enemyMelee;
    [SerializeField] private SOAgentUpgrade enemyArcher;
    [SerializeField] private SOAgentUpgrade enemyDigger;
    
    [Space(10)]
    [Header("Enemy")]
    [SerializeField] private SOAgent enemyMeleeAgent;
    [SerializeField] private SOAgent enemyArcherAgent;
    [SerializeField] private SOAgent enemyDiggerAgent;

    
    
    
    [Space(10)]
    [Header("Wave")]
    [SerializeField] private int waveTime;
    [SerializeField] private Image waveSlider;
    [SerializeField] private TextMeshProUGUI waveText;

    
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
        _singletonHandler = SingletonHandler.Instance;
        gameLevel = _gameManager.soGameSettings.level;
        SetSlider();
    }

    private void SetSlider()
    {
        waveText.SetText("Wave "+(waveCount).ToString());
        waveSlider.fillAmount = 0;
        waveSlider.DOFillAmount(1, waveTime).OnComplete(() =>
        {
            if (waveCount>=2 && waveCount<10)
            {
                if (waveCount % 2 == 1)
                {
                    enemyMelee.stage++;
                    enemyArcher.stage++;
                    enemyDigger.stage++;
                    SetValue(enemyMelee,enemyMeleeAgent);
                    SetValue(enemyArcher,enemyArcherAgent);
                    SetValue(enemyDigger,enemyDiggerAgent);
                }
            }
            
            
            // m = 2+(w-1)*w/2
            var meleeCount = (2+gameLevel) + (waveCount - 1) * waveCount / 2;
            Spawn(ObjectType.Enemy,meleeCount,enemyMelee,enemyMeleeAgent);
            
            //r = (w-1)*2-1
            var rangeCount = (waveCount - 1) * 2 - 1 + gameLevel;
            Spawn(ObjectType.EnemyArcher,rangeCount,enemyArcher,enemyArcherAgent);
            
            //g = w - 5
            var diggerCount = waveCount - 5+gameLevel;
            Spawn(ObjectType.EnemyDigger,diggerCount,enemyDigger,enemyDiggerAgent);
            
            waveCount++;
            SetSlider();
          
        });
    }
    private void Spawn(ObjectType objectType,int count,SOAgentUpgrade soAgentUpgrade,SOAgent soAgent)
    {
        if (count<=0) return;
        for (var i = 0; i < count; i++)
        {
            var cloneObj = _singletonHandler.GetSingleton<ObjectPool>().TakeObject(objectType);
            //var cloneObj= Instantiate(obj, pos.position,Quaternion.identity,pos);
            var rand = Random.Range(10, -10);
            cloneObj.transform.localPosition = new Vector3(rand, 0, 0);
            if (objectType==ObjectType.EnemyDigger)
            {
                cloneObj.transform.localScale = Vector3.one*2f;
            }
            else
            {
                cloneObj.transform.localScale = Vector3.one*1.75f;
            }
           
            cloneObj.SetActive(true);
            AgentBase agentBase = cloneObj.GetComponent<AgentBase>();
            agentBase.InıtAgent();
            _gameManager.enemies.Add(agentBase);
        }
      
    }

    private void SetValue(SOAgentUpgrade soAgentUpgrade,SOAgent soAgent)
    {
        soAgent.health=Formula(soAgentUpgrade.multipherHealth.a,soAgentUpgrade.multipherHealth.b,soAgentUpgrade.multipherHealth.c,0,soAgentUpgrade);
        soAgent.damage=Formula(soAgentUpgrade.multipherDamage.a,soAgentUpgrade.multipherDamage.b,soAgentUpgrade.multipherDamage.c,0,soAgentUpgrade);
    }
    
    private int Formula(float a, float b, float c,int levelPlus,SOAgentUpgrade soAgentUpgrade)
    {
        // math.ceil((a*(i**2)+b*i+c)*(1.2**((i-1)//5)))
        var i = soAgentUpgrade.stage+levelPlus;
        var formula=Math.Ceiling((a * Math.Pow(i, 2) + b * i + c) * Math.Pow(1.2, (i - 1) / 5));
        return (int)formula;
    }
    private void OnApplicationQuit()
    {
        enemyMeleeAgent.DefaultData();
        enemyArcherAgent.DefaultData();
        enemyDiggerAgent.DefaultData();
        
        enemyMelee.DefaultData();
        enemyArcher.DefaultData();
        enemyDigger.DefaultData();
    }
    
}
