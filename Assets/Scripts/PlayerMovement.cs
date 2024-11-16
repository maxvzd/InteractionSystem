using System;
using Constants;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float _runModifier;

    private Animator _animator;
    private bool _shouldLockWalkSpeed;
    private float _lockedWalkSpeedModifier;
    private float _maxMovementSpeed;
    private float _runModifierBeforeLock;

    private const float MAX_MOVEMENT_SPEED = 2f;
    private const float MIN_MOVEMENT_SPEED = 0.5f;

    #region Input
    
    private PlayerInput _playerInput;
    private InputAction _movementAction;
    private InputAction _scrollAction;
    
    #endregion Input
    
    private Vector2 _smoothedInput;
    private Vector2 _smoothInputVelocity;
    public Vector2 CurrentSpeed => _smoothedInput * _runModifier;
    public bool IsMovingVertically => _isMovingVertically;
    private bool _isMovingVertically;
    private bool IsMovingHorizontally => _isMovingHorizontally;
    private bool _isMovingHorizontally;
    public bool IsMoving => IsMovingHorizontally || IsMovingVertically;


    [SerializeField] private float playerAccelerationTime;
    
    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _maxMovementSpeed = MAX_MOVEMENT_SPEED;
        
        _playerInput = GetComponent<PlayerInput>();
        _movementAction = _playerInput.actions[InputConstants.MoveAction];
        _scrollAction = _playerInput.actions[InputConstants.SpeedModifierAction];
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 input = _movementAction.ReadValue<Vector2>();
        _smoothedInput = Vector2.SmoothDamp(_smoothedInput, input, ref _smoothInputVelocity, playerAccelerationTime);
        
        _isMovingHorizontally = _smoothedInput.x is > .1f or < -.1f;
        _isMovingVertically = _smoothedInput.y is > .1f or < -.1f;
        
        bool isHorizontalInputDown = input.x is > 0 or < 0;
        bool isVerticalInputDown = input.y is > 0 or < 0;

        if (!_isMovingHorizontally && !isHorizontalInputDown)
        {
            _smoothedInput.x = 0f;
        }
        
        if (!_isMovingVertically && !isVerticalInputDown)
        {
            _smoothedInput.y = 0f;
        }
        
        _runModifier +=  _scrollAction.ReadValue<float>();
        _runModifier = Mathf.Clamp(_runModifier, MIN_MOVEMENT_SPEED, _maxMovementSpeed);
        
        Vector2 modifiedInput = CurrentSpeed;
       
        _animator.SetFloat(AnimatorConstants.Vertical, modifiedInput.y);
        _animator.SetFloat(AnimatorConstants.Horizontal, modifiedInput.x);
    }

    public void UnlockWalkSpeed()
    {
        _maxMovementSpeed = MAX_MOVEMENT_SPEED;
        _runModifier = _runModifierBeforeLock;
    }

    public void ClampMovementSpeedTo(float walkSpeed)
    {
        _runModifierBeforeLock = _runModifier;
        float maxMovementSpeed = Mathf.Clamp(walkSpeed, MIN_MOVEMENT_SPEED, MAX_MOVEMENT_SPEED);
        _maxMovementSpeed = maxMovementSpeed;
    }
}