using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerAiming
{
    public class DeadZoneLook : MonoBehaviour
    {
        public bool UseDeadZone { get; set; }
        
        [SerializeField] private Transform lookAtBase;
        [SerializeField] private Transform aimAtBase;
        [SerializeField] private float maxGunAimAngle;
        [SerializeField] private float sensitivity;
        [SerializeField] private float maxVerticalAngle;
        [SerializeField] private PlayerInput playerInput;
        
        private IEnumerator _lerpAimToLookCoRoutine;
        private bool _isVerticalRotationLocked;
        private InputAction _lookAction;
        private bool _isCameraLocked;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _lookAction = playerInput.actions[InputConstants.LookAction];
        }
        
        private void Update()
        {
            if (_isCameraLocked) return;
            
            Vector2 mouseDelta = _lookAction.ReadValue<Vector2>() * sensitivity;
            float verticalRotation = -mouseDelta.y;
            float horizontalRotation = mouseDelta.x;
            
            if (_isVerticalRotationLocked)
            {
                verticalRotation = 0f;
            }
            
            Vector3 aimRotation = aimAtBase.eulerAngles + new Vector3(verticalRotation, horizontalRotation, 0);
            Vector3 lookRotation = lookAtBase.eulerAngles;

            if (UseDeadZone)
            {
                lookRotation += CalculateDeadZone(aimRotation.x, lookRotation.x, Vector3.right) * verticalRotation;
                lookRotation += CalculateDeadZone(aimRotation.y, lookRotation.y, Vector3.up) * horizontalRotation;
            }
            else
            {
                lookRotation += new Vector3(verticalRotation, horizontalRotation, 0);
            }

            aimRotation.x = ClampEulerAngle(aimRotation.x, maxVerticalAngle);
            lookRotation.x = ClampEulerAngle(lookRotation.x, maxVerticalAngle);

            lookAtBase.eulerAngles = lookRotation;
            aimAtBase.eulerAngles = aimRotation;
        }

        private Vector3 CalculateDeadZone(float aimRotation, float lookRotation, Vector3 direction)
        {
            Vector3 offset = Vector3.zero;

            if (Mathf.Abs(aimRotation - lookRotation) > maxGunAimAngle)
            {
                offset = direction;
            }
            return offset;
        }

        private static float ClampEulerAngle(float eulerAngleToClamp, float angleToClampTo)
        {
            eulerAngleToClamp = AngleHelper.GetRealAngle(eulerAngleToClamp);
            return Mathf.Clamp(eulerAngleToClamp, -angleToClampTo, angleToClampTo);
        }

        public void OnPlayerAiming()
        {
            LerpAimToLook();
            UseDeadZone = false;
        }

        public void OnPlayerNotAiming()
        {
            UseDeadZone = true;
        }
        
        private void LerpAimToLook()
        {
            if (_lerpAimToLookCoRoutine is not null)
            {
                StopCoroutine(_lerpAimToLookCoRoutine);
            }

            IEnumerable<ILerpComponent> lerpComponents = new List<ILerpComponent>
            {
                new LerpQuaternion(lookAtBase.rotation, aimAtBase)
            };

            _lerpAimToLookCoRoutine = Lerper.Lerp(lerpComponents, 0.1f);
            StartCoroutine(_lerpAimToLookCoRoutine);
        }
        
        public void UnlockYDirection()
        {
            _isVerticalRotationLocked = false;
        }

        public void LockYDirection()
        {
            _isVerticalRotationLocked = true;
        }
    }
}
