using System.Collections;
using UnityEngine;

public class CoRoutineStarter
{
    private IEnumerator _coRoutine;
    private readonly MonoBehaviour _associatedBehaviour;

    public CoRoutineStarter(MonoBehaviour associatedBehaviour)
    {
        _associatedBehaviour = associatedBehaviour;
    }
    
    public void Start(IEnumerator coRoutine)
    {
        if (_coRoutine is not null)
        {
            _associatedBehaviour.StopCoroutine(_coRoutine);
            _coRoutine = null;
        }

        _coRoutine = coRoutine;
        _associatedBehaviour.StartCoroutine(_coRoutine);
    }
}