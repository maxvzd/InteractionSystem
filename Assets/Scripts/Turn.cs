using System;
using UnityEngine;

public class Turn : MonoBehaviour
{
    [SerializeField] private float angleToTurnAt;
    [SerializeField] private Transform cameraTransform;
    
    [SerializeField] private Transform target;
    [SerializeField] private float turnSpeed;

    private bool _isTurning;
    private Animator _animator;
    private AnimationEventListener _animEventListener;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        
        _animEventListener = GetComponent<AnimationEventListener>();
        _animEventListener.OnFinishedTurning += (sender, args) =>
        {
            _isTurning = false;
        };
    }
    
    private void Update()
    {
        float verticalInput = Input.GetAxis(Constants.VerticalKey);
        float horizontalInput = Input.GetAxis(Constants.HorizontalKey);
        
        if (verticalInput > 0 || verticalInput < 0 || horizontalInput > 0 || horizontalInput < 0)
        {
            var currentPosition = transform.position;
            var targetPosition = target.position;
            
            Vector3 relativePos = new Vector3(targetPosition.x, currentPosition.y, targetPosition.z) - currentPosition;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
            _isTurning = false;
        }
        else if (!_isTurning)
        {
            float angleBetweenCameraAndBody = cameraTransform.eulerAngles.y - transform.eulerAngles.y;
            if (angleBetweenCameraAndBody < 0)
            {
                angleBetweenCameraAndBody += 360;
            }

            if (angleBetweenCameraAndBody > 0 + angleToTurnAt &&  angleBetweenCameraAndBody < 180)
            {
                _isTurning = true;
                _animator.SetTrigger(Constants.TurnRightTrigger);
            }

            if (angleBetweenCameraAndBody > 180 && angleBetweenCameraAndBody < 360 - angleToTurnAt)
            {
                _isTurning = true;
                _animator.SetTrigger(Constants.TurnLeftTrigger);
            }
        }
    }
}
