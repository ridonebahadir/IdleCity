using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AgentBase
{
    private IEnumerator _attack;
    protected override void AttackType()
    {
        _targetAgentBase.TakeDamage(_damage);
            //DetectTarget();
    }

    protected override void SlotTarget()
    {
        _target = _domination.SlotTarget(_agentType);
    }

    protected override void SlotTargetRemove()
    {
        _domination.SlotTargetRemove(_agentType);
    }
}
