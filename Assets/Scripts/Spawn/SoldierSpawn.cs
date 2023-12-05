
using System.Collections;
using System.Diagnostics;
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
     private float _soldierCost;
     private float _soldierArcherCost;
     private float _soldierDiggerCost;

    
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


    private IEnumerator _soldierEnumerator;
    private IEnumerator _soldierEnumeratorArcher;
    private IEnumerator _soldierEnumeratorDigger;
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
    }

    private void Spawn(float cost,GameObject prefab,Transform pos,Image image)
    {
        if (_gameManager.GetGold < cost) return;
        var obj= Instantiate(prefab, pos.position,Quaternion.identity,pos);
        obj.transform.localScale = new Vector3(2, 2, 2);
        _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
        _gameManager.GetReward(-cost);
        Stop();
        image.fillAmount = 0;
        Go();
    }
    private void SpawnSoldier()
    {
        Spawn(_soldierCost,soldier,spawnPointSoldier,soldierImage);
        
       //  if (_gameManager.GetGold < _soldierCost) return;
       //  var obj= Instantiate(soldier, spawnPointSoldier.position,Quaternion.identity,spawnPointSoldier);
       //  _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
       //  _gameManager.GetReward(-_soldierCost);
       //  Stop();
       //  soldierImage.fillAmount = 0;
       // Go();
        
    }
    private void SpawnSoldierArcher()
    {
        Spawn(_soldierArcherCost,soldierArcher,spawnPointSoldier,soldierArcherImage);
       //  if (_gameManager.GetGold < _soldierArcherCost) return;
       //  var obj= Instantiate(soldierArcher, spawnPointSoldier.position,Quaternion.identity,spawnPointSoldier);
       //  _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
       //  _gameManager.GetReward(-_soldierArcherCost);
       // Stop();
       //  soldierArcherImage.fillAmount = 0;
       //  Go();
    }
    private void SpawnSoldierDigger()
    {
        Spawn(_soldierDiggerCost,soldierDigger,spawnPointSoldier,soldierDiggerImage);
        // if (_gameManager.GetGold < _soldierDiggerCost) return;
        // var obj= Instantiate(soldierDigger, spawnPointSoldier.position,Quaternion.identity,spawnPointSoldier);
        // _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
        // _gameManager.GetReward(-_soldierDiggerCost);
        // Stop();
        // soldierDiggerImage.fillAmount = 0;
        // Go();

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
