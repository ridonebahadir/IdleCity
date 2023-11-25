using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public enum AgentType
{
    Enemy,
    Soldier,
}
public abstract class AgentBase : MonoBehaviour
{
    public SOAgent soAgent;
    
    private AgentType _agentType;
    private float _diggSpeed;
    protected float _attackDistance;
    private int _health;
    private int _damage;
    private float cost;
    
    protected NavMeshAgent navMeshAgent;

    private Domination.Domination _domination;
    private GameManager _gameManager;
    public Transform _target;
    
    protected AgentBase _agentBase;
    internal bool isInside;
    protected WaitForSeconds _wait = new(0.5f);
    public float _dist;
    public float DiggSpeed => _diggSpeed;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        _gameManager=GameManager.Instance;
        _target = _gameManager.dominationArea.transform;
        _domination = _gameManager.dominationArea;
        InıtAgent();
        StartCoroutine(MoveDominationArea());
       
    }

    private void InıtAgent()
    {
        _agentType = soAgent.agentType;
        navMeshAgent.speed = soAgent.speed;
        _health = soAgent.health;
        _damage = soAgent.damage;
        _diggSpeed = soAgent.diggSpeed;
        _attackDistance = soAgent.attackDistance;

    }
    protected virtual IEnumerator MoveDominationArea()
    {
        while (true)
        {
            _dist = Vector3.Distance(transform.position, _target.position);
           
            
            if (_dist<_attackDistance)
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
        switch (_agentType)
        {
            case AgentType.Enemy:
            {
                if (_domination._soldiers.Count>0 && isInside)
                {
                    Attack(_domination.CloseAgentSoldier(transform));
                }
                else 
                {
                    _target = GameManager.Instance.dominationArea.transform;
                }
                break;
            }
            case AgentType.Soldier:
            {
                if (_domination._enemies.Count>0 && isInside)
                {
                    Attack(_domination.CloseAgentEnemy(transform));
                }
                else 
                {
                    _target = GameManager.Instance.dominationArea.transform;
                }

                break;
            }
        }
    }
    public void TakeDamage()
    {
        _health -= _damage;
        if (_health>0)
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
        _gameManager.RemoveList(this,_agentType);
        _domination.RemoveList(this,_agentType);
    }

    
 
   
}
