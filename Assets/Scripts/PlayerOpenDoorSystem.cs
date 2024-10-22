using Constants;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOpenDoorSystem : MonoBehaviour
{
    public bool IsHoldingHandle => _isHoldingHandle;

    private InteractionSystem _interactionSystem;
    private PlayerState _playerState;
    private OpenDoor _currentDoor;
    private bool _isHoldingHandle;

    [SerializeField] private float distanceToLetGoOfDoor;

    private PlayerMovement _playerMovement;
    private PlayerInput _playerInput;
    private InputAction _lookAction;
    [SerializeField] private float doorMoveSensitivity;
    private InputAction _fireAction;

    private void Start()
    {
        _interactionSystem = GetComponent<InteractionSystem>();
        _playerState = GetComponent<PlayerState>();

        _playerMovement = GetComponent<PlayerMovement>();
        _playerInput = GetComponent<PlayerInput>();
        _lookAction = _playerInput.actions[InputConstants.LookAction];
        _fireAction = _playerInput.actions[InputConstants.FireAction];
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

        if (_fireAction.WasPressedThisFrame())
        {
            _playerState.LockYLookDirection();
            _currentDoor.ChangeHingeLimitsToOpen();
        }

        if (_fireAction.IsPressed())
        {
            
            float mouseVerticalMovement = _lookAction.ReadValue<Vector2>().y;
            _currentDoor.SetDoorVelocity(mouseVerticalMovement * doorMoveSensitivity);
        }
        else if (_playerMovement.IsMovingVertically)
        {
            _currentDoor.ChangeHingeLimitsToOpen();
            _currentDoor.SetDoorVelocity(_playerMovement.CurrentSpeed.y * 0.5f);
        }
        else
        {
            _currentDoor.SetDoorVelocityToZero();
        }

        if (_fireAction.WasReleasedThisFrame())
        {
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