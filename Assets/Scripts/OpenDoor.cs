using System;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private HingeJoint _hingeJoint;

    private readonly JointLimits _shutDoorLimit = new()
    {
        min = -0.1f,
        max = 0f,
    };

    private readonly JointLimits _openDoorLimit = new()
    {
        min = -90f,
        max = 0f,
    };

    private void Start()
    {
        _hingeJoint = GetComponent<HingeJoint>();
    }

    private void Update()
    {
        if (Input.GetButtonDown(Constants.Fire1))
        {
            _hingeJoint.limits = _openDoorLimit;
        }

        if (Input.GetButton(Constants.Fire1))
        {
            float mouseVerticalMovement = Input.GetAxis(Constants.MouseY);

            JointMotor hingeMotor = _hingeJoint.motor;
            hingeMotor.targetVelocity = -90 * mouseVerticalMovement;
            hingeMotor.freeSpin = false;
            hingeMotor.force = 100;

            _hingeJoint.motor = hingeMotor;
            _hingeJoint.useMotor = true;
        }

        if (Input.GetButtonUp(Constants.Fire1))
        {
            //_hingeJoint.limits = _shutDoorLimit;
        }
    }
}