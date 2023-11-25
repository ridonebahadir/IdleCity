using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AgentBase
{
    protected override void AttackType()
    {
        if (_agentBase!=null)
        {
            if (_agentBase.TakeDamage(5))
            {
                DetectTarget();
            }
        }
        else
        {
                    
        }
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
