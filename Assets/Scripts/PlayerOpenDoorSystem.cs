using System;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerOpenDoorSystem : MonoBehaviour
{
    public bool IsHoldingHandle => _isHoldingHandle;


    private InteractionSystem _interactionSystem;
    private PlayerState _playerState;
    private OpenDoor _currentDoor;
    private bool _isHoldingHandle;
    private Animator _animator;
    private bool _isStepping;
    
    [SerializeField] private float distanceToLetGoOfDoor;

    private void Start()
    {
        _interactionSystem = GetComponent<InteractionSystem>();
        _playerState = GetComponent<PlayerState>();
        _animator = GetComponent<Animator>();
        GetComponent<AnimationEventListener>().FinishedStepping += (sender, args) =>
        {
            _isStepping = false;
        };
    }

    public void Update()
    {
        if (!_isHoldingHandle) return;

         float distanceBetweenDoorAndPlayer = CheckDistanceBetweenHandleAndDoor();
        // if (distanceBetweenDoorAndPlayer > distanceToLetGoOfDoor)
        // {
        //     ReleaseHandle();
        //     return;
        // }
        

        float verticalAxis = Input.GetAxis(Constants.VerticalKey);
        float horizontalAxis = Input.GetAxis(Constants.HorizontalKey);

        if (!_isStepping)// && distanceBetweenDoorAndPlayer < distanceToLetGoOfDoor)
        {
            switch (verticalAxis)
            {
                case > 0.1f:
                    _isStepping = true;
                    _animator.SetTrigger(Constants.StepForwardTrigger);
                    break;
                case < -0.1f:
                    _isStepping = true;
                    _animator.SetTrigger(Constants.StepBackwardTrigger);
                    break;
            }

            switch (horizontalAxis)
            {
                case > 0.1f:
                    _isStepping = true;
                    _animator.SetTrigger(Constants.StepRightTrigger);
                    break;
                case < -0.1f:
                    _isStepping = true;
                    _animator.SetTrigger(Constants.StepLeftTrigger);
                    break;
            } 
        }
        

        if (Input.GetButtonDown(Constants.Fire1))
        {
            _playerState.LockYLookDirection();
            _currentDoor.ChangeHingeLimitsToOpen();
        }

        if (Input.GetButton(Constants.Fire1))
        {
            float mouseVerticalMovement = -Input.GetAxis(Constants.MouseY);
            _currentDoor.SetDoorVelocity(mouseVerticalMovement);
        }
        else
        {
            _currentDoor.SetDoorVelocityToZero();
        }

        if (Input.GetButtonUp(Constants.Fire1))
        {
            _currentDoor.LatchDoorIfFullyClosed();
            _playerState.UnlockYLookDirection();
        }
    }

    private float CheckDistanceBetweenHandleAndDoor()
    {
        Vector3 doorHandlePosition = _currentDoor.PlayerIsInteractingFromFront ? _currentDoor.FrontHandlePosition : _currentDoor.BackHandlePosition;

        Vector3 scrubbedYCurrentTransform = transform.position;
        scrubbedYCurrentTransform.y = doorHandlePosition.y;

        Debug.DrawLine(doorHandlePosition, scrubbedYCurrentTransform, Color.red);

        return Vector3.Distance(scrubbedYCurrentTransform, doorHandlePosition);
        //return distanceBetweenDoorAndBody > distanceToLetGoOfDoor;
    }

    public void OpenDoor(Transform door)
    {
        InteractionObject interactionObject = door.gameObject.GetComponent<InteractionObject>();
        if (ReferenceEquals(interactionObject, null)) return;

        _currentDoor = door.GetComponent<OpenDoor>();
        if (ReferenceEquals(_currentDoor, null)) return;

        _isHoldingHandle = true;
        _animator.SetBool(Constants.IsInteractingWithDoor, true);

        if (Vector3.Dot((door.position - transform.position).normalized, door.right) < 0)
        {
            _currentDoor.InteractWithDoor(true);
            _interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, interactionObject, false);
        }
        else
        {
            _currentDoor.InteractWithDoor(false);
            _interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, interactionObject, false);
        }
    }

    public void ReleaseHandle()
    {
        if (_currentDoor.PlayerIsInteractingFromFront)
        {
            _interactionSystem.StopInteraction(FullBodyBipedEffector.RightHand);
        }

        if (_currentDoor.PlayerIsInteractingFromFront)
        {
            _interactionSystem.StopInteraction(FullBodyBipedEffector.LeftHand);
        }

        if (_currentDoor is not null)
        {
            _currentDoor.EndInteraction();
        }

        _isHoldingHandle = false;
        _playerState.UnlockYLookDirection();
        _currentDoor = null;
        _animator.SetBool(Constants.IsInteractingWithDoor, false);
    }
}