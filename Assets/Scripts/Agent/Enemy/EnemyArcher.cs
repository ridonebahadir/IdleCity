using System.Collections;
using System.Collections.Generic;
using Agent;
using DG.Tweening;
using LeonBrave;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class EnemyArcher : AgentBase
{
    [SerializeField] private GameObject arrowObj;
    [SerializeField] private ObjectType arrowType;
    
    private IEnumerator _attack;
    protected override void AttackType()
    {
       ThrowArrow();
    }

    protected override void SlotTarget()
    {
        target = Domination.SlotArcherTarget(agentType);
        attackDistance =0.5f;
        NavMeshAgent.stoppingDistance = 0f;
        
    }

    protected override void SlotTargetRemove()
    {
        Domination.SlotTargetArcherRemove(agentType);
        attackDistance = soAgent.attackDistance;
        NavMeshAgent.stoppingDistance = attackDistance;
    }

    protected override void Flee()
    {
        FleeArcher();
    }

    private void FleeArcher()
     {
         Vector3 fleeVector = target.position - transform.position;
         NavMeshAgent.SetDestination(transform.position - fleeVector);
     }

    private void Seek(Vector3 location)
     {
         NavMeshAgent.SetDestination(location);
     }

    void ThrowArrow()
    {
        if (IsDeath) return;
        var arrow = SingletonHandler.GetSingleton<ObjectPool>().TakeObject(arrowType);
        //var arrow = Instantiate(arrowObj,transform.position,UnityEngine.Quaternion.identity,transform);
        arrow.transform.position = transform.position;
        arrow.transform.SetParent(transform);
        arrow.gameObject.SetActive(true);
        arrow.transform.SetParent(target);
        arrow.transform.DOLocalJump(Vector3.zero, 6, 0, 0.5f).OnComplete(() =>
        {
            arrow.gameObject.SetActive(false);
            arrow.transform.SetParent(transform);
            arrow.transform.localPosition = Vector3.zero;
            if (TargetAgentBase!=null) TargetAgentBase.TakeDamage(damage);
            //_targetAgentBase = null;
        });
    }
  
}
