using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domination;
using UnityEngine;
using UnityEngine.AI;


namespace Agent
{
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
        public Transform target;
        public float dist;
        
        [SerializeField] protected AgentState agentState;
        [SerializeField] private Animator animator;
        [SerializeField] protected float attackDistance;
        [SerializeField] private float health;
        [SerializeField] private float speed;
        [SerializeField] protected float damage;
        [SerializeField] private new ParticleSystem particleSystem;
        [SerializeField] private List<AgentBase> closeList;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private float firstAnimWait;
        [SerializeField] private float secondAnimWait;
        
        protected AgentType AgentType; 
        protected NavMeshAgent NavMeshAgent;
        protected Domination.Domination Domination;
        protected AgentBase TargetAgentBase;
        protected bool IsDeath;
        
        private float _diggSpeed;
        private float _cost;
        private GameManager _gameManager; 
        private readonly WaitForSeconds _wait = new(0.05f); 
        private IEnumerator _move;
        private float _navMeshStopDistance;
        private float _startRotateSpeed;
        private WaitForSeconds _firstAnimWaitForSeconds;
        private WaitForSeconds _secondAnimWaitForSeconds;
        private IEnumerator _attack;
        private static readonly int Death1 = Animator.StringToHash("Death");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        private static readonly int Wait = Animator.StringToHash("Wait");
        
        public float DiggSpeed => _diggSpeed;

        private void Awake()
        {
            _gameManager=GameManager.Instance;
            Domination = _gameManager.dominationArea;
        }

        private void Start()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            NavMeshAgent.stoppingDistance = soAgent.attackDistance;
            
            SetPercentSpeed(100);
            InıtAgent();
            SetStartTarget();
           
            
            _move = MoveTarget();
            StartCoroutine(_move);
           
