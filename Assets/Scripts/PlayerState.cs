using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private PlayerLookScript _playerLookScript;

    private void Start()
    {
        _playerLookScript = GetComponent<PlayerLookScript>();
    }

    public void LockYLookDirection()
    {
        _playerLookScript.LockYDirection();
    }

    public void UnlockYLookDirection()
    {
        _playerLookScript.UnlockYDirection();
    }
}