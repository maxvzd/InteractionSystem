using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private Transform frontHandle;
    [SerializeField] private Transform backHandle;

    public Vector3 FrontHandlePosition => frontHandle.position;
    public Vector3 BackHandlePosition => backHandle.position;

    private HingeJoint _hingeJoint;

    private bool _playerIsInteractingFromFront;
    private bool _playerIsInteractingFromBack;
    private float _targetVelocity;
    private bool _isDoorShut = true;

    public bool PlayerIsInteractingFromFront => _playerIsInteractingFromFront;
    public bool PlayerIsInteractingFromBack => _playerIsInteractingFromBack;

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

    public void InteractWithDoor(bool isFromFront)
    {
        _playerIsInteractingFromFront = isFromFront;
        _playerIsInteractingFromBack = !isFromFront;
    }

    public void EndInteraction()
    {
        _playerIsInteractingFromFront = false;
        _playerIsInteractingFromBack = false;
    }

    public void ChangeHingeLimitsToOpen()
    {
        if (!_isDoorShut) return;

        _hingeJoint.limits = _openDoorLimit;
        _isDoorShut = false;
    }

    public void ChangeHingeLimitsToClosed()
    {
        if (_isDoorShut) return;
        if (_hingeJoint.angle >= 1f) return;
        
        _isDoorShut = true;
        _hingeJoint.limits = _shutDoorLimit;
    }

    public void SetDoorVelocity(float velocity)
    {
        float directionModifier = 1f;
        if (_playerIsInteractingFromBack)
        {
            directionModifier *= -1f;
        }

        _targetVelocity = 90 * velocity * directionModifier;
    }

    public void SetDoorVelocityToZero()
    {
        _targetVelocity = 0f;
    }
    
    private void Start()
    {
        _hingeJoint = GetComponent<HingeJoint>();
        _hingeJoint.limits = _shutDoorLimit;
    }

    private void Update()
    {
        if (!_playerIsInteractingFromFront && !_playerIsInteractingFromBack)
        {
            ChangeHingeLimitsToClosed();
        }

        if (!_playerIsInteractingFromFront && !_playerIsInteractingFromBack) return;

        JointMotor hingeMotor = _hingeJoint.motor;
        _hingeJoint.useMotor = true;
        hingeMotor.targetVelocity = 0f;
        hingeMotor.freeSpin = false;
        hingeMotor.force = 100;
        hingeMotor.targetVelocity = _targetVelocity;
        _hingeJoint.motor = hingeMotor;
    }
}