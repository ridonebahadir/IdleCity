using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AgentType
{
    Enemy,
    Soldier,
}
public abstract class AgentBase : MonoBehaviour
{
    public AgentType agentType;
    [SerializeField] private int health;
    public float attackDistance;
    
    public NavMeshAgent navMeshAgent;

    private Domination.Domination _domination;
    private GameManager _gameManager;
    public Transform _target;
    public AgentBase _agentBase;

    public bool isInside;
    protected WaitForSeconds _wait = new(0.5f);
    public float _dist;
    private void Start()
    {
        _gameManager=GameManager.Instance;
        _target = _gameManager.dominationArea.transform;
        _domination = _gameManager.dominationArea;
        StartCoroutine(MoveDominationArea());
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
    protected void DetectTarget()
    {
        _domination.DetectTarget(this); 
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health>0)
        {
            
        }
        else
        {
            Death();
           
        }
       
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
