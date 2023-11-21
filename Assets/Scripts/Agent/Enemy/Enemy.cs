using System.Collections;
using UnityEngine;

namespace Agent.Enemy
{
    public class Enemy : AgentBase
    {
        protected override Transform TargetDetection()
        {
            if (_isWar)
            {
                if (gm.soldiers.Count==0) return null;
                var target = gm.GetRandomHealths(gm.soldiers);
                return target.transform;
            }
            else
            {
                var target = gm.GetRandomHealths(gm.destroyRiverPoints);
                return target.transform;
            }
        }

        protected override void AttackType()
        {
            if (targetWorkBase.healthType==HealthType.DestroyRiverPoint)
            {
                StartCoroutine(Attack(1,5));
            }
            else
            {
                StartCoroutine(Attack(10,1));
            }
           
        }
        
    }
}
