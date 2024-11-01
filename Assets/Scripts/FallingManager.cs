using System;
using Constants;
using UnityEngine;

public class FallingManager : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidBody;
    private bool _isGrounded;
    private float _amountOfTimeNotGrounded;

    private void Start()
    {
        PlayerGroundedStatus grounded = GetComponent<PlayerGroundedStatus>();
        grounded.IsGrounded += IsGrounded;
        grounded.IsNotGrounded += IsNotGrounded;
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_isGrounded)
        {
            _amountOfTimeNotGrounded += Time.deltaTime * -_rigidBody.linearVelocity.y;
        }
    }

    private void IsNotGrounded(object sender, EventArgs e)
    {
        _animator.SetBool(AnimatorConstants.IsGrounded, false);
        _animator.applyRootMotion = false;
        _isGrounded = false;
        
        _amountOfTimeNotGrounded = 0f;
    }

    private void IsGrounded(object sender, EventArgs e)
    {
        _animator.SetBool(AnimatorConstants.IsGrounded, true);
        _animator.applyRootMotion = true;
        _isGrounded = true;

        _animator.SetFloat(AnimatorConstants.FallIntensity, _amountOfTimeNotGrounded);
    }
}