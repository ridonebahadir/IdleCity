using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    protected AgentType _agentType; 
    private float _diggSpeed;
    [SerializeField] protected float _attackDistance;
    [SerializeField]  private float _health;
    [SerializeField]  private float _speed;
    [SerializeField]  protected float _damage;
    private float cost;
    protected Collider _collider;
    protected NavMeshAgent navMeshAgent;

    [SerializeField] private ParticleSystem particleSystem;
    
    protected Domination.Domination _domination;
    private GameManager _gameManager;
    public Transform _target;
    
    protected AgentBase _targetAgentBase;
    internal bool isInside;
    protected WaitForSeconds _wait = new(0.05f);
    public float _dist;
    
    protected bool isDeath;
    private float _navMeshStopDistance;
    [SerializeField] private List<AgentBase> closeList;
    
    public float DiggSpeed => _diggSpeed;
    protected IEnumerator Move;
    [SerializeField] private int slotTurn;
    
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
    protected IEnumerator MoveTarget()
    {
        while (true)
        {
            navMeshAgent.SetDestination(_target.position);
            _dist = Vector3.Distance(transform.position, _target.position);
            if (agentState == AgentState.Fighting)
            {
                if (_targetAgentBase.GetHealth<=0)
                {
                    TargetDeath();
                }
            }
            if (_dist < _attackDistance)
            {
                if (agentState == AgentState.Fighting)
                { 
                   Attack();
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
                    SlotTarget();
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
    protected abstract void SlotTarget();
    protected abstract void  SlotTargetRemove();
    
    private IEnumerator _attack;
    public void Attack()
    {
        if (_attack==null)
        {
            _attack = AttackCoroutine();
            StartCoroutine(_attack);
        }
    }
    IEnumerator AttackCoroutine()
    {
        WaitForSeconds wait = new(1);
        animator.SetBool("Attack",true);
        while (true)
        {
            
            
            if (_targetAgentBase.GetHealth<=0)
            {
                TargetDeath();
                yield break;
                       
            }
            else
            {
                if (GetHealth>0)
                {
                           
                    yield return wait;
                    AttackType();
                    yield return new WaitForSeconds(0.6f);
                    //_targetAgentBase.TakeDamage(_damage);
                }
                else
                {
                    SlotTarget();
                    animator.SetBool("Attack",false);
                    yield break;
                }
                        
            }
            
            
           
            yield return null; 
        }
                

    }

    private void TargetDeath()
    {
        animator.SetBool("Attack",false);
        agentState = AgentState.Walking;
        if (_attack!=null) StopCoroutine(_attack);
        _targetAgentBase = null;
        _attack = null;
        AliveAgentCheck();
        if(closeList.Count>0)StartAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        
            if (other.TryGetComponent(out AgentBase otherAgentBase))
            {
                closeList.Add(otherAgentBase);
                if (closeList.Count==1)
                {
                    StartAttack();
                }
               
                //Attack(otherAgentBase.transform);
            }
        
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out AgentBase otherAgentBase))
        {
            closeList.Remove(otherAgentBase);
        }
    }

    private void StartAttack()
    {
        SlotTargetRemove();
        _attackDistance = soAgent.attackDistance;
        navMeshAgent.stoppingDistance =_navMeshStopDistance;
        _targetAgentBase = CloseAgent();
        _target = _targetAgentBase.transform;
        agentState = AgentState.Fighting;
        //_targetAgentBase = _target.transform.GetComponent<AgentBase>();
    }
    public void TakeDamage(float damage)
    {
        particleSystem.Play();
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

    private void AliveAgentCheck()
    {
        for (int i = 0; i < closeList.Count; i++)
        {
            if (closeList[i].GetHealth<=0)
            {
                Debug.Log("DELETE");
                closeList.Remove(closeList[i]);
            }
            
        }
    }
    private AgentBase CloseAgent()
    {
       
        return closeList.OrderBy(go => (transform.position - go.transform.position).sqrMagnitude).First();
    }
    public float GetHealth => _health;
    
}
