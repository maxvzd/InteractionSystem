using DealDamage;

namespace BehaviourTrees.Zombie
{
    public class IsZombieDeadCheck : Node
    {
        private readonly MainHealthScript _zombieHealth;

        public IsZombieDeadCheck(MainHealthScript zombieHealth)
        {
            _zombieHealth = zombieHealth;
        }
        
        public override NodeState Evaluate()
        {
            if (_zombieHealth.Health <= 0f)
            {
                return State = NodeState.Success;
            }
            return State = NodeState.Failure;
        }
    }
}