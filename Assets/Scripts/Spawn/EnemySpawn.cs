using System;
using System.Collections;
using System.Collections.Generic;
using Agent;
using DG.Tweening;
using Domination;
using LeonBrave;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemySpawn : MonoBehaviour
{
    private GameManager _gameManager;
    private Domination.Domination _domination;

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

    private SingletonHandler _singletonHandler;
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
        StartCoroutine(SpawnOrder());
        _singletonHandler = SingletonHandler.Instance;
    }

    private IEnumerator SpawnOrder()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(SpawnEnemyRoutine(1,spawnEnemyTime,enemyImage));
        //yield return new WaitForSeconds(spawnEnemyTime);
        StartCoroutine(SpawnEnemyRoutine(2,spawnEnemyArcherTime,enemyArcherImage));
        //yield return new WaitForSeconds(spawnEnemyArcherTime);
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
                        for (int i = 0; i < 3; i++)
                        {
                            SpawnEnemy();
                        }
                        
                        break;
                    case 2:
                        for (int i = 0; i < 3; i++)
                        {
                            SpawnEnemyArcher();
                        }
                        
                        break;
                    case 3:
                        for (int i = 0; i < 3; i++)
                        {
                            SpawnEnemyDigger();
                        }
                       
                        break;
                }
                image.fillAmount = 1;
            });
            
            yield return waitForSeconds;
        }
    }
    private void SpawnEnemy()
    {
        Spawn(ObjectType.Enemy);
        // var obj= Instantiate(enemy, spawnPointEnemy.position,Quaternion.identity,spawnPointEnemy);
        // var rand = Random.Range(10, -10);
        // obj.transform.localPosition = new Vector3(rand, 0, 0);
        // obj.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
        // AgentBase agentBase = obj.GetComponent<AgentBase>();
        // _gameManager.enemies.Add(agentBase);
        //if (_domination.dominationMoveDirect == DominationMoveDirect.EnemyMove) agentBase.SetBattleLineState();
    }

    private void SpawnEnemyArcher()
    {
        Spawn(ObjectType.EnemyArcher);
        // var obj= Instantiate(enemyArcher, spawnPointEnemy.position,Quaternion.identity,spawnPointEnemy);
        // obj.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
        // AgentBase agentBase = obj.GetComponent<AgentBase>();
        // _gameManager.enemies.Add(agentBase);
        //if (_domination.dominationMoveDirect == DominationMoveDirect.EnemyMove) agentBase.SetBattleLineState();
    }
    private void SpawnEnemyDigger()
    {
        Spawn(ObjectType.EnemyDigger);
        // var obj= Instantiate(enemyDigger, spawnPointEnemy.position,Quaternion.identity,spawnPointEnemy);
        // obj.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
        // AgentBase agentBase = obj.GetComponent<AgentBase>();
        // _gameManager.enemies.Add(agentBase);
       // if (_domination.dominationMoveDirect == DominationMoveDirect.EnemyMove) agentBase.SetBattleLineState();
    }

    private void Spawn(ObjectType objectType)
    {
        var cloneObj = _singletonHandler.GetSingleton<ObjectPool>().TakeObject(objectType);
        //var cloneObj= Instantiate(obj, pos.position,Quaternion.identity,pos);
        var rand = Random.Range(3, -3);
        cloneObj.transform.localPosition = new Vector3(rand, 0, 0);
        cloneObj.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
        cloneObj.SetActive(true);
        AgentBase agentBase = cloneObj.GetComponent<AgentBase>();
        agentBase.InıtAgent();
        _gameManager.enemies.Add(agentBase);
    }
}
