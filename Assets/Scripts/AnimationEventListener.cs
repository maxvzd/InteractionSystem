using System;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    public EventHandler FinishedTurning;
    public EventHandler JumpPeaked;
    
    private void OnFinishedTurning()
    {
        FinishedTurning?.Invoke(this, EventArgs.Empty);
    }
    
    private void JumpPeak()
    {
        JumpPeaked?.Invoke(this, EventArgs.Empty);
    } 
}