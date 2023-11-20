using Agent.Enemy;
using UnityEngine;

namespace Agent
{
    public class Soldier : AgentBase
    {
        protected override Transform TargetDetection()
        {
            if (_isWar)
            {
                if (gm.enemies.Count==0) return null;
                var target = gm.GetRandomHealths(gm.enemies)    ;
                return target.transform;
            }
            else
            {
                var target =gm.buildManager.GetRandomSoldierBuild();
                return target.transform;
            }
        
        }

        protected override void AttackType()
        {
            if (_isWar)
            {
                StartCoroutine(Attack());
                          
            }
            else
            {
                Debug.Log("Kışlaya Girdi");
            }
        }
    }
}
