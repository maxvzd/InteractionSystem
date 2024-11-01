using System;
using Constants;
using UnityEngine;

public class FallingManager : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        PlayerGroundedStatus grounded = GetComponent<PlayerGroundedStatus>();
        grounded.IsGrounded += IsGrounded;
        grounded.IsNotGrounded += IsNotGrounded;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
    }

    private void IsNotGrounded(object sender, EventArgs e)
    {
        _animator.SetBool(AnimatorConstants.IsGrounded, false);
        _animator.applyRootMotion = false;
    }

    private void IsGrounded(object sender, EventArgs e)
    {
        _animator.SetBool(AnimatorConstants.IsGrounded, true);
        _animator.applyRootMotion = true;
    }
}