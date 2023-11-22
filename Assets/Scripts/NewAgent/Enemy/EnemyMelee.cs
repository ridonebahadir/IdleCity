using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyAgentBase
{
    protected override void DetectTarget()
    {
        if (_gm.GetRandomTransformPoints(_gm.destroyRiverPoints)==null) return;
        _collider.enabled = true;
        _target = _gm.GetRandomTransformPoints(_gm.destroyRiverPoints).transform.GetComponent<WorkBase>();
        StartCoroutine(MoveControl());
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SoldierAgentBase soldierAgentBase))
        {
            _collider.enabled = false;
            soldierAgentBase.GetComponent<Collider>().enabled = false;
            _target = soldierAgentBase.GetComponent<WorkBase>();

        }
    }


  
}
