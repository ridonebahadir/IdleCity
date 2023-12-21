using System.Collections;
using System.Collections.Generic;
using Agent;
using UnityEngine;

public class Enemy : AgentBase
{
    private IEnumerator _attack;
    protected override void AttackType()
    {
        TargetAgentBase.TakeDamage(damage);
            //DetectTarget();
    }

    protected override void SlotTarget()
    {
        target = Domination.SlotTarget(agentType);
        attackDistance = 0.5f;
        NavMeshAgent.stoppingDistance =  0;
        
    }

    protected override void SlotTargetRemove()
    {
        Domination.SlotTargetRemove(agentType);
        attackDistance = soAgent.attackDistance;
        NavMeshAgent.stoppingDistance = attackDistance;
    }

    protected override void Flee(){}
    
}
