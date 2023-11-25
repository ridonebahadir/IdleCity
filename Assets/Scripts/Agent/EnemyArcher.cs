using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class EnemyArcher : AgentBase
{
    [SerializeField] private GameObject arrowObj;
    
    protected override void AttackType()
    {
        var arrow = Instantiate(arrowObj,transform.position,UnityEngine.Quaternion.identity,transform);
        arrow.gameObject.SetActive(true);
        arrow.transform.SetParent(_target);
        arrow.transform.DOLocalJump(Vector3.zero, 2, 0, 0.5f).OnComplete(() =>
        {
            arrow.gameObject.SetActive(false);
            arrow.transform.SetParent(transform);
            arrow.transform.localPosition = Vector3.zero;
            _agentBase.TakeDamage(2);
            DetectTarget();
            
        });
    }

    protected override IEnumerator MoveDominationArea()
    {
        while (true)
        {
            _dist = Vector3.Distance(transform.position, _target.position);
            
            if (_dist <attackDistance && _agentBase!=null)
            {
                Flee(_target.position);
                AttackType();
            }
            else
            {
                Seek(_target.position);
            }
           
            yield return _wait;
        }
    }


     void Flee(Vector3 location)
     {
         Vector3 fleeVector = location - transform.position;
         navMeshAgent.SetDestination(transform.position - fleeVector);
     }

     void Seek(Vector3 location)
     {
         navMeshAgent.SetDestination(location);
     }
        
    
    void DetectTarget()
    {
        if (_domination._soldiers.Count>0)
        {
            Attack(_gameManager.CloseAgentSoldier(transform));
        }
        else
        {
            _target = GameManager.Instance.dominationArea.transform;
        }
    }

  
}
