using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMelee : SoldierAgentBase
{
    protected override void DetectTarget()
    {
        if (_gm.GetRandomTransformPoints(_gm.enemies)==null) return;
        _collider.enabled = true;
        _target = _gm.GetRandomTransformPoints(_gm.enemies).transform.GetComponent<WorkBase>();
        StartCoroutine(MoveControl());
    }

    protected override void OnTriggerEnter(Collider other)
    {
    }
}