            _firstAnimWaitForSeconds = new WaitForSeconds(firstAnimWait);
            _secondAnimWaitForSeconds = new WaitForSeconds(secondAnimWait);

        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out AgentBase otherAgentBase)) return;
            closeList.Add(otherAgentBase);
            if (closeList.Count != 1) return;
            animator.SetBool(Wait,false);
            NavMeshAgent.angularSpeed = _startRotateSpeed;
            StartAttack();
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out AgentBase otherAgentBase))
            {
                closeList.Remove(otherAgentBase);
            }
        }
        
        protected abstract void AttackType();
        protected abstract void SlotTarget();
        protected abstract void  SlotTargetRemove();
        protected abstract void  Flee();
        
        
        
        private void InıtAgent()
        {
            AgentType = soAgent.agentType;
            speed = soAgent.speed;
            health = soAgent.health;
            damage = soAgent.damage;
            _diggSpeed = soAgent.diggSpeed;
            attackDistance = soAgent.attackDistance;
            _navMeshStopDistance = soAgent.attackDistance;
            _startRotateSpeed = NavMeshAgent.angularSpeed;
        }
        private IEnumerator MoveTarget()
        {
            while (true)
            {
                var pos = target.position;
                NavMeshAgent.SetDestination(pos);
                dist = Vector3.Distance(transform.position, pos);
                if (agentState == AgentState.Fighting)
                {
                    if (TargetAgentBase.GetHealth<=0)
                    {
                        TargetDeath();
                    }
                }
                if (dist < attackDistance)
                {
                    if (agentState == AgentState.Fighting)
                    { 
                        //Flee();
                        Attack();
                    }
                    if (agentState==AgentState.Waiting)
                    {
                        NavMeshAgent.angularSpeed = 0;
                        animator.SetBool(Wait,true);
                    }

                    if (agentState==AgentState.Walking)
                    {
                        SlotTarget();
                        agentState = AgentState.Waiting;
                    }
                    
                }
                yield return _wait;
            }
            // ReSharper disable once IteratorNeverReturns
        }
        private void Attack()
        {
            animator.SetBool(Wait,false);
            if (_attack==null)
            {
                _attack = AttackCoroutine();
                StartCoroutine(_attack);
            }
        }
        IEnumerator AttackCoroutine()
        {
            animator.SetBool(Attack1,true);
            while (true)
            {
                if (TargetAgentBase.GetHealth<=0)
                {
                    TargetDeath();
                    yield break;
                }
                else
                {
                    if (GetHealth>0)
                    {
                        yield return _firstAnimWaitForSeconds;
                        AttackType();
                        yield return _secondAnimWaitForSeconds;
                    }
                    else
                    {
                        SlotTarget();
                        animator.SetBool(Attack1,false);
                        yield break;
                    }
                }
                yield return null; 
            }
                

        }
        private void TargetDeath()
        {
            animator.SetBool(Attack1,false);
            agentState = AgentState.Walking;
            if (_attack!=null) StopCoroutine(_attack);
            TargetAgentBase = null;
            _attack = null;
            AliveAgentCheck();
            if(closeList.Count>0)StartAttack();
            else SetStartTarget();
        }
        
        private void StartAttack()
        {
            SlotTargetRemove();
            attackDistance = soAgent.attackDistance;
            NavMeshAgent.stoppingDistance =_navMeshStopDistance;
            TargetAgentBase = CloseAgent();
            target = TargetAgentBase.transform;
            agentState = AgentState.Fighting;
            //_targetAgentBase = _target.transform.GetComponent<AgentBase>();
        }
        public void TakeDamage(float dmg)
        {

            healthBar.HealthBarCanvasGroupShow();
            particleSystem.Play();
            health -= dmg;
            healthBar.SetHealthBar(soAgent.health,health);
            if (health>0)
            {
            
            }
            else
            {
                healthBar.StopHealthBarShow();
                StopCoroutine(_move);
                Death();
           
           
            }
       
        }
        private void Death()
        {
            if (IsDeath) return;
       
            StartCoroutine(DeathIE());
            return;

            IEnumerator DeathIE()
            {
                IsDeath = true;
                animator.SetTrigger(Death1);
                if (AgentType==AgentType.Enemy)  GetReward(); 
                _gameManager.RemoveList(this,AgentType);
                yield return new WaitForSeconds(2.25f);
                NavMeshAgent.enabled = false;
                gameObject.SetActive(false);
            }


        }
        void GetReward()
        {
            _gameManager.GetReward(soAgent.reward);
        }
        public void SetPercentHealth(float value)
        {
            var a = (soAgent.health * value) / 100; 
            health += a;
            if (health>=soAgent.health)  health = soAgent.health;
        }
        public void SetPercentSpeed(float value)
        {
            var a = (soAgent.speed * value) / 100;
            NavMeshAgent.speed = speed + a;
        }
        public void SetPercentAttack(float value)
        {
            var a = (soAgent.damage * value) / 100;
            damage += a;
        }
        public void SetPercentTakeDamage(float value)
        {
            var a=(soAgent.health * value) / 100;
            TakeDamage(a);
        }
        private void SetStartTarget()
        {
            if (AgentType==AgentType.Enemy)
            {
                if ( Domination.dominationMoveDirect==DominationMoveDirect.EnemyMove)
                {
                    agentState = AgentState.Waiting;
                    SlotTarget();
                }
                else
                {
                    target=_gameManager.dominationArea.transform;
                    attackDistance = 0.3f;
                    NavMeshAgent.stoppingDistance =  0.3f;
                }
                
            }
            else
            {
                if ( Domination.dominationMoveDirect==DominationMoveDirect.AlliesMove)
                {
                    agentState = AgentState.Waiting;
                    SlotTarget();
                }
                else
                {
                    target=_gameManager.dominationArea.transform;
                    attackDistance = 0.3f;
                    NavMeshAgent.stoppingDistance =  0.3f;
                }
            }
            NavMeshAgent.SetDestination(target.position);
        }
        private void AliveAgentCheck()
        {
            for (var i = 0; i < closeList.Count; i++)
            {
                if (closeList[i].GetHealth<=0)
                {
                    closeList.Remove(closeList[i]);
                }
            
            }
        }
        private AgentBase CloseAgent()
        {
       
            return closeList.OrderBy(go => (transform.position - go.transform.position).sqrMagnitude).First();
        }
        private float GetHealth => health;
        public void DominationMove()
        {
            if (agentState==AgentState.Waiting)
            {
                agentState = AgentState.Walking;
                target = Domination.transform;
            }
          
        }

       
        
    }
}