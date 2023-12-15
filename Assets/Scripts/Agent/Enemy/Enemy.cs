using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AgentBase
{
    private IEnumerator _attack;
    protected override void AttackType()
    {
        if (_attack==null)
        {
            _attack = AttackCoroutine();
            StartCoroutine(_attack);
        }
            //DetectTarget();
    }

    protected override void SlotTarget()
    {
        _target = _domination.SlotTarget(_agentType);
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
                _attack = null;
                yield break;
                       
            }
            else
            {
                if (GetHealth>0)
                {
                           
                    yield return wait; 
                    _targetAgentBase.TakeDamage(_damage);
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
    

    
}
