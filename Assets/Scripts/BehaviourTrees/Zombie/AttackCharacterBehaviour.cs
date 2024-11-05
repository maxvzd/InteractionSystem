using Constants;
using UnityEngine;

namespace BehaviourTrees.Zombie
{
    public class AttackCharacterBehaviour : Node
    {
        private readonly Animator _animator;

        public AttackCharacterBehaviour(Animator animator)
        {
            _animator = animator;
        }
        
        public override NodeState Evaluate()
        {
            _animator.SetTrigger(AnimatorConstants.AttackTrigger);
            return State =  NodeState.Success;
        }
    }
}