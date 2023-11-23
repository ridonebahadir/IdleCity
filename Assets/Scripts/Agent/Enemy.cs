using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AgentBase
{
    protected override void DetectTarget()
    {
        if (_domination._soldiers.Count>0)
        {
            Attack(_domination.CloseAgentSoldier(transform));
            //Attack(_domination._soldiers[0].transform);
        }
        else
        {
            _target = GameManager.Instance.dominationArea.transform;
        }
    }

    protected override void RemoveList()
    {
        _domination._enemies.Remove(this);
    }
}
