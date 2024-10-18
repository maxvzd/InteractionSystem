using RootMotion.FinalIK;
using UnityEngine;

public class PlayerOpenDoorSystem : MonoBehaviour
{
    public bool IsHoldingHandle => _isHoldingHandle;
    
    private InteractionSystem _interactionSystem;
    private PlayerState _playerState;
    private OpenDoor _currentDoor;
    private bool _isHoldingHandle;
    
    [SerializeField] private float distanceToLetGoOfDoor;

    private void Start()
    {
        _interactionSystem = GetComponent<InteractionSystem>();
        _playerState = GetComponent<PlayerState>();
    }

    public void Update()
    {
        if (!_isHoldingHandle) return;

         float distanceBetweenDoorAndPlayer = CheckDistanceBetweenHandleAndDoor();
        if (distanceBetweenDoorAndPlayer > distanceToLetGoOfDoor)
        {
            ReleaseHandle();
            return;
        }

        if (Input.GetButtonDown(Constants.Fire1))
        {
            _playerState.LockYLookDirection();
            _currentDoor.ChangeHingeLimitsToOpen();
        }

        if (Input.GetButtonDown(Constants.VerticalKey))
        {
            _currentDoor.ChangeHingeLimitsToOpen();
        }
        
        if (Input.GetButton(Constants.Fire1))
        {
            float mouseVerticalMovement = Input.GetAxis(Constants.MouseY);
            _currentDoor.SetDoorVelocity(mouseVerticalMovement);
        }
        else if (Input.GetButton(Constants.VerticalKey))
        {
            float verticalAxis = Input.GetAxis(Constants.VerticalKey);
            _currentDoor.SetDoorVelocity(verticalAxis * 0.5f);
        }
        else
        {
            _currentDoor.SetDoorVelocityToZero();
        }

        if (Input.GetButtonUp(Constants.Fire1) || Input.GetButtonUp(Constants.VerticalKey))
        {
            _currentDoor.ChangeHingeLimitsToClosed();
            _playerState.UnlockYLookDirection();
        }
    }

    private float CheckDistanceBetweenHandleAndDoor()
    {
        Vector3 doorHandlePosition = _currentDoor.PlayerIsInteractingFromFront ? _currentDoor.FrontHandlePosition : _currentDoor.BackHandlePosition;

        Vector3 scrubbedYCurrentTransform = transform.position;
        scrubbedYCurrentTransform.y = doorHandlePosition.y;

        return Vector3.Distance(scrubbedYCurrentTransform, doorHandlePosition);
    }

    public void OpenDoor(Transform door)
    {
        InteractionObject interactionObject = door.gameObject.GetComponent<InteractionObject>();
        if (ReferenceEquals(interactionObject, null)) return;

        _currentDoor = door.GetComponent<OpenDoor>();
        if (ReferenceEquals(_currentDoor, null)) return;

        _isHoldingHandle = true;
        _playerState.ClampMovementSpeed(0.7f);

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
        bool successful = false;
        if (_currentDoor.PlayerIsInteractingFromFront)
        {
            _interactionSystem.StopInteraction(FullBodyBipedEffector.RightHand);
            successful = true;
        }

        if (_currentDoor.PlayerIsInteractingFromBack)
        {
            _interactionSystem.StopInteraction(FullBodyBipedEffector.LeftHand);
            successful = true;
        }

        if (!successful) return;
        
        if (_currentDoor is not null)
        {
            _currentDoor.EndInteraction();
        }

        _isHoldingHandle = false;
        _playerState.UnlockYLookDirection();
        _currentDoor = null;
        _playerState.UnlockWalkSpeed();
    }
}