using RootMotion.Dynamics;
using UnityEngine;

namespace DealDamage.DeadLimbEffects
{
    public abstract class LimbDeathEffect : ScriptableObject, ILimbDeathEffect
    {
        public abstract void Apply(BehaviourPuppet puppet, Animator animator);
    }
}