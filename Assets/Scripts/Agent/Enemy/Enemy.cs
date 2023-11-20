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
            if (targetWorkBase.healthType==HealthType.RiverPoint)
            {
                StartCoroutine(DestructRiver());
            }
            else
            {
                StartCoroutine(Attack());
            }
        }
        IEnumerator DestructRiver()
        {
            while (true)
            {
                if (targetWorkBase.DestructRiver())
                {
                    
                }
                yield return _wait;
            }
           
        }
    }
}
