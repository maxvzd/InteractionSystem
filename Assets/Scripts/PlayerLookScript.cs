using System.Collections;
using UnityEngine;

public class PlayerLookScript : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private float maxVerticalAngle;
    [SerializeField] private Transform objectToRotate;

    private bool _lockYRotation;
    private IEnumerator _lerpToYZeroCoRoutine;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float rotationX = 0f;
        
        if (!_lockYRotation)
        {
             rotationX = Input.GetAxis(Constants.InputConstants.MouseY) * sensitivity;
        }
        float rotationY = Input.GetAxis(Constants.InputConstants.MouseX) * sensitivity;

        Vector3 lookRotation = objectToRotate.eulerAngles + new Vector3(-rotationX, rotationY, 0);
        lookRotation.x = lookRotation.x > 180 ? lookRotation.x - 360 : lookRotation.x;
        lookRotation.x = Mathf.Clamp(lookRotation.x, -maxVerticalAngle, maxVerticalAngle);
        objectToRotate.eulerAngles = lookRotation;
    }

    public void UnlockYDirection()
    {
        _lockYRotation = false;

        //if (!ReferenceEquals(_lerpToYZeroCoRoutine, null))
        //{
            //StopCoroutine(_lerpToYZeroCoRoutine);
        //}
    }

    public void LockYDirection()
    {
        _lockYRotation = true;
        //_lerpToYZeroCoRoutine = LerpToStraightAhead();
        //StartCoroutine(_lerpToYZeroCoRoutine);
    }

    private IEnumerator LerpToStraightAhead()
    {
        float timeElapsed = 0f;
        float lerpTo = 0.292f;
        
        Quaternion currentYRotation = objectToRotate.rotation;
        
        Quaternion lerpToRot = currentYRotation;
        lerpToRot.x = lerpTo;
        
        float timeToLerp = 1;
        
        while (timeElapsed < timeToLerp)
        {
            float t = timeElapsed / timeToLerp;

            objectToRotate.rotation = Quaternion.Lerp(currentYRotation, lerpToRot, t);
            
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    
    
}