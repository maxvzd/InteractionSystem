using RootMotion.FinalIK;
using UnityEngine;

public class PlayerOpenDoorSystem : MonoBehaviour
{
    private InteractionSystem _interactionSystem;

    private bool _isHoldingHandle;
    private PlayerState _playerState;
    public bool IsHoldingHandle => _isHoldingHandle;

    private OpenDoor _currentDoor;

    [SerializeField] private float distanceToLetGoOfDoor;

    private void Start()
    {
        _interactionSystem = GetComponent<InteractionSystem>();
        _playerState = GetComponent<PlayerState>();
    }

    public void Update()
    {
        if (!_isHoldingHandle) return;

        if (CheckDistanceBetweenHandleAndDoor())
        {
            RemoveHandFromHandle();
            return;
        }

        if (Input.GetButtonDown(Constants.Fire1))
        {
            _playerState.LockYLookDirection();
        }

        if (Input.GetButtonUp(Constants.Fire1))
        {
            _playerState.UnlockYLookDirection();
        }
    }

    private bool CheckDistanceBetweenHandleAndDoor()
    {
        Vector3 scrubbedYDoorHandlePosition = _currentDoor.FrontHandlePosition;
        scrubbedYDoorHandlePosition.y = 0f;

        Vector3 scrubbedYCurrentTransform = transform.position;
        scrubbedYCurrentTransform.y = 0f;

        float distanceBetweenDoorAndBody = Vector3.Distance(scrubbedYCurrentTransform, scrubbedYDoorHandlePosition);
        return distanceBetweenDoorAndBody > distanceToLetGoOfDoor;
    }

    public void OpenDoor(Transform door)
    {
        InteractionObject interactionObject = door.gameObject.GetComponent<InteractionObject>();
        if (ReferenceEquals(interactionObject, null)) return;

        _interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, interactionObject, true);

        _isHoldingHandle = true;
        _currentDoor = door.GetComponent<OpenDoor>();
        if (!ReferenceEquals(_currentDoor, null))
        {
            _currentDoor.PlayerIsInteracting = true;
        }

        _playerState.LockWalkSpeedTo(0.5f);
    }

    public void RemoveHandFromHandle()
    {
        _interactionSystem.StopInteraction(FullBodyBipedEffector.RightHand);

        _isHoldingHandle = false;
        if (!ReferenceEquals(_currentDoor, null))
        {
            _currentDoor.PlayerIsInteracting = false;
        }
        
        _playerState.UnlockYLookDirection();
        _playerState.UnlockWalkSpeed();

        _currentDoor = null;
    }
}