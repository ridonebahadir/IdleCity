
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;


public abstract class AgentBase : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        protected WorkBase _target;
        protected GameManager _gm;
        private readonly WaitForSeconds _wait = new(1);
        private bool _goRiver = true;
        protected Collider _collider;
        private void Start()
        {
            _collider = GetComponent<Collider>();
            _gm = GameManager.Instance;
            _navMeshAgent = GetComponent<NavMeshAgent>();
             DetectTarget();
        }

        protected abstract void DetectTarget();
       
        protected IEnumerator MoveControl()
        {
            while (true)
            {
                _navMeshAgent.destination = _target.transform.position;
                var dist = Vector3.Distance(transform.position, _target.transform.position);
                if (dist<2)
                {
                    
                    StartCoroutine(Attack());
                    break;
                }

                yield return _wait;
            }
        }
        IEnumerator Attack()
        {
            while (_goRiver)
            {
                if (_target.TakeDamage(10))
                {
                    DetectTarget();
                }
                yield return _wait;
            }
        }
        protected abstract void OnTriggerEnter(Collider other);
    }

