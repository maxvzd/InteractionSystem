using PlayerAiming;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private DeadZoneLook playerLookBehaviour;
    private PlayerMovement _playerMovement;
    private AimBehaviour _playerAimBehaviour;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAimBehaviour = GetComponent<AimBehaviour>();

        _playerAimBehaviour.PlayerAiming += (sender, args) =>
        {
            playerLookBehaviour.UseDeadZone = false;
            playerLookBehaviour.LerpAimToLook();
            //_isAiming = true;
        };
        
        _playerAimBehaviour.PlayerNotAiming += (sender, args) =>
        {
            playerLookBehaviour.UseDeadZone = true;
            //_isAiming = false;
        };
    }

    public void LockYLookDirection()
    {
        playerLookBehaviour.LockYDirection();
    }
    
    public void UnlockYLookDirection()
    {
        playerLookBehaviour.UnlockYDirection();
    }

    public void UnlockWalkSpeed()
    {
        _playerMovement.UnlockWalkSpeed();
    }

    public void ClampMovementSpeed(float walkSpeed)
    {
        _playerMovement.ClampMovementSpeedTo(walkSpeed);
    }
}