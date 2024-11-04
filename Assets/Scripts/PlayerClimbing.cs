using System;
using Constants;
using Kinemation.MotionWarping.Runtime.Core;
using Kinemation.MotionWarping.Runtime.Examples;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClimbing : MonoBehaviour
{
    [SerializeField] private float playerSpeedJumpTolerance;
    [SerializeField] private float playerJumpHeight;
    //[SerializeField] private AnimationCurve playerSpeedJumpModifier;  

    private MotionWarping _motionWarping;
    private MantleComponent _mantle;
    private VaultComponent _vault;
    private GrounderFBBIK _grounder;
    private LookAtIK _lookAtIk;
    private float _previousBodyWeight;
    private CapsuleCollider _collider;
    private PlayerInput _playerInput;
    private InputAction _jumpAction;
    private bool _isGrounded;
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private AnimationEventListener _animationEventListener;
    private Rigidbody _rigidBody;

    public void OnWarpStart()
    {
        _collider.enabled = false;
        _grounder.weight = 0;
        _previousBodyWeight = _lookAtIk.solver.bodyWeight;
        _lookAtIk.solver.bodyWeight = 0;
    }

    public void OnWarpEnd()
    {
        _collider.enabled = true;
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        _grounder.weight = 1;
        _lookAtIk.solver.bodyWeight = _previousBodyWeight;
    }

    private void Start()
    {
        _motionWarping = GetComponent<MotionWarping>();
        _mantle = GetComponent<MantleComponent>();
        _vault = GetComponent<VaultComponent>();
        _collider = GetComponent<CapsuleCollider>();

        _grounder = GetComponent<GrounderFBBIK>();
        _lookAtIk = GetComponent<LookAtIK>();

        _playerInput = GetComponent<PlayerInput>();
        _jumpAction = _playerInput.actions[InputConstants.JumpAction];

        PlayerGroundedStatus grounded = GetComponent<PlayerGroundedStatus>();
        grounded.IsGrounded += (sender, args) => { _isGrounded = true; };
        grounded.IsNotGrounded += (sender, args) => { _isGrounded = false; };

        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();

        _animationEventListener = GetComponent<AnimationEventListener>();
        _animationEventListener.JumpPeaked += OnJumpPeak;

        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnJumpPeak(object sender, EventArgs e)
    {
        Transform currentTransform = transform;
        Vector3 up = currentTransform.up;
        Vector3 forward = currentTransform.forward;
        Vector3 characterRootPosition = currentTransform.position;
        float distanceToCheck = 3f;

        //Cast ray forward and below to check for ground where character would land
        bool isJumpEndGrounded = Physics.Raycast(
            characterRootPosition,
            -up + forward,
            distanceToCheck,
            LayerMask.GetMask(LayerConstants.LAYER_TERRAIN));
        //Debug.DrawRay(characterRootPosition, (-up+ forward * distanceToCheck), Color.green, 1f);

        _animator.SetBool(AnimatorConstants.IsJumpLocationGrounded, isJumpEndGrounded);
    }

    private void Update()
    {
        if (_motionWarping.IsActive() || !_isGrounded) return;

        if (!_jumpAction.WasPressedThisFrame()) return;
        if (_motionWarping.Interact(_vault)) return;
        if (_motionWarping.Interact(_mantle)) return;


        //check for anything infront for x units

        if (_playerMovement.CurrentSpeed.y < playerSpeedJumpTolerance) return;
        //Necessary? Maybe causes the player to ragdoll if it hits something (puppetmaster)
        if (SpaceInFrontOfPlayerIsObstructed(2.5f)) return;
        Jump();
    }

    private void Jump()
    {
        _animator.applyRootMotion = false;
        _animator.SetTrigger(AnimatorConstants.JumpTrigger);
        _rigidBody.AddForce(transform.up * playerJumpHeight, ForceMode.Impulse);
    }

    private bool SpaceInFrontOfPlayerIsObstructed(float distance)
    {
        Transform currentTransform = transform;
        Vector3 forwardDir = currentTransform.forward;
        Vector3 currentPosition = currentTransform.position;
        return Physics.Raycast(currentPosition + currentTransform.up * _collider.height, forwardDir, distance, LayerMask.NameToLayer(LayerConstants.LAYER_TERRAIN));
    }
}