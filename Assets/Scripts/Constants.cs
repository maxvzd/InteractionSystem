using UnityEngine;

public static class Constants
{
    public static readonly string HorizontalKey = "Horizontal";
    public static readonly string VerticalKey = "Vertical";
    public static string UseKey => "Use";
    public static readonly string Fire1 = "Fire1";
    public static readonly string MouseX = "Mouse X";
    public static readonly string MouseY = "Mouse Y";

    public const string LAYER_TERRAIN = "Terrain";
    public const string LAYER_DOOR = "Door";
    public const string LAYER_ITEM = "Item";
    
    public static readonly int Vertical = Animator.StringToHash("Vertical");
    public static readonly int Horizontal = Animator.StringToHash("Horizontal");
    public static readonly int TurnRightTrigger = Animator.StringToHash("TurnRightTrigger");
    public static readonly int TurnLeftTrigger = Animator.StringToHash("TurnLeftTrigger");
    public static readonly int IsHoldingItem = Animator.StringToHash("IsHoldingItem");
}