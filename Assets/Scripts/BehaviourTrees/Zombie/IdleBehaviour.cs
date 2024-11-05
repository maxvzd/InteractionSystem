namespace BehaviourTrees.Zombie
{
    public class IdleBehaviour : Node
    {
        public override NodeState Evaluate()
        {
            return State = NodeState.Running;
        }
    }
}