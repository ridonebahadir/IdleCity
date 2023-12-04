using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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

    [Space(10)]
    [Header("SPAWN TIME")]
    [SerializeField] private Image enemyImage;
    [SerializeField] private Image enemyArcherImage;
    [SerializeField] private Image enemyDiggerImage;

    
    private void Start()
    {
        _gameManager = GameManager.Instance;
        StartCoroutine(SpawnOrder());
    }

    private IEnumerator SpawnOrder()
    {
        yield return new WaitForSeconds(10);
        StartCoroutine(SpawnEnemyRoutine(1,spawnEnemyTime,enemyImage));
        yield return new WaitForSeconds(spawnEnemyTime);
        StartCoroutine(SpawnEnemyRoutine(2,spawnEnemyArcherTime,enemyArcherImage));
        yield return new WaitForSeconds(spawnEnemyArcherTime);
        StartCoroutine(SpawnEnemyRoutine(3,spawnEnemyDiggerTime,enemyDiggerImage));
    }
    private IEnumerator SpawnEnemyRoutine(int turn,int time,Image image)
    {
        WaitForSeconds waitForSeconds = new(time);
        while (true)
        {
            image.DOFillAmount(0, time).OnComplete(()=>
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
                image.fillAmount = 1;
            });
            
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
        _gameManager.enemies.Add(obj.GetComponent<AgentBase>());
    }
}
