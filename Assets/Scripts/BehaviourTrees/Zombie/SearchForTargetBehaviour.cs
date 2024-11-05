using UnityEngine;

namespace BehaviourTrees.Zombie
{
    public class SearchForTargetBehaviour : Node
    {
        public override NodeState Evaluate()
        {
            return State =  NodeState.Success;
        }
    }
}