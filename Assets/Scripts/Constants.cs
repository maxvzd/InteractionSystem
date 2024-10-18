using UnityEngine;

public static class Constants
{
    public static readonly string HorizontalKey = "Horizontal";
    public static readonly string VerticalKey = "Vertical";
    public static readonly string UseKey = "Use";
    public static readonly string Fire1 = "Fire1";
    public static readonly string Fire2Key = "Fire2";
    public static readonly string SwapWeaponKey = "SwapWeapon";
    public static readonly string MouseX = "Mouse X";
    public static readonly string MouseY = "Mouse Y";
    public static readonly string JumpKey= "Jump";

    public const string LAYER_TERRAIN = "Terrain";
    public const string LAYER_DOOR = "Door";
    public const string LAYER_ITEM = "Item";
    public const string LAYER_GUN = "Gun";
    public const string LAYER_PLAYER = "Player";
    
    public static readonly int Vertical = Animator.StringToHash("Vertical");
    public static readonly int Horizontal = Animator.StringToHash("Horizontal");
    public static readonly int TurnRightTrigger = Animator.StringToHash("TurnRightTrigger");
    public static readonly int TurnLeftTrigger = Animator.StringToHash("TurnLeftTrigger");
    public static readonly int IsHoldingItem = Animator.StringToHash("IsHoldingItem");
    public static readonly int IsHoldingTwoHandedGun = Animator.StringToHash("IsHoldingTwoHandedGun");
    public static readonly int IsHoldingPistol = Animator.StringToHash("IsHoldingPistol");
    public static readonly int IsAiming = Animator.StringToHash("IsAiming");
    public static readonly int StepForwardTrigger = Animator.StringToHash("StepForwardTrigger");
    public static readonly int StepBackwardTrigger = Animator.StringToHash("StepBackwardTrigger");
    public static readonly int StepRightTrigger = Animator.StringToHash("StepRightTrigger");
    public static readonly int StepLeftTrigger = Animator.StringToHash("StepLeftTrigger");
    public static readonly int IsInteractingWithDoor = Animator.StringToHash("IsInteractingWithDoor");
    
}