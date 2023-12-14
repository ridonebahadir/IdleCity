using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public enum AgentType
{
    Enemy,
    Soldier,
}
public enum AgentState
{
    Walking,
    Waiting,
    Fighting
}
public abstract class AgentBase : MonoBehaviour
{
    public SOAgent soAgent;
    [SerializeField] protected AgentState agentState;
    [SerializeField] public Animator animator;
    private AgentType _agentType; 
    private float _diggSpeed;
    [SerializeField] protected float _attackDistance;
    [SerializeField]  private float _health;
    [SerializeField]  private float _speed;
    [SerializeField]  protected float _damage;
    private float cost;
    protected Collider _collider;
    protected NavMeshAgent navMeshAgent;

    private Domination.Domination _domination;
    private GameManager _gameManager;
    public Transform _target;
    
    protected AgentBase _targetAgentBase;
    internal bool isInside;
    protected WaitForSeconds _wait = new(0.05f);
    public float _dist;
    
    protected bool isDeath;
    private float _navMeshStopDistance;
    public float DiggSpeed => _diggSpeed;
    protected IEnumerator Move;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshStopDistance = navMeshAgent.stoppingDistance;
        _gameManager=GameManager.Instance;
        
        
        _domination = _gameManager.dominationArea;
        _collider = GetComponent<Collider>();
        SetPercentSpeed(100);
        InıtAgent();
       
        if (agentState == AgentState.Walking)
        {
           
            _target = _gameManager.dominationArea.transform;
        }
        else
        {
            _target = _domination.SlotTarget(_agentType);
            //_attackDistance = 0;
        }
        navMeshAgent.SetDestination(_target.position);
        Move = MoveTarget();
        StartCoroutine(Move);
       
    }
    private void InıtAgent()
    {
        _agentType = soAgent.agentType;
        _speed = soAgent.speed;
        _health = soAgent.health;
        _damage = soAgent.damage;
        _diggSpeed = soAgent.diggSpeed;
        _attackDistance = soAgent.attackDistance;

    }
    protected virtual IEnumerator MoveTarget()
    {
        while (true)
        {
            navMeshAgent.SetDestination(_target.position);
            _dist = Vector3.Distance(transform.position, _target.position);
            if (_dist < _attackDistance)
            {
                if (agentState == AgentState.Fighting)
                { 
                   AttackType();
                   //StopCoroutine(Move);
                }
                if (agentState==AgentState.Waiting)
                {
                    _attackDistance = 0;
                    navMeshAgent.stoppingDistance = 0;
                    Debug.Log("Wait");
                }

                if (agentState==AgentState.Walking)
                {
                    _target = _domination.SlotTarget(_agentType);
                    agentState = AgentState.Waiting;
                    //_attackDistance = 0;
                    //animator.SetBool("Digg",true);
                }
               
                
            }
            else
            {
               
                //navMeshAgent.destination = _target.position;
            }
            yield return _wait;
        }
    }
   
    protected abstract void AttackType();
    public void Attack(Transform target)
    {
        //animator.SetBool("Digg",false);
        //isWar = true;
        //_attackDistance = soAgent.attackDistance;
       
        //_target = target;
       
        //_agentBase = target.transform.GetComponent<AgentBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AgentBase otherAgentBase))
        {
            _attackDistance = soAgent.attackDistance;
            navMeshAgent.stoppingDistance =_navMeshStopDistance;
            _collider.enabled = false;
            _targetAgentBase = otherAgentBase;
            agentState = AgentState.Fighting;
            _target = otherAgentBase.transform;
            //Attack(otherAgentBase.transform);
        }
    }

    protected void DetectTarget()
    {
        switch (_agentType)
        {
            case AgentType.Enemy:
            {
                if (_domination.soldiers.Count>0 && isInside)
                {
                    Attack(_domination.CloseAgentSoldier(transform));
                }
                else 
                {
                    _target = GameManager.Instance.dominationArea.transform;
                    //isWar = false;
                    agentState = AgentState.Walking;
                }
                break;
            }
            case AgentType.Soldier:
            {
                if (_domination.enemies.Count>0 && isInside)
                {
                    Attack(_domination.CloseAgentEnemy(transform));
                }
                else 
                {
                    _target = GameManager.Instance.dominationArea.transform;
                     //isWar = false;
                     agentState = AgentState.Walking;
                }

                break;
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        //particleSystem.Play();
        _health -= damage;
        if (_health>0)
        {
            
        }
        else
        {
            _collider.enabled = false;
            StopCoroutine(Move);
            Death();
           
           
        }
       
    }

    
    private void Death()
    {
        if (isDeath) return;
       
        StartCoroutine(DeathIE());
        IEnumerator DeathIE()
        {
            //isDeath = true;
            animator.SetTrigger("Death");
            if (_agentType==AgentType.Enemy)  GetReward();
            //RemoveList();
            yield return new WaitForSeconds(2.25f);
            navMeshAgent.enabled = false;
            gameObject.SetActive(false);
        }


    }

    void RemoveList()
    {
        _gameManager.RemoveList(this,_agentType);
        _domination.RemoveList(this,_agentType);
    }

    void GetReward()
    {
        _gameManager.GetReward(soAgent.reward);
    }

    public void SetPercentHealth(float value)
    {
        var a = (soAgent.health * value) / 100; 
        _health += a;
        if (_health>=soAgent.health)  _health = soAgent.health;
    }

    public void SetPercentSpeed(float value)
    {
        var a = (soAgent.speed * value) / 100;
        navMeshAgent.speed = _speed + a;
    }


    public void SetPercentAttack(float value)
    {
        var a = (soAgent.damage * value) / 100;
        _damage += a;
    }

    public void SetPercentTakeDamage(float value)
    {
        var a=(soAgent.health * value) / 100;
        TakeDamage(a);
    }

    public void SetBattleLineState()
    {
        agentState = AgentState.Waiting;
    }

    public float GetHealth => _health;
    
}
