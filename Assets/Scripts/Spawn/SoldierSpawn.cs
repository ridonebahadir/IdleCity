
using UnityEngine;


public class SoldierSpawn : MonoBehaviour
{
    
    [SerializeField] private GameObject soldier;
    [SerializeField] private GameObject soldierArcher;
    [SerializeField] private GameObject soldierDigger;
    [SerializeField] private Transform spawnPointSoldier;
    
    
    [Header("COST")] 
     private int _soldierCost;
     private int _soldierArcherCost;
     private int _soldierDiggerCost;

    
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.uIManager.spawnSoldier.onClick.AddListener(SpawnSoldier);
        _gameManager.uIManager.spawnSoldierArcher.onClick.AddListener(SpawnSoldierArcher);
        _gameManager.uIManager.spawnSoldierDigger.onClick.AddListener(SpawnSoldierDigger);
      
        _soldierCost = _gameManager.soldierSO.cost;
        _soldierArcherCost = _gameManager.soldierArcherSO.cost;
        _soldierDiggerCost = _gameManager.soldierDiggerSO.cost;
    }

    private void SpawnSoldier()
    {
        if (_gameManager.GetGold < _soldierCost) return;
        var obj= Instantiate(soldier, spawnPointSoldier.position,Quaternion.identity,spawnPointSoldier);
        _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
        _gameManager.GetReward(-_soldierCost);

    }
    private void SpawnSoldierArcher()
    {
        if (_gameManager.GetGold < _soldierArcherCost) return;
        var obj= Instantiate(soldierArcher, spawnPointSoldier.position,Quaternion.identity,spawnPointSoldier);
        _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
        _gameManager.GetReward(-_soldierArcherCost);
    }
    private void SpawnSoldierDigger()
    {
        if (_gameManager.GetGold < _soldierDiggerCost) return;
        var obj= Instantiate(soldierDigger, spawnPointSoldier.position,Quaternion.identity,spawnPointSoldier);
        _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
        _gameManager.GetReward(-_soldierDiggerCost);
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
