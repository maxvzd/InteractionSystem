using RootMotion.FinalIK;
using UnityEngine;

public class PlayerOpenDoorSystem : MonoBehaviour
{
    private InteractionSystem _interactionSystem;

    private bool _isHoldingHandle;
    private PlayerState _playerState;
    public bool IsHoldingHandle => _isHoldingHandle;
    
    private OpenDoor _currentDoor;
    
    private void Start()
    {
        _interactionSystem = GetComponent<InteractionSystem>();
        _playerState = GetComponent<PlayerState>();
    }

    public void Update()
    {

        if (_isHoldingHandle && Input.GetButtonDown(Constants.Fire1))
        {
            _playerState.LockYLookDirection();
        }
        
        

        if (_isHoldingHandle && Input.GetButtonUp(Constants.Fire1))
        {
            _playerState.UnlockYLookDirection();
        }
    }

    public void OpenDoor(Transform door)
    {
        InteractionObject interactionObject = door.gameObject.GetComponent<InteractionObject>();
        if (ReferenceEquals(interactionObject, null)) return;
        
        _isHoldingHandle = _interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, interactionObject, true);
        _currentDoor = door.GetComponent<OpenDoor>();
        if (!ReferenceEquals(_currentDoor, null))
        {
            _currentDoor.PlayerIsInteracting = true;
        }
    }

    public void RemoveHandFromHandle()
    {
        _interactionSystem.ResumeInteraction(FullBodyBipedEffector.RightHand);
        _isHoldingHandle = false;
        if (!ReferenceEquals(_currentDoor, null))
        {
            _currentDoor.PlayerIsInteracting = false;
        }
    }
}