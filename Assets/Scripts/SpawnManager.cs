using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject soldier;

    [SerializeField] private Transform spawnPointEnemy;
    [SerializeField] private Transform spawnPointSoldier;
    
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.uIManager.spawnEnemy.onClick.AddListener(SpawnEnemy);
        _gameManager.uIManager.spawnSoldier.onClick.AddListener(SpawnSoldier);
    }

    private void SpawnSoldier()
    {
       Instantiate(soldier, spawnPointSoldier.position,Quaternion.identity,spawnPointSoldier);
    }

    private void SpawnEnemy()
    {
        Instantiate(enemy, spawnPointEnemy.position,Quaternion.identity,spawnPointEnemy);
    }
    
    private void OnEnable()
    {
        UIManager.OnClickedEnemySpawnButton += SpawnEnemy;
        UIManager.OnClickedSoldierSpawnButton += SpawnSoldier;
    }

    private void OnDisable()
    {
        UIManager.OnClickedEnemySpawnButton -= SpawnEnemy;
        UIManager.OnClickedSoldierSpawnButton -= SpawnSoldier;
    }
}
