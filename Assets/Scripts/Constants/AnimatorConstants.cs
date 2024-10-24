using UnityEngine;

namespace Constants
{
    public static class AnimatorConstants
    {
        public static readonly int Vertical = Animator.StringToHash("Vertical");
        public static readonly int Horizontal = Animator.StringToHash("Horizontal");
        public static readonly int TurnRightTrigger = Animator.StringToHash("TurnRightTrigger");
        public static readonly int TurnLeftTrigger = Animator.StringToHash("TurnLeftTrigger");
        public static readonly int IsHoldingTwoHandedGun = Animator.StringToHash("IsHoldingTwoHandedGun");
        public static readonly int IsHoldingPistol = Animator.StringToHash("IsHoldingPistol");
        public static readonly int IsAiming = Animator.StringToHash("IsAiming");
        public static readonly int EquipBackpackTrigger = Animator.StringToHash("EquipBackpackTrigger");
        public static readonly int UnEquipBackpackTrigger = Animator.StringToHash("UnEquipBackpackTrigger");
        public static readonly int BackpackIsOut = Animator.StringToHash("BackpackIsOut");
    }
}