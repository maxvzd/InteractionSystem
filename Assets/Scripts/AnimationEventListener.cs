using System;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    public delegate void OnFinishedTurningEvent(object sender, EventArgs e);
    public event OnFinishedTurningEvent OnFinishedTurning;
    
    private void FinishedTurning()
    {
        OnFinishedTurning?.Invoke(this, EventArgs.Empty);
    }
}