using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private Transform frontHandle;
    [SerializeField] private Transform backHandle;

    public Vector3 FrontHandlePosition => frontHandle.position;
    public Vector3 BackHandlePosition => backHandle.position;
    
    private HingeJoint _hingeJoint;
    
    public bool PlayerIsInteractingFromFront;
    public bool PlayerIsInteractingFromBack;

    private readonly JointLimits _shutDoorLimit = new()
    {
        min = 0.0f,
        max = 0f,
    };

    private readonly JointLimits _openDoorLimit = new()
    {
        min = 90f,
        max = 0f,
    };

    private void Start()
    {
        _hingeJoint = GetComponent<HingeJoint>();
        _hingeJoint.limits = _shutDoorLimit;
    }

    private void Update()
    {
        if (_hingeJoint.angle < 1f && !PlayerIsInteractingFromFront && !PlayerIsInteractingFromBack)
        {
            _hingeJoint.limits = _shutDoorLimit;
        }
        
        if (!PlayerIsInteractingFromFront && !PlayerIsInteractingFromBack) return;

        float directionModifier = 1f;
        if (PlayerIsInteractingFromBack)
        {
            directionModifier *= -1f;
        }


        if (Input.GetButtonDown(Constants.Fire1))
        {
            _hingeJoint.limits = _openDoorLimit;
        }

        if (Input.GetButton(Constants.Fire1))
        {
            float mouseVerticalMovement = -Input.GetAxis(Constants.MouseY);

            JointMotor hingeMotor = _hingeJoint.motor;
            hingeMotor.targetVelocity = -90 * mouseVerticalMovement * directionModifier;
            hingeMotor.freeSpin = false;
            hingeMotor.force = 100;

            _hingeJoint.motor = hingeMotor;
            _hingeJoint.useMotor = true;
        }

        if (Input.GetButtonUp(Constants.Fire1))
        {
            if (_hingeJoint.angle < 1f)
            {
                _hingeJoint.limits = _shutDoorLimit;
            }
        }
    }
}