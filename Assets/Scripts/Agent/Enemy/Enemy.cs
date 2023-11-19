namespace Agent.Enemy
{
    public class Enemy : AgentBase
    {
        protected override Health TargetDetection()
        {
            if (_isWar)
            {
                if (gm.soldiers.Count==0) return null;
                var target = gm.GetRandomSoldiers();
                return target;
            }
            else
            {
                var target = gm.buildManager.GetRandomCivilianBuild();
                return target;
            }
        }
    }
}
