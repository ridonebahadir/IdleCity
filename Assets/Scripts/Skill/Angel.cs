using System;
using System.Collections;
using System.Collections.Generic;
using Agent;
using UnityEngine;
using UnityEngine.AI;

public class Angel : MonoBehaviour
{
    public static Angel Create(GameObject angelObj,Transform angelSpawnPoint,AgentBase target,float lifeTime)
    {
        var cloneAngel=Instantiate(angelObj,angelSpawnPoint.position,Quaternion.identity,angelSpawnPoint).transform;
        var angel = cloneAngel.GetComponent<Angel>();
        angel.Setup(target,lifeTime);
        return angel;
    }

    private float _lifeTime;
    private GameManager _gameManager;
    private AgentBase _target;
    private NavMeshAgent _navMeshAgent;
    private List<AgentBase> _closeAgentList = new List<AgentBase>();
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
        if (_target.GetHealth <= 0) if (_gameManager.soldiers.Count>0) _target=_gameManager.GetFurthestAllie();
        _navMeshAgent.SetDestination(_target.transform.position);
    }

    private void Setup(AgentBase target,float lifeTime)
    {
        _target = target;
        _lifeTime = lifeTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out AgentBase agentBase)) return;
        if (!_closeAgentList.Contains(agentBase)) _closeAgentList.Add(agentBase);
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out AgentBase agentBase)) return;
        if (!_closeAgentList.Contains(agentBase))
        {
            _closeAgentList.Remove(agentBase);
            agentBase.TakeHealOver();
        }
       
    }
    
    private IEnumerator TakeHeal()
    {
        while (true)
        {
            if (_closeAgentList.Count > 0)
            {
                foreach (var item in _closeAgentList) item.TakeHeal(3);
               
            }
            _lifeTime--;
            if (_lifeTime <= 0) Death();
            yield return _wait;
        }
      
        // ReSharper disable once IteratorNeverReturns
    }

    private void Death()
    {
        foreach (var item in _closeAgentList)  item.TakeHealOver();
        StopCoroutine(_takeHealIE);
        gameObject.SetActive(false);
    }
    
}
