using PlayerAiming;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private DeadZoneLook playerLookBehaviour;
    private PlayerMovement _playerMovement;
    private AimBehaviour _playerAimBehaviour;

    private void Start()
    {
        //_playerLookScript = GetComponent<PlayerLookScript>();
        _playerMovement = GetComponent<PlayerMovement>();
        
        //_playerLookBehaviour = GetComponentInChildren<DeadZoneLook>();
        //playerLookBehaviour.UseDeadZone = true;
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

    // public void LockYLookDirection()
    // {
    //     _playerLookScript.LockYDirection();
    // }
    //
    // public void UnlockYLookDirection()
    // {
    //     _playerLookScript.UnlockYDirection();
    // }

    public void UnlockWalkSpeed()
    {
        _playerMovement.UnlockWalkSpeed();
    }

    public void LockWalkSpeedTo(float walkSpeed)
    {
        _playerMovement.LockMovementSpeedTo(walkSpeed);
    }
}