
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject soldier;
    [SerializeField] private GameObject enemyArcher;
    [SerializeField] private GameObject soldierArcher;
    [SerializeField] private GameObject enemyDigger;
    [SerializeField] private GameObject soldierDigger;

    [SerializeField] private Transform spawnPointEnemy;
    [SerializeField] private Transform spawnPointSoldier;
    
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.uIManager.spawnEnemy.onClick.AddListener(SpawnEnemy);
        _gameManager.uIManager.spawnSoldier.onClick.AddListener(SpawnSoldier);
        _gameManager.uIManager.spawnEnemyArcher.onClick.AddListener(SpawnEnemyArcher);
        _gameManager.uIManager.spawnSoldierArcher.onClick.AddListener(SpawnSoldierArcher);
        _gameManager.uIManager.spawnSoldierDigger.onClick.AddListener(SpawnSoldierDigger);
        _gameManager.uIManager.spawnEnemyDigger.onClick.AddListener(SpawnEnemyDigger);
    }

    private void SpawnSoldier()
    {
      var obj= Instantiate(soldier, spawnPointSoldier.position,Quaternion.identity,spawnPointSoldier);
      _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
    }

    private void SpawnEnemy()
    {
        var obj= Instantiate(enemy, spawnPointEnemy.position,Quaternion.identity,spawnPointEnemy);
        _gameManager.enemies.Add(obj.GetComponent<AgentBase>());
    }

    private void SpawnEnemyArcher()
    {
        var obj= Instantiate(enemyArcher, spawnPointEnemy.position,Quaternion.identity,spawnPointEnemy);
        _gameManager.enemies.Add(obj.GetComponent<AgentBase>());
    }
    private void SpawnSoldierArcher()
    {
        var obj= Instantiate(soldierArcher, spawnPointSoldier.position,Quaternion.identity,spawnPointSoldier);
        _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
    }
    private void SpawnEnemyDigger()
    {
        var obj= Instantiate(enemyDigger, spawnPointEnemy.position,Quaternion.identity,spawnPointEnemy);
        _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
    }
    private void SpawnSoldierDigger()
    {
        var obj= Instantiate(soldierDigger, spawnPointSoldier.position,Quaternion.identity,spawnPointSoldier);
        _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
    }
    
    private void OnEnable()
    {
        UIManager.OnClickedEnemySpawnButton += SpawnEnemy;
        UIManager.OnClickedSoldierSpawnButton += SpawnSoldier;
        UIManager.OnClickedEnemyArcherSpawnButton += SpawnEnemyArcher;
        UIManager.OnClickedSoldierArcherSpawnButton += SpawnSoldierArcher;
        UIManager.OnClickedSoldierDiggerSpawnButton += SpawnSoldierDigger;
        UIManager.OnClickedEnemyDiggerSpawnButton += SpawnEnemyDigger;
    }

    private void OnDisable()
    {
        UIManager.OnClickedEnemySpawnButton -= SpawnEnemy;
        UIManager.OnClickedSoldierSpawnButton -= SpawnSoldier;
        UIManager.OnClickedEnemyArcherSpawnButton -= SpawnEnemyArcher;
        UIManager.OnClickedSoldierArcherSpawnButton -= SpawnSoldierArcher;
        UIManager.OnClickedSoldierDiggerSpawnButton -= SpawnSoldierDigger;
        UIManager.OnClickedEnemyDiggerSpawnButton -= SpawnEnemyDigger;
    }
}
