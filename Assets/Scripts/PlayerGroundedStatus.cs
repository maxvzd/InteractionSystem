using System;
using Constants;
using UnityEngine;

public class PlayerGroundedStatus : MonoBehaviour
{
    public EventHandler IsGrounded;
    public EventHandler IsNotGrounded;

    [SerializeField] private float boxCastPenetrationHeight;
    private float _playerRadius;
    private bool _isGrounded;
    private bool _lastFrameIsGrounded;

    private void Start()
    {
        Collider playerCollider = GetComponent<Collider>();
        _playerRadius = playerCollider.bounds.extents.x;
    }
    
    private void FixedUpdate()
    {
        float boxHeight = 0.05f;
        Transform currentTransform = transform;
        Vector3 playerPosition = currentTransform.position;
        Vector3 centreOfGroundCast = new Vector3(playerPosition.x, playerPosition.y - boxHeight * 0.5f, playerPosition.z);
        Vector3 groundCastHalfExtent = new Vector3(_playerRadius, boxHeight, _playerRadius);

        //Using box cast all because BoxCast seems to not work for some reason
        RaycastHit[] floorHit = Physics.BoxCastAll(centreOfGroundCast, groundCastHalfExtent, -currentTransform.up, currentTransform.rotation, boxHeight, LayerMask.GetMask(LayerConstants.LAYER_TERRAIN));
        
        _isGrounded = floorHit.Length > 0;

        switch (_isGrounded)
        {
            case true when !_lastFrameIsGrounded:
                IsGrounded?.Invoke(this, EventArgs.Empty);
                break;
            case false when _lastFrameIsGrounded:
                IsNotGrounded?.Invoke(this, EventArgs.Empty);
                break;
        }

        _lastFrameIsGrounded = _isGrounded;
    }
}