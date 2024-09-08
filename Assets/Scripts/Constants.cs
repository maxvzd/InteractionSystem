using UnityEngine;

public static class Constants
{
    public static readonly string HorizontalKey = "Horizontal";
    public static readonly string VerticalKey = "Vertical";
    
    public static readonly int Vertical = Animator.StringToHash("Vertical");
    public static readonly int Horizontal = Animator.StringToHash("Horizontal");
    public static readonly int TurnRightTrigger = Animator.StringToHash("TurnRightTrigger");
    public static readonly int TurnLeftTrigger = Animator.StringToHash("TurnLeftTrigger");
}