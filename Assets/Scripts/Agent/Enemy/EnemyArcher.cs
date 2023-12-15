using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class EnemyArcher : AgentBase
{
    [SerializeField] private GameObject arrowObj;
    private IEnumerator _attack;
    protected override void AttackType()
    {
        if (_attack==null)
        {
            _attack = AttackCoroutine();
            StartCoroutine(_attack);
            //StopCoroutine(MoveTarget());
        }
        
       
    }

    protected override void SlotTarget()
    {
        _target = _domination.SlotArcherTarget(_agentType);
    }

    IEnumerator AttackCoroutine()
    {
        WaitForSeconds wait = new(2);
        animator.SetBool("Attack",true);
        while (true)
        {
            
            
                if (_targetAgentBase.GetHealth<=0)
                {
                    animator.SetBool("Attack",false);
                    _collider.enabled = true;
                    agentState = AgentState.Walking;
                    //StartCoroutine(MoveTarget());
                    _attack = null;
                    yield break;
                       
                }
                else
                {
                    if (GetHealth>0)
                    {
                           
                        yield return wait;
                        ThrowArrow();
                        yield return new WaitForSeconds(0.6f);
                        //_targetAgentBase.TakeDamage(_damage);
                    }
                    else
                    {
                        animator.SetBool("Attack",false);
                        yield break;
                    }
                        
                }
            
            
           
            yield return null; 
        }
                

    }
    // protected override IEnumerator MoveTarget()
    // {
    //     while (true)
    //     {
    //         _dist = Vector3.Distance(transform.position, _target.position);
    //         
    //         if (_dist <_attackDistance && _targetAgentBase!=null)
    //         {
    //             if (agentState==AgentState.Fighting)
    //             {
    //                 Flee(_target.position);
    //                 AttackType();
    //             }
    //             else
    //             {
    //                 animator.SetBool("Digg",true);
    //             }
    //             
    //         }
    //         else
    //         {
    //             Seek(_target.position);
    //         }
    //        
    //         yield return _wait;
    //     }
    // }


    private void Flee(Vector3 location)
     {
         Vector3 fleeVector = location - transform.position;
         navMeshAgent.SetDestination(transform.position - fleeVector);
     }

    private void Seek(Vector3 location)
     {
         navMeshAgent.SetDestination(location);
     }

    void ThrowArrow()
    {
        if (isDeath) return;
        var arrow = Instantiate(arrowObj,transform.position,UnityEngine.Quaternion.identity,transform);
        arrow.gameObject.SetActive(true);
        arrow.transform.SetParent(_target);
        arrow.transform.DOLocalJump(Vector3.zero, 6, 0, 0.5f).OnComplete(() =>
        {
            arrow.gameObject.SetActive(false);
            arrow.transform.SetParent(transform);
            arrow.transform.localPosition = Vector3.zero;
            _targetAgentBase.TakeDamage(_damage);
            //_targetAgentBase = null;
        });
    }
    

  
}
