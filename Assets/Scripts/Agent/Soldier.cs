using Agent.Enemy;

namespace Agent
{
    public class Soldier : AgentBase
    {
        protected override Health TargetDetection()
        {
            if (_isWar)
            {
                if (gm.enemies.Count==0) return null;
                var target = gm.GetRandomEnemies();
                return target;
            }
            else
            {
                var target =gm.buildManager.GetRandomSoldierBuild();
                return target;
            }
        
        }

    
    }
}
