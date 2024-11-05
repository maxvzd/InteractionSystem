namespace BehaviourTrees.Zombie
{
    public class DeadZombieBehaviour : Node
    {
        private readonly ZombieBehaviourTree _behaviourTree;

        public DeadZombieBehaviour(ZombieBehaviourTree behaviourTree)
        {
            _behaviourTree = behaviourTree;
        }
        
        public override NodeState Evaluate()
        {
            _behaviourTree.isAiEnabled = false;
            return State = NodeState.Success;
        }
    }
}