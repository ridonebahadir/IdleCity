using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AgentType
{
    Enemy,
    Soldier,
    EnemyArcher
}
public abstract class AgentBase : MonoBehaviour
{
    public AgentType agentType;
    [SerializeField] private int health;
    public float attackDistance;
    
    public NavMeshAgent navMeshAgent;

    protected Domination _domination;
    protected GameManager _gameManager;
    public Transform _target;
    public AgentBase _agentBase;
    
    
    protected WaitForSeconds _wait = new(0.5f);
    public float _dist;
    private void Start()
    {
        _gameManager=GameManager.Instance;
        _target = _gameManager.dominationArea.transform;
        _domination = _gameManager.dominationArea;
        StartCoroutine(MoveDominationArea());
        
        //_gameManager = GameManager.Instance;

    }

    protected virtual IEnumerator MoveDominationArea()
    {
        while (true)
        {
            _dist = Vector3.Distance(transform.position, _target.position);
           
            
            if (_dist<attackDistance)
            {
                AttackType();

            }
            else
            {
                navMeshAgent.destination = _target.position;
            }
            yield return _wait;
        }
    }

    protected abstract void AttackType();
    
    


    public void Attack(Transform target)
    {
        _target = target;
        _agentBase = target.transform.GetComponent<AgentBase>();
    }

    public bool TakeDamage(int damage)
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

    void RemoveList()
    {
        _gameManager.RemoveList(this,agentType);
        _domination.RemoveList(this,agentType);
    }
    
    
}
