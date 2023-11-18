using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

namespace Agent.Enemy
{
    enum AgentState
    {
        Death,
        Life,
        Attack
    }
    public enum AgentType
    {
        Enemy,
        Player,
    }
    public abstract class AgentBase : MonoBehaviour
    {
        private WaitForSeconds _wait = new(1);
        private NavMeshAgent _navMeshAgent;
        private float _dist;
        public Health _targetHealth;

        private Coroutine _moveCoroutine;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            MoveToTarget();
        }

        IEnumerator Move()
        {
           
            while (true)
            {
                _dist = Vector3.Distance(_targetHealth.transform.position,transform.position);
                if (_dist<1)
                {
                    StartCoroutine(Attack());
                    break;
                }
                yield return _wait;
            }
        }

        IEnumerator Attack()
        {
            while (true)
            {
                if ( _targetHealth.TakeDamage(10))
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
            _navMeshAgent.destination = _targetHealth.transform.position;
            _moveCoroutine=StartCoroutine(Move());
        }
        
        protected abstract Health TargetDetection();
    }
}