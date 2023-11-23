using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AgentBase : MonoBehaviour
{
    [SerializeField] private int health;
    
    [SerializeField] private NavMeshAgent navMeshAgent;

    protected Domination _domination;
    //private GameManager _gameManager;
    public Transform _target;
    public AgentBase _agentBase;
    
    
    private WaitForSeconds _wait = new(1);
    [SerializeField] private float _dist;
    private void Start()
    {
        _target = GameManager.Instance.dominationArea.transform;
        _domination = GameManager.Instance.dominationArea;
        StartCoroutine(MoveDominationArea());
        
        //_gameManager = GameManager.Instance;

    }

    IEnumerator MoveDominationArea()
    {
        while (true)
        {
            
            navMeshAgent.destination = _target.position;
            _dist = Vector3.Distance(transform.position, _target.position);
            if (_dist<1.5f)
            {
                if (_agentBase!=null)
                {
                    if (_agentBase.TakeDamage(10))
                    {
                        DetectTarget();
                    }
                }
                else
                {
                    
                }
                
            }
            yield return _wait;
        }
    }

    protected abstract void DetectTarget();
    protected abstract void RemoveList();
    


    public void Attack(Transform target)
    {
        _target = target;
        _agentBase = target.transform.GetComponent<AgentBase>();

    }

    bool TakeDamage(int damage)
    {
        health -= damage;
        if (health>0)
        {
            
        }
        else
        {
            Death();
            return true;
        }
        return false;
    }

    private void Death()
    {
        RemoveList();
       gameObject.SetActive(false);
    }
}
