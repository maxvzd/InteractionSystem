using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Lerper
{
    public static IEnumerator Lerp(
        IEnumerable<ILerpComponent> lerpData,
        float lerpTime)
    {
        float elapsedTime = 0f;
        IList<ILerpComponent> lerpList = lerpData.ToList();

        while (elapsedTime < lerpTime)
        {
            float t = elapsedTime / lerpTime;

            foreach (ILerpComponent lerpComponent in lerpList)
            {
                lerpComponent.Lerp(t);
            }
            
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        
        foreach (ILerpComponent lerpComponent in lerpList)
        {
            lerpComponent.FinishLerp();
        }
    }
}

public interface ILerpComponent
{ 
    void Lerp(float t); 
    void FinishLerp();
}

public class LerpLocalVector : ILerpComponent
{
    public LerpLocalVector(Vector3 positionToLerpTo, Transform transform)
    {
        _positionToLerpTo = positionToLerpTo;
        _transform = transform;
        _originalPosition = transform.localPosition;
    }

    private readonly Transform _transform;
    private readonly Vector3 _positionToLerpTo;
    private readonly Vector3 _originalPosition;
    
    public void Lerp(float t)
    {
        _transform.localPosition = Vector3.Lerp(_originalPosition, _positionToLerpTo, t);
    }

    public void FinishLerp()
    {
        _transform.localPosition = _positionToLerpTo;
    }
}

public class LerpLocalQuaternion : ILerpComponent
{
    public LerpLocalQuaternion(Quaternion rotationToLerpTo, Transform transform)
    {
        _rotationToLerpTo = rotationToLerpTo;
        _transform = transform;
        _originalRotation = transform.localRotation;
    }

    private readonly Transform _transform;
    private readonly Quaternion _rotationToLerpTo;
    private readonly Quaternion _originalRotation;
    
    public void Lerp(float t)
    {
        _transform.localRotation = Quaternion.Lerp(_originalRotation, _rotationToLerpTo, t);
    }

    public void FinishLerp()
    {
        _transform.localRotation = _rotationToLerpTo;
    }
}

public class LerpQuaternion : ILerpComponent
{
    public LerpQuaternion(Quaternion rotationToLerpTo, Transform transform)
    {
        _rotationToLerpTo = rotationToLerpTo;
        _transform = transform;
        _originalRotation = transform.rotation;
    }

    private readonly Transform _transform;
    private readonly Quaternion _rotationToLerpTo;
    private readonly Quaternion _originalRotation;
    
    public void Lerp(float t)
    {
        _transform.rotation = Quaternion.Lerp(_originalRotation, _rotationToLerpTo, t);
    }

    public void FinishLerp()
    {
        _transform.rotation = _rotationToLerpTo;
    }
}

public class LerpFOV : ILerpComponent
{
    public LerpFOV(float fovToLerpTo, Camera camera)
    {
        _fovToLerpTo = fovToLerpTo;
        _camera = camera;
        _originalFov = camera.fieldOfView;
    }

    private readonly Camera _camera;
    private readonly float _fovToLerpTo;
    private readonly float _originalFov;
    
    public void Lerp(float t)
    {
        _camera.fieldOfView = Mathf.Lerp(_originalFov, _fovToLerpTo, t);
    }

    public void FinishLerp()
    {
        _camera.fieldOfView = _fovToLerpTo;
    }
}