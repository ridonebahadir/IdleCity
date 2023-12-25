using System;
using System.Collections;
using System.Collections.Generic;
using Agent;
using UnityEngine;
using UnityEngine.AI;

public class Devil : MonoBehaviour
{
    public static Devil Create(GameObject angelObj,Transform angelSpawnPoint,SmallTrigger target,float lifeTime)
    {
        var cloneAngel=Instantiate(angelObj,angelSpawnPoint.position,Quaternion.identity,angelSpawnPoint).transform;
        var devil = cloneAngel.GetComponent<Devil>();
        devil.Setup(target,lifeTime);
        return devil;
    }
    private float _lifeTime;
    private GameManager _gameManager;
    private SmallTrigger _target;
    private NavMeshAgent _navMeshAgent;
    private List<SmallTrigger> _closeAgentList = new List<SmallTrigger>();
    private readonly WaitForSeconds _wait = new(1);
    private IEnumerator _takeDamage;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _takeDamage = GiveDamage();
        StartCoroutine(_takeDamage);
    }
    void Update()
    {
        if (_target==null)return;
        if (_target.agentBase.GetHealth <= 0) if (_gameManager.enemies.Count>0) _target=_gameManager.GetFurhestEnemies();
        _navMeshAgent.SetDestination(_target.transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out SmallTrigger smallTrigger)) return;
        if (!_closeAgentList.Contains(smallTrigger)) _closeAgentList.Add(smallTrigger);
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out SmallTrigger smallTrigger)) return;
        if (!_closeAgentList.Contains(smallTrigger))
        {
            _closeAgentList.Remove(smallTrigger);
            smallTrigger.agentBase.TakeHealOver();
        }
       
    }

    private void Setup(SmallTrigger target, float lifeTime)
    {
        _target = target;
        _lifeTime = lifeTime;
    }
    
    private IEnumerator GiveDamage()
    {
        while (true)
        {
            if (_closeAgentList.Count > 0)
            {
                foreach (var item in _closeAgentList)
                {
                    if (item.agentBase.GetHealth>0)  item.agentBase.SetPercentTakeDamage(50);
                }
               
            }
            _lifeTime--;
            if (_lifeTime <= 0) Death();
            yield return _wait;
        }
      
        // ReSharper disable once IteratorNeverReturns
    }
    private void Death()
    {
        //foreach (var item in _closeAgentList)  item.TakeHealOver();
        StopCoroutine(GiveDamage());
        gameObject.SetActive(false);
    }
}
