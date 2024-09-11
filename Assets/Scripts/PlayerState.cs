using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private PlayerLookScript _playerLookScript;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerLookScript = GetComponent<PlayerLookScript>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void LockYLookDirection()
    {
        _playerLookScript.LockYDirection();
    }

    public void UnlockYLookDirection()
    {
        _playerLookScript.UnlockYDirection();
    }

    public void UnlockWalkSpeed()
    {
        _playerMovement.UnlockWalkSpeed();
    }

    public void LockWalkSpeedTo(float walkSpeed)
    {
        _playerMovement.LockMovementSpeedTo(walkSpeed);
    }
}