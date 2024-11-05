using UnityEngine;

namespace Constants
{
    public static class AnimatorConstants
    {
        //PLayerConstants
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
        public static readonly int JumpTrigger = Animator.StringToHash("JumpTrigger");
        public static readonly int IsJumpLocationGrounded = Animator.StringToHash("IsJumpLocationGrounded");
        public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        public static readonly int FallIntensity = Animator.StringToHash("FallIntensity");
        public static readonly int RollTrigger = Animator.StringToHash("RollTrigger");
        public static readonly int WasCrouchPressed = Animator.StringToHash("WasCrouchPressed");
        public static readonly int IsCrouching = Animator.StringToHash("IsCrouching");
        
        //Zombie Constants
        public static readonly int Speed = Animator.StringToHash("Speed"); 
        public static readonly int IsMoving = Animator.StringToHash("IsMoving"); 
        public static readonly int IsCrippled = Animator.StringToHash("IsCrippled"); 
        public static readonly int AlertTrigger = Animator.StringToHash("AlertTrigger"); 
        public static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger"); 
    }
}