using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

namespace Agent.Enemy
{
    public enum AgentType
    {
        Enemy,
        Player,
    }
    public abstract class AgentBase : MonoBehaviour
    {
        [SerializeField] private AgentType agentType;
        
        private WaitForSeconds _wait = new(1);
        private NavMeshAgent _navMeshAgent;
        private float _dist;
        public Health _targetHealth;
        public Transform _patrolPoint;
        protected bool _isWar;
        protected GameManager gm;
        private void Start()
        {
            gm = GameManager.Instance;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            MoveToTarget();
        }

        IEnumerator Move()
        {
           
            while (true)
            {
                if (_targetHealth!=null)
                {
                    _navMeshAgent.destination = _targetHealth.transform.position;
                    _dist = Vector3.Distance(_targetHealth.transform.position,transform.position);
                    if (_dist<1)
                    {
                        if (agentType == AgentType.Enemy)
                        {
                            StartCoroutine(Attack());
                        
                        }
                        else
                        {
                            if (_isWar)
                            {
                                StartCoroutine(Attack());
                          
                            }
                            else
                            {
                                Debug.Log("Kışlaya Dön");
                            }
                        }
                        break;
                   
                    }
                }
                
                yield return _wait;
            }
        }
        IEnumerator Attack()
        {
            while (true)
            {
                if (_targetHealth!=null)
                {
                    if ( _targetHealth.TakeDamage(10))
                    {

                        MoveToTarget();
                        break;
                    }
                }
                else
                {
                    break;
                }
                yield return _wait;
            }
        }

        IEnumerator Patrol()
        {
            while (_isWar)
            {
                _navMeshAgent.destination = _patrolPoint.position;
                _dist = Vector3.Distance(_patrolPoint.position,transform.position);
                Debug.Log("dslghlj");
                if (_dist<1)
                {
                    MoveToTarget();
                    break;
                }
                yield return _wait;
            }
        }
        private void MoveToTarget()
        {
            _targetHealth = TargetDetection();
            if (_targetHealth==null)
            {
                StopCoroutine(Attack());
                _patrolPoint = gm.GetRandomPatrolPoints();
                StartCoroutine(Patrol());
            }
            else
            {
                StartCoroutine(Move());
            }
          
           
        }
        
        protected abstract Health TargetDetection();

       
        
        
        private void ShiftControl()
        {
           StopCoroutine(Attack());
            _isWar = !_isWar;
            MoveToTarget();
       
        }
        private void OnEnable()
        {
            UIManager.OnClickedShiftButton += ShiftControl;
        }

        private void OnDisable()
        {
            UIManager.OnClickedShiftButton -= ShiftControl;
        }
    }
}