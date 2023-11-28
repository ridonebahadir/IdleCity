
using System.Collections;
using DG.Tweening;
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
     private int _soldierCost;
     private int _soldierArcherCost;
     private int _soldierDiggerCost;

    
    private GameManager _gameManager;
    private UIManager _uiManager;
    
    
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


    private IEnumerator _enemyEnumerator;
    private IEnumerator _enemyEnumeratorArcher;
    private IEnumerator _enemyEnumeratorDigger;
    private void Start()
    {
        _gameManager = GameManager.Instance;
        _uiManager = _gameManager.uIManager;
       
        spawnSoldierButton.onClick.AddListener(SpawnSoldier);
        spawnSoldierArcherButton.onClick.AddListener(SpawnSoldierArcher);
        spawnSoldierDiggerButton.onClick.AddListener(SpawnSoldierDigger);
      
        _soldierCost = soldierSo.cost;
        _soldierArcherCost = soldierArcherSo.cost;
        _soldierDiggerCost = soldierDiggerSo.cost;

        soldierCostText.text = _soldierCost + "G";
        soldierArcherCostText.text = _soldierArcherCost + "G";
        soldierDiggerCostText.text = _soldierDiggerCost + "G";
        
        _enemyEnumerator = ControlGold(_soldierCost,spawnSoldierButton,soldierImage);
        _enemyEnumeratorArcher = ControlGold(_soldierArcherCost,spawnSoldierArcherButton,soldierArcherImage);
        _enemyEnumeratorDigger = ControlGold(_soldierDiggerCost,spawnSoldierDiggerButton,soldierDiggerImage);
        
        StartCoroutine(_enemyEnumerator);
        StartCoroutine(_enemyEnumeratorArcher);
        StartCoroutine(_enemyEnumeratorDigger);
    }
    
    IEnumerator ControlGold(int cost,Button button,Image image)
    {
        WaitForSeconds waitForSeconds = new(1);
        while (true)
        {
            if (_gameManager.goldCount<cost)
            {
                button.interactable = false;
                var value =(_gameManager.goldCount/cost);
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

    private void SpawnSoldier()
    {
        if (_gameManager.GetGold < _soldierCost) return;
        var obj= Instantiate(soldier, spawnPointSoldier.position,Quaternion.identity,spawnPointSoldier);
        _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
        _gameManager.GetReward(-_soldierCost);
        StopCoroutine(_enemyEnumerator); 
        soldierImage.fillAmount = 0;
        StartCoroutine(_enemyEnumerator);
        
    }
    private void SpawnSoldierArcher()
    {
        if (_gameManager.GetGold < _soldierArcherCost) return;
        var obj= Instantiate(soldierArcher, spawnPointSoldier.position,Quaternion.identity,spawnPointSoldier);
        _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
        _gameManager.GetReward(-_soldierArcherCost);
        StopCoroutine(_enemyEnumeratorArcher); 
        soldierArcherImage.fillAmount = 0;
        StartCoroutine(_enemyEnumeratorArcher);
    }
    private void SpawnSoldierDigger()
    {
        if (_gameManager.GetGold < _soldierDiggerCost) return;
        var obj= Instantiate(soldierDigger, spawnPointSoldier.position,Quaternion.identity,spawnPointSoldier);
        _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
        _gameManager.GetReward(-_soldierDiggerCost);
        StopCoroutine(_enemyEnumeratorDigger);
        soldierDiggerImage.fillAmount = 0;
        StartCoroutine(_enemyEnumeratorDigger);
       
    }
   
    
    private void OnEnable()
    {
      
        UIManager.OnClickedSoldierSpawnButton += SpawnSoldier;
        UIManager.OnClickedSoldierArcherSpawnButton += SpawnSoldierArcher;
        UIManager.OnClickedSoldierDiggerSpawnButton += SpawnSoldierDigger;
       
    }

    private void OnDisable()
    {
        UIManager.OnClickedSoldierSpawnButton -= SpawnSoldier;
        UIManager.OnClickedSoldierArcherSpawnButton -= SpawnSoldierArcher;
        UIManager.OnClickedSoldierDiggerSpawnButton -= SpawnSoldierDigger;
       
    }
}
