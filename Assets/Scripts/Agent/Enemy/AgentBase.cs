using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
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
        
        protected WaitForSeconds _wait = new(1);
        private NavMeshAgent _navMeshAgent;
        private float _dist; 
        public WorkBase targetWorkBase;
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
                if (targetWorkBase!=null)
                {
                    _navMeshAgent.destination = targetWorkBase.transform.position;
                    _dist = Vector3.Distance(targetWorkBase.transform.position,transform.position);
                    if (_dist<1)
                    {
                        AttackType();
                    }
                }
                
                yield return _wait;
            }
        }

       

        protected IEnumerator Attack()
        {
            while (true)
            {
                if (targetWorkBase!=null)
                {
                    if ( targetWorkBase.TakeDamage(10))
                    {
                        targetWorkBase = null;
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
            var targetDetection = TargetDetection();
            if (targetDetection==null)
            {
                StopCoroutine(Attack());
                _patrolPoint = gm.GetRandomTransformPoints(gm.patrolPoints);
                StartCoroutine(Patrol());
            }
            else
            {
                targetWorkBase = targetDetection.GetComponent<WorkBase>();
                StartCoroutine(Move());
            }
          
           
        }
        
        protected abstract Transform TargetDetection();
        protected abstract void AttackType();

       
        
        
        private void ShiftControl()
        {
           //StopCoroutine(Attack());
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