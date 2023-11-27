using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject enemyDigger;
    [SerializeField] private GameObject enemyArcher;
    [SerializeField] private Transform spawnPointEnemy;
    
    [Space(10)]
    [Header("SPAWN TIME")]
    [SerializeField] private int spawnEnemyTime;
    [SerializeField] private int spawnEnemyArcherTime;
    [SerializeField] private int spawnEnemyDiggerTime;

    
    private void Start()
    {
        _gameManager = GameManager.Instance;
        StartCoroutine(SpawnOrder());
    }

    private IEnumerator SpawnOrder()
    {
        yield return new WaitForSeconds(10);
        StartCoroutine(SpawnEnemyRoutine(1,spawnEnemyTime));
        yield return new WaitForSeconds(spawnEnemyTime);
        StartCoroutine(SpawnEnemyRoutine(2,spawnEnemyArcherTime));
        yield return new WaitForSeconds(spawnEnemyArcherTime);
        StartCoroutine(SpawnEnemyRoutine(3,spawnEnemyDiggerTime));
    }
    private IEnumerator SpawnEnemyRoutine(int turn,int time)
    {
        WaitForSeconds waitForSeconds = new(time);
        while (true)
        {
            switch (turn)
            {
                case 1 :
                    SpawnEnemy();
                    break;
                case 2:
                    SpawnEnemyArcher();
                    break;
                case 3:
                    SpawnEnemyDigger();
                    break;
            }
            yield return waitForSeconds;
        }
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
    private void SpawnEnemyDigger()
    {
        var obj= Instantiate(enemyDigger, spawnPointEnemy.position,Quaternion.identity,spawnPointEnemy);
        _gameManager.soldiers.Add(obj.GetComponent<AgentBase>());
    }
}
