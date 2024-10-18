using System;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    public event EventHandler FinishedTurning;
    
    private void OnFinishedTurning()
    {
        FinishedTurning?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler FinishedStepping;

    private void OnFinishedStepping()
    {
        FinishedStepping?.Invoke(this, EventArgs.Empty);
    }
}