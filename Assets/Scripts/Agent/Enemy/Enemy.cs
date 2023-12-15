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
    }

    protected override void SlotTargetRemove()
    {
        Domination.SlotTargetRemove(AgentType);
    }

   
}
