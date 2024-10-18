using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _runModifier;

    private Animator _animator;
    private bool _shouldLockWalkSpeed;
    private float _lockedWalkSpeedModifier;
    private float _maxMovementSpeed;

    private const float MAX_MOVEMENT_SPEED = 2f;
    private const float MIN_MOVEMENT_SPEED = 0.5f;

    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _maxMovementSpeed = MAX_MOVEMENT_SPEED;
    }

    // Update is called once per frame
    private void Update()
    {
        float verticalInput = Input.GetAxis(Constants.VerticalKey);
        float horizontalInput = Input.GetAxis(Constants.HorizontalKey);

        _runModifier += Input.GetAxis("Mouse ScrollWheel");
        _runModifier = Mathf.Clamp(_runModifier, MIN_MOVEMENT_SPEED, _maxMovementSpeed);

        verticalInput *= _runModifier;
        horizontalInput *= _runModifier;

        _animator.SetFloat(Constants.Vertical, verticalInput);
        _animator.SetFloat(Constants.Horizontal, horizontalInput);
    }

    public void UnlockWalkSpeed()
    {
        _maxMovementSpeed = MAX_MOVEMENT_SPEED;
    }

    public void ClampMovementSpeedTo(float walkSpeed)
    {
        float maxMovementSpeed = Mathf.Clamp(walkSpeed, MIN_MOVEMENT_SPEED, MAX_MOVEMENT_SPEED);
        _maxMovementSpeed = maxMovementSpeed;
    }
}