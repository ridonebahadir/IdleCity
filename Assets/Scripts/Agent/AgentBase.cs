using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domination;
using LeonBrave;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;


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
        [SerializeField] private AgentState agentState;
        [SerializeField] private ObjectType objectType;
        
        public SOAgent soAgent;
        public Transform target;
        public float dist;
        
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
        [SerializeField] private SmallTrigger small;
        [SerializeField] private Collider col;
        [SerializeField] protected NavMeshAgent NavMeshAgent;
        
        protected AgentType agentType; 
        protected Domination.Domination Domination;
        protected AgentBase TargetAgentBase;
        protected bool IsDeath;
        
        private float _diggSpeed;
        private float _cost;
        private GameManager _gameManager;
        protected SingletonHandler SingletonHandler;
        private readonly WaitForSeconds _wait = new(0.05f); 
        private IEnumerator _move;
        private float _navMeshStopDistance;
        private float _startRotateSpeed;
        private WaitForSeconds _firstAnimWaitForSeconds;
        private WaitForSeconds _secondAnimWaitForSeconds;
        private IEnumerator _attack;
        private float _maxHealth;
        private static readonly int Death1 = Animator.StringToHash("Death");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        private static readonly int Wait = Animator.StringToHash("Wait");
        
        
        
        public void InıtAgent()
        {
            Debug.Log("dşsglşi");
            _gameManager=GameManager.Instance;
            SingletonHandler = SingletonHandler.Instance;
            Domination = _gameManager.dominationArea;
            NavMeshAgent = GetComponent<NavMeshAgent>();
            NavMeshAgent.enabled = true;
            NavMeshAgent.stoppingDistance = soAgent.attackDistance;
            agentType = soAgent.agentType;
            speed = soAgent.speed;
            health = soAgent.health;
            _maxHealth = soAgent.health;
            damage = soAgent.damage;
            _diggSpeed = soAgent.diggSpeed;
            attackDistance = soAgent.attackDistance;
            _navMeshStopDistance = soAgent.attackDistance;
            _startRotateSpeed = NavMeshAgent.angularSpeed;
            NavMeshAgent.speed = speed;
            
            col.enabled = true;
            agentState = AgentState.Walking;
            _oneTimeRun = true;
            IsDeath = false;
            SetStartTarget();
            //SetPercentSpeed(100);
            animator.SetBool(Wait,false);
            _move = MoveTarget();
            StartCoroutine(_move);
            
            _firstAnimWaitForSeconds = new WaitForSeconds(firstAnimWait);
            _secondAnimWaitForSeconds = new WaitForSeconds(secondAnimWait);

        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out AgentBase otherAgentBase)) return;
            if (closeList.Contains(otherAgentBase)) return;
            closeList.Add(otherAgentBase);
            if (closeList.Count != 1) return;
            animator.SetBool(Wait,false);
            mesh.transform.localRotation = Quaternion.Euler(0,0,0);
            NavMeshAgent.angularSpeed = _startRotateSpeed;
            if (agentState!=AgentState.Fighting) SlotTargetRemove();
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
        

       

        public bool _oneTimeRun = true;
        [SerializeField] private Transform mesh;
        private readonly Vector3 _worldForwardDirection = Vector3.forward;
        private readonly Vector3 _worldBackwardDirection = Vector3.back;
        private IEnumerator MoveTarget()
        {
            while (true)
            {
                if (agentState == AgentState.Fighting)
                {
                    if (!TargetAgentBase._oneTimeRun)
                    { 
                        if (_attack!=null)StopCoroutine(_attack);
                        TargetDeath();
                        
                        _oneTimeRun = true;
                    }
                }
                if (dist <= attackDistance)
                {
                    if (agentState == AgentState.Fighting)
                    { 
                        //Flee();
                        Attack();
                    }
                    if (agentState==AgentState.Waiting)
                    {
                        //NavMeshAgent.angularSpeed = 0;
                        if (agentType==AgentType.Enemy) mesh.transform.rotation = Quaternion.LookRotation(_worldBackwardDirection, Vector3.up);
                        else mesh.transform.rotation = Quaternion.LookRotation(_worldForwardDirection, Vector3.up);

                        animator.SetBool(Wait,true);
                    }

                    if (agentState==AgentState.Walking)
                    {
                        //SlotTarget();
                        agentState = AgentState.Waiting;
                    }
                    
                }
                var pos = target.position;
                NavMeshAgent.SetDestination(pos);
                dist = Vector3.Distance(transform.position, pos);
                yield return _wait;
            }
            // ReSharper disable once IteratorNeverReturns
        }
        private void Attack()
        {
            animator.SetBool(Wait,false);
            mesh.transform.localRotation = Quaternion.Euler(0,0,0);
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
                if (GetHealth>0)
                {
                    yield return _firstAnimWaitForSeconds;
                    if (dist<=attackDistance) AttackType();
                    else TargetDeath();
                    yield return _secondAnimWaitForSeconds;
                }
                // if (TargetAgentBase.GetHealth<=0)
                // {
                //     TargetDeath();
                //    
                //     yield break;
                // }
                else
                {
                    // if (GetHealth>0)
                    // {
                    //     yield return _firstAnimWaitForSeconds;
                    //     Debug.Log(gameObject.name +" = "+target.name);
                    //     AttackType();
                    //     yield return _secondAnimWaitForSeconds;
                    // }
                    // else
                    // {
                    //     // SlotTarget();
                    //     // animator.SetBool(Attack1,false);
                    //     // yield break;
                    // }
                }
                yield return Wait; 
            }
                

        }
        private void TargetDeath()
        {
            if (_attack!=null) StopCoroutine(_attack);
            animator.SetBool(Attack1,false);
            agentState = AgentState.Walking;
            TargetAgentBase = null;
            _attack = null;
            AliveAgentCheck();
            if(closeList.Count>0)StartAttack();
            else SetStartTarget();
        }
        
        private void StartAttack()
        {
            attackDistance = soAgent.attackDistance;
            NavMeshAgent.stoppingDistance =_navMeshStopDistance;
            TargetAgentBase = CloseAgent();
            target = TargetAgentBase.transform;
            agentState = AgentState.Fighting;
            //_targetAgentBase = _target.transform.GetComponent<AgentBase>();
        }
        public void TakeDamage(float dmg)
        {
            DamagePopup.Create(healthBar.transform, dmg);
            healthBar.HealthBarCanvasGroupShow();
            particleSystem.Play();
            health -= dmg;
            healthBar.SetHealthBar(_maxHealth,health,agentType);
            if (health>0)
            {
            
            }
            else
            {
                _oneTimeRun = false;
                healthBar.StopHealthBarShow();
                StopCoroutine(_move);
                Death();
           
           
            }
       
        }

        private IEnumerator _deathIE;
        private void Death()
        {
            if (IsDeath) return;
            _deathIE = DeathIE();
            StartCoroutine(_deathIE);
            col.enabled = false;
            return;

            IEnumerator DeathIE()
            {
                IsDeath = true;
                Domination.UnRegister(small);
                animator.SetTrigger(Death1);
                if (agentType==AgentType.Enemy)  GetReward(); 
                _gameManager.RemoveList(this,agentType);
                
               
                yield return new WaitForSeconds(2.25f);
                NavMeshAgent.enabled = false;
                closeList.Clear();
                if (_attack != null)
                {
                    StopCoroutine(_attack);
                    _attack = null;
                }
                SingletonHandler.GetSingleton<ObjectPool>().AddObject(gameObject,objectType);
                
            }


        }
        void GetReward()
        {
            _gameManager.GetReward(soAgent.reward);
        }
        // public void SetPercentHealth(float value)
        // {
        //     var a = (soAgent.health * value) / 100; 
        //     health += a;
        //     if (health>=soAgent.health)  health = soAgent.health;
        // }
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
        public void SetStartTarget()
        {
            
            if (agentState==AgentState.Fighting) return;
            if (agentType==AgentType.Enemy)
            {
                if ( Domination.dominationMoveDirect==DominationMoveDirect.EnemyMove)
                {
                    SlotTarget();
                    //agentState = AgentState.Waiting;
                }
                else
                {
                    mesh.transform.localRotation = Quaternion.Euler(0,0,0);
                    target = Domination.transform;
                    attackDistance = 0.3f;
                    NavMeshAgent.stoppingDistance =  0f;
                }
                
            }
            else
            {
                if ( Domination.dominationMoveDirect==DominationMoveDirect.AlliesMove)
                {
                    
                    SlotTarget();
                    //agentState = AgentState.Waiting;
                }
                else
                {
                    mesh.transform.localRotation = Quaternion.Euler(0,0,0);
                    target = Domination.transform;
                    attackDistance = 0.3f;
                    NavMeshAgent.stoppingDistance =  0f;
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
        public float GetHealth => health;
        // public void DominationMove()
        // {
        //     if (agentState!=AgentState.Fighting)
        //     {
        //         agentState = AgentState.Walking;
        //         target = Domination.transform;
        //     }
        //   
        // }
        public void TakeHeal(float value)
        {
            if (IsDeath) return;
            healthBar._ishealth = true;
            if (!(health < _maxHealth)) return;
            health += value;
            if (health>=_maxHealth) health = _maxHealth;

        }

        public void TakeHealOver()
        {
            healthBar._ishealth = false;
        }

       
        
    }
}