using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject soldier;
    [SerializeField] private GameObject enemyArcher;

    [SerializeField] private Transform spawnPointEnemy;
    [SerializeField] private Transform spawnPointSoldier;
    
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.uIManager.spawnEnemy.onClick.AddListener(SpawnEnemy);
        _gameManager.uIManager.spawnSoldier.onClick.AddListener(SpawnSoldier);
        _gameManager.uIManager.spawnEnemyArcher.onClick.AddListener(SpawnEnemyArcher);
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
    
    private void OnEnable()
    {
        UIManager.OnClickedEnemySpawnButton += SpawnEnemy;
        UIManager.OnClickedSoldierSpawnButton += SpawnSoldier;
        UIManager.OnClickedEnemyArcherSpawnButton += SpawnEnemyArcher;
    }

    private void OnDisable()
    {
        UIManager.OnClickedEnemySpawnButton -= SpawnEnemy;
        UIManager.OnClickedSoldierSpawnButton -= SpawnSoldier;
        UIManager.OnClickedEnemyArcherSpawnButton -= SpawnEnemyArcher;
    }
}
