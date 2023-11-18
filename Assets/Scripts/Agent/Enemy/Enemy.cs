namespace Agent.Enemy
{
    public class Enemy : AgentBase
    {
        protected override Health TargetDetection()
        {
            var target = GameManager.Instance.buildManager.RandomTransform();
            return target;
        }
    }
}
