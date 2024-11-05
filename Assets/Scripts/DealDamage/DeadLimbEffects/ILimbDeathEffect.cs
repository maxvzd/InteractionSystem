using RootMotion.Dynamics;
using UnityEngine;

namespace DealDamage.DeadLimbEffects
{
    public interface ILimbDeathEffect
    {
        public void Apply(BehaviourPuppet puppet, Animator animator);
    }
}