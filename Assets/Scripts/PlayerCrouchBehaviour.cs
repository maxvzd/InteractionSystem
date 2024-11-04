using System;
using Constants;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerCrouchBehaviour : MonoBehaviour
{
    public EventHandler PlayerCrouched;
    public EventHandler PlayerUnCrouched;
    
    [FormerlySerializedAs("crouchFallWindow")] [SerializeField] private float rollFallWindow;
    
    private PlayerGroundedStatus _playerGrounded;
    private bool _isGrounded;
    private Animator _animator;
    private float _shouldRollWhenGrounded;
    private float _rollTimer;
    private float _timeWhenCrouchWasPressed;
    private InputAction _crouchAction;
    private bool _crouchedPerformedDuringFall;
    private bool _isCrouching;
    private CapsuleCollider _playerCollider;

    private const float CROUCH_HEIGHT = 1.35f;
    private const float STANDING_HEIGHT = 1.9f;
    private const float CROUCH_COLLIDER_CENTRE_HEIGHT = 0.7f;
    private const float STANDING_COLLIDER_CENTRE_HEIGHT = 0.95f;

    public const float CROUCH_DISTANCE = 0.4f; 
    
    private void Start()
    {
        _playerGrounded = GetComponent<PlayerGroundedStatus>();
        _animator = GetComponent<Animator>();
        PlayerInput playerInput = GetComponent<PlayerInput>();
        _crouchAction = playerInput.actions[InputConstants.CrouchAction];

        _playerGrounded.IsGrounded += OnPlayerGrounded;
        _playerGrounded.IsNotGrounded += OnPlayerNotGrounded;

        _playerCollider = GetComponent<CapsuleCollider>();
    }

    private void OnPlayerGrounded(object sender, EventArgs args)
    {
        _isGrounded = true;
        if (!_crouchedPerformedDuringFall) return;
        
        float timeBetweenCrouchPressedAndLanding = _rollTimer - _timeWhenCrouchWasPressed;
        if (timeBetweenCrouchPressedAndLanding < rollFallWindow)
        {
            _animator.SetBool(AnimatorConstants.WasCrouchPressed, true);
            _animator.SetTrigger(AnimatorConstants.RollTrigger);
        }
    }

    private void OnPlayerNotGrounded(object sender, EventArgs args)
    {
        _isGrounded = false;
        _rollTimer = 0f;
        _timeWhenCrouchWasPressed = 0f;
        _crouchedPerformedDuringFall = false;
        _animator.SetBool(AnimatorConstants.WasCrouchPressed, false);
    }

    private void Update()
    {
        bool wasCrouchPerformed = _crouchAction.WasPerformedThisFrame();
        
        if (_isGrounded)
        {
            if (wasCrouchPerformed)
            {
                Crouch();
            }
        }
        else
        {
            _rollTimer += Time.deltaTime;
        
            if (wasCrouchPerformed)
            {
                _crouchedPerformedDuringFall = true;
                _timeWhenCrouchWasPressed = _rollTimer;
            }
        }
    }

    private void Crouch()
    {
        _isCrouching = !_isCrouching;
        _animator.SetBool(AnimatorConstants.IsCrouching, _isCrouching);

        float colliderCentre = _isCrouching ? CROUCH_COLLIDER_CENTRE_HEIGHT : STANDING_COLLIDER_CENTRE_HEIGHT;
        _playerCollider.center = new Vector3(0, colliderCentre, 0);
        _playerCollider.height = _isCrouching ? CROUCH_HEIGHT : STANDING_HEIGHT;

        if (_isCrouching)
        {
            PlayerCrouched?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            PlayerUnCrouched?.Invoke(this, EventArgs.Empty);
        }
    }
}