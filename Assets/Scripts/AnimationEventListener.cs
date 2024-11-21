using System;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    public EventHandler FinishedTurning;
    public EventHandler JumpPeaked;
    public EventHandler FinishedReloading;
    
    private void OnFinishedTurning()
    {
        FinishedTurning?.Invoke(this, EventArgs.Empty);
    }
    
    private void JumpPeak()
    {
        JumpPeaked?.Invoke(this, EventArgs.Empty);
    } 
    
    private void OnFinishedReloading()
    {
        FinishedReloading?.Invoke(this, EventArgs.Empty);
    } 
}