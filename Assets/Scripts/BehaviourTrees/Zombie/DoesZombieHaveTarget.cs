using Constants;
using UnityEngine;

namespace BehaviourTrees.Zombie
{
    public class DoesZombieHaveTarget : Node
    {
        public override NodeState Evaluate() => GetData(BehaviourTreeData.TargetPositionData) is not null ? State = NodeState.Success : State = NodeState.Failure;
    }
}