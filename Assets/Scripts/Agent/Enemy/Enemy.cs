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
        target = Domination.SlotTarget(AgentType);
        attackDistance = 0.3f;
        NavMeshAgent.stoppingDistance =  0.3f;
    }

    protected override void SlotTargetRemove()
    {
        Domination.SlotTargetRemove(AgentType);
        attackDistance = soAgent.attackDistance;
        NavMeshAgent.stoppingDistance = attackDistance;
    }

    protected override void Flee(){}
    
}
