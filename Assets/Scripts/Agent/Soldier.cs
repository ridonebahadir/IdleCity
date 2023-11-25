using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : AgentBase
{
    protected override void AttackType()
    {
        if (_agentBase!=null)
        {
            _agentBase.TakeDamage(5);
            DetectTarget();
            
        }
        else
        {
                    
        }
    }

   void DetectTarget()
    {
       
        if (_domination._enemies.Count>0 && isInside)
        {
           Attack(_gameManager.CloseAgentEnemy(transform));
         
        }
        else
        {
            _target = GameManager.Instance.dominationArea.transform;
        }
    }

   
}
