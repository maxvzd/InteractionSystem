using System;
using Constants;
using Kinemation.MotionWarping.Runtime.Core;
using Kinemation.MotionWarping.Runtime.Examples;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClimbing : MonoBehaviour
{
    private MotionWarping _motionWarping;
    private MantleComponent _mantle;
    private VaultComponent _vault;

    private GrounderFBBIK _grounder;

    private LookAtIK _lookAtIk;
    private float _previousBodyWeight;

    private Collider _collider;
    
    private PlayerInput _playerInput;
    private InputAction _jumpAction;

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
        _collider = GetComponent<Collider>();

        _grounder = GetComponent<GrounderFBBIK>();
        _lookAtIk = GetComponent<LookAtIK>();

        _playerInput = GetComponent<PlayerInput>();
        _jumpAction = _playerInput.actions[InputConstants.JumpAction];
    }

    private void Update()
    {
        if (_motionWarping.IsActive()) return;
        
        if (_jumpAction.WasPressedThisFrame())
        {
            if (_motionWarping.Interact(_vault)) return;
            if (_motionWarping.Interact(_mantle)) return;
        }
    }
}