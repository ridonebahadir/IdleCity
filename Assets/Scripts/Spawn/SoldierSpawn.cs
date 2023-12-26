
using System.Collections;
using Agent;
using DG.Tweening;
using LeonBrave;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SoldierSpawn : MonoBehaviour
{
    
    [SerializeField] private GameObject soldier;
    [SerializeField] private GameObject soldierArcher;
    [SerializeField] private GameObject soldierDigger;
    [SerializeField] private Transform spawnPointSoldier;
    
    
    [Space(10)]
    [Header("SO AGENT")]
    public SOAgent soldierSo;
    public SOAgent soldierArcherSo; 
    public SOAgent soldierDiggerSo;
    
    
    [Header("COST")] 
     private float _soldierCost;
     private float _soldierArcherCost;
     private float _soldierDiggerCost;

    
    private GameManager _gameManager;

    
    
    [Space(10)]
    [Header("COST TEXT")]
    public TextMeshProUGUI soldierCostText;
    public TextMeshProUGUI soldierArcherCostText;
    public TextMeshProUGUI soldierDiggerCostText;
   
    [Space(10)]
    [Header("TILE")]
    public Image soldierImage;
    public Image soldierArcherImage;
    public Image soldierDiggerImage;

    [Space(10)]
    [Header("BUTTON")]
    public Button spawnSoldierButton;
    public Button spawnSoldierArcherButton;
    public Button spawnSoldierDiggerButton;


    private IEnumerator _soldierEnumerator;
    private IEnumerator _soldierEnumeratorArcher;
    private IEnumerator _soldierEnumeratorDigger;


    private SingletonHandler _singletonHandler;
    private void Start()
    {
        _gameManager = GameManager.Instance;
        _singletonHandler = SingletonHandler.Instance;
        
        spawnSoldierButton.onClick.AddListener(SpawnSoldier);
        spawnSoldierArcherButton.onClick.AddListener(SpawnSoldierArcher);
        spawnSoldierDiggerButton.onClick.AddListener(SpawnSoldierDigger);
      
        _soldierCost = soldierSo.cost;
        _soldierArcherCost = soldierArcherSo.cost;
        _soldierDiggerCost = soldierDiggerSo.cost;

        soldierCostText.text = Mathf.Floor(_soldierCost).ToString();
        soldierArcherCostText.text =  Mathf.Floor(_soldierArcherCost).ToString();
        soldierDiggerCostText.text = Mathf.Floor(_soldierDiggerCost).ToString();
        
        _soldierEnumerator = ControlGold(_soldierCost,spawnSoldierButton,soldierImage);
        _soldierEnumeratorArcher = ControlGold(_soldierArcherCost,spawnSoldierArcherButton,soldierArcherImage);
        _soldierEnumeratorDigger = ControlGold(_soldierDiggerCost,spawnSoldierDiggerButton,soldierDiggerImage);
        
        StartCoroutine(_soldierEnumerator);
        StartCoroutine(_soldierEnumeratorArcher);
        StartCoroutine(_soldierEnumeratorDigger);
    }
    
    IEnumerator ControlGold(float cost,Button button,Image image)
    {
        WaitForSeconds waitForSeconds = new(0.5f);
        while (true)
        {
            if (_gameManager.GetGold<cost)
            {
                button.interactable = false;
                var value =(_gameManager.GetGold/cost);
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
        // ReSharper disable once IteratorNeverReturns
    }

    private void Spawn(float cost,ObjectType objectType,Image image)
    {
        if (_gameManager.GetGold < cost) return;
        var cloneObj = _singletonHandler.GetSingleton<ObjectPool>().TakeObject(objectType);
        //var obj= Instantiate(prefab, pos.position,Quaternion.identity,pos);
        var rand = Random.Range(10, -10);
        cloneObj.transform.localPosition = new Vector3(rand, 0, 0);
        cloneObj.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
        cloneObj.SetActive(true);
        AgentBase agentBase = cloneObj.GetComponent<AgentBase>();
        agentBase.InıtAgent();
        _gameManager.soldiers.Add(agentBase);
        _gameManager.GetReward(-cost);
        Stop();
        image.fillAmount = 0;
        Go();
    }
    private void SpawnSoldier()
    {
        Spawn(_soldierCost,ObjectType.Soldier,soldierImage);
    }
    private void SpawnSoldierArcher()
    {
        Spawn(_soldierArcherCost,ObjectType.SoldierArcher,soldierArcherImage);
    }
    private void SpawnSoldierDigger()
    {
        Spawn(_soldierDiggerCost,ObjectType.SoliderDigger,soldierDiggerImage);
    }

    private void Stop()
    {
        StopCoroutine(_soldierEnumerator);
        StopCoroutine(_soldierEnumeratorArcher);
        StopCoroutine(_soldierEnumeratorDigger);
    }
    private void Go()
    {
        StartCoroutine(_soldierEnumerator);
        StartCoroutine(_soldierEnumeratorArcher);
        StartCoroutine(_soldierEnumeratorDigger);
    }
    
   
}
