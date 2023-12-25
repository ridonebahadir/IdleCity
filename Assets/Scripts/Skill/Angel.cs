using System;
using System.Collections;
using System.Collections.Generic;
using Agent;
using UnityEngine;
using UnityEngine.AI;

public class Angel : MonoBehaviour
{
    public static Angel Create(GameObject angelObj,Transform angelSpawnPoint,SmallTrigger target,float lifeTime)
    {
        var cloneAngel=Instantiate(angelObj,angelSpawnPoint.position,Quaternion.identity,angelSpawnPoint).transform;
        var angel = cloneAngel.GetComponent<Angel>();
        angel.Setup(target,lifeTime);
        return angel;
    }

    private float _lifeTime;
    private GameManager _gameManager;
    private SmallTrigger _target;
    private NavMeshAgent _navMeshAgent;
    private List<SmallTrigger> _closeAgentList = new List<SmallTrigger>();
    private readonly WaitForSeconds _wait = new(1);
    private IEnumerator _takeHealIE;
    
    void Start()
    {
        _gameManager = GameManager.Instance;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _takeHealIE = TakeHeal();
        StartCoroutine(_takeHealIE);
    }
    
    void Update()
    {
        if (_target==null)return;
        if (_target.agentBase.GetHealth <= 0) if (_gameManager.soldiers.Count>0) _target=_gameManager.GetFurthestAllie();
        _navMeshAgent.SetDestination(_target.transform.position);
    }

    private void Setup(SmallTrigger target,float lifeTime)
    {
        _target = target;
        _lifeTime = lifeTime;
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
    
    private IEnumerator TakeHeal()
    {
        while (true)
        {
            if (_closeAgentList.Count > 0)
            {
                foreach (var item in _closeAgentList) item.agentBase.TakeHeal(3);
               
            }
            _lifeTime--;
            if (_lifeTime <= 0) Death();
            yield return _wait;
        }
      
        // ReSharper disable once IteratorNeverReturns
    }

    private void Death()
    {
        foreach (var item in _closeAgentList)  item.agentBase.TakeHealOver();
        StopCoroutine(_takeHealIE);
        gameObject.SetActive(false);
    }
    
}
