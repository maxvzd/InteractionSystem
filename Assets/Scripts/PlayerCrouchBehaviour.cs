using System;
using Constants;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerCrouchBehaviour : MonoBehaviour
{
    [FormerlySerializedAs("crouchFallWindow")] [SerializeField] private float rollFallWindow;
    
    private PlayerGroundedStatus _playerGrounded;
    private bool _isGrounded;
    private Animator _animator;

    private float _shouldRollWhenGrounded;
    private float _rollTimer;
    private float _timeWhenCrouchWasPressed;
    private InputAction _crouchAction;
    private bool _crouchedPerformedDuringFall;

    private void Start()
    {
        _playerGrounded = GetComponent<PlayerGroundedStatus>();
        _animator = GetComponent<Animator>();
        PlayerInput playerInput = GetComponent<PlayerInput>();
        _crouchAction = playerInput.actions[InputConstants.CrouchAction];

        _playerGrounded.IsGrounded += OnPlayerGrounded;
        _playerGrounded.IsNotGrounded += OnPlayerNotGrounded;
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
        if (_isGrounded) return;
        
        bool crouchPerformed = _crouchAction.WasPerformedThisFrame();
        _rollTimer += Time.deltaTime;
        
        if (crouchPerformed)
        {
            _crouchedPerformedDuringFall = true;
            _timeWhenCrouchWasPressed = _rollTimer;
        }
    }
}