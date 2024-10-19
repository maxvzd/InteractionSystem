using System.Collections;
using UnityEngine;

namespace PlayerAiming
{
    public class DeadZoneLook : MonoBehaviour
    {
        [SerializeField] private Transform lookAtBase;
        [SerializeField] private Transform aimAtBase;

        [SerializeField] private float maxGunAimAngle;
        [SerializeField] private float sensitivity;
        [SerializeField] private float maxVerticalAngle;
        private IEnumerator _lerpAimToLookCoRoutine;

        private bool _lockYRotation;
    
        public bool UseDeadZone { get; set; } = false;

        public void LerpAimToLook()
        {
            if (_lerpAimToLookCoRoutine is not null)
            {
                StopCoroutine(_lerpAimToLookCoRoutine);
            }

            _lerpAimToLookCoRoutine = LerpAimToLookCoroutine();
            StartCoroutine(_lerpAimToLookCoRoutine);
        }

        private IEnumerator LerpAimToLookCoroutine()
        {
            float timeElapsed = 0f;
            float lerpTime = 0.1f;

            Vector3 originalAimRotation = aimAtBase.eulerAngles;
            while (timeElapsed < lerpTime)
            {
                float t = timeElapsed / lerpTime;

                aimAtBase.eulerAngles = Vector3.Lerp(originalAimRotation, lookAtBase.eulerAngles, t);

                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;
            }

            aimAtBase.eulerAngles = lookAtBase.eulerAngles;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    
        public void UnlockYDirection()
        {
            _lockYRotation = false;
        }

        public void LockYDirection()
        {
            _lockYRotation = true;
        }

        private void Update()
        {
            float rotationX = 0f;
        
            if (!_lockYRotation)
            {
                rotationX = Input.GetAxis(Constants.InputConstants.MouseY) * sensitivity;
            }
        
            float rotationY = Input.GetAxis("Mouse X") * sensitivity;

            Vector3 aimRotation = aimAtBase.eulerAngles + new Vector3(-rotationX, rotationY, 0);
            Vector3 lookRotation = lookAtBase.eulerAngles;

            if (UseDeadZone)
            {
                lookRotation += CalculateDeadZone(aimRotation.y, lookRotation.y, Vector3.up) * rotationY;
                lookRotation += CalculateDeadZone(aimRotation.x, lookRotation.x, Vector3.right) * -rotationX;
            }
            else
            {
                lookRotation += new Vector3(-rotationX, rotationY, 0);
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
            eulerAngleToClamp = GetRealAngle(eulerAngleToClamp);
            return Mathf.Clamp(eulerAngleToClamp, -angleToClampTo, angleToClampTo);
        }

        private static float GetRealAngle(float angle)
        {
            return angle > 180 ? angle - 360 : angle;
        }
    }
}
