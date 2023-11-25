using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AgentBase
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

    

    
}
