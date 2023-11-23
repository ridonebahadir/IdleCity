using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : AgentBase
{
    protected override void DetectTarget()
    {
       
        if (_domination._enemies.Count>0)
        {
           Attack(_domination.CloseAgentEnemy(transform));
            //Attack(_domination._enemies[0].transform);
        }
        else
        {
            _target = GameManager.Instance.dominationArea.transform;
        }
    }

    protected override void RemoveList()
    {
        _domination.RemoveListSoldiers(this);
        //_domination._soldiers.Remove(this);
    }
}
