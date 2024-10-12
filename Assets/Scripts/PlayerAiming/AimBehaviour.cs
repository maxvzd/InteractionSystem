using System;
using System.Collections;
using UnityEngine;

namespace PlayerAiming
{
    public class AimBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform rearSightTransform;
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform gunFulcrum;

        private IEnumerator _weaponPositionLerper;
        private float _originalFOV;

        private Transform _originalParent;
        private Vector3 _originalCameraPosition;
        private Vector3 _originalGunPosition;
        private Vector3 _targetGunPos;

        public event EventHandler<EventArgs> PlayerAiming;
        public event EventHandler<EventArgs> PlayerNotAiming;

        private void Start()
        {
            _originalFOV = mainCamera.fieldOfView;

            Transform cameraTransform = mainCamera.transform;
            _originalParent = cameraTransform.parent;
            _originalCameraPosition = cameraTransform.localPosition;

            Transform gunTransform = gunFulcrum.transform;
            _originalGunPosition = gunTransform.localPosition;
            
            //Not sure why this is but should be _originalGunPosition + gunTransform.up * 0.1f
            //Some sort of weird rotation thing?
            var currentTransform = transform;
            _targetGunPos = _originalGunPosition 
                            - currentTransform.right * 0.1f 
                            - currentTransform.up * 0.05f 
                            - currentTransform.forward * 0.05f;
        }

        private void Update()
        {
            Transform cameraTransform = mainCamera.transform;
            if (Input.GetButtonDown(Constants.Fire2Key))
            {
                
                
                cameraTransform.parent = rearSightTransform;
                StartAimLerp(
                    cameraTransform, 
                    Vector3.zero, 
                    gunFulcrum,
                    _targetGunPos,
                    40, 
                    0.2f);
                
                PlayerAiming?.Invoke(this, EventArgs.Empty);
            }

            if (Input.GetButtonUp(Constants.Fire2Key))
            {
                cameraTransform.parent = _originalParent;
                StartAimLerp(cameraTransform, 
                    _originalCameraPosition, 
                    gunFulcrum,
                    _originalGunPosition, 
                    _originalFOV,
                    0.2f);
                
                PlayerNotAiming?.Invoke(this, EventArgs.Empty);
            }

            Quaternion lookAtRotation = Quaternion.LookRotation(gunFulcrum.position - aimTarget.position);
            gunFulcrum.rotation = lookAtRotation;

            Quaternion lookAtRotationSight = Quaternion.LookRotation(rearSightTransform.position - aimTarget.position);
            rearSightTransform.rotation = lookAtRotationSight;
        }

        private void StartAimLerp(
            Transform cameraTransform, 
            Vector3 cameraPositionToLerpTo,
            Transform gunTransform,
            Vector3 gunPositionToLerpTo,
            float fovToLerpTo, 
            float lerpTime)
        {
            if (!ReferenceEquals(_weaponPositionLerper, null))
            {
                StopCoroutine(_weaponPositionLerper);
            }
        
            _weaponPositionLerper = LerpTransformToPoint(cameraTransform, cameraPositionToLerpTo, gunTransform, gunPositionToLerpTo, fovToLerpTo, lerpTime);
            StartCoroutine(_weaponPositionLerper);
        }

        private IEnumerator LerpTransformToPoint(
            Transform cameraTransform, 
            Vector3 cameraPositionToLerpTo,
            Transform gunTransform,
            Vector3 gunPositionToLerpTo,
            float fovToLerpTo, 
            float lerpTime)
        {
            Vector3 fromCameraPosition = cameraTransform.localPosition;
            Vector3 fromGunPosition = gunTransform.localPosition;
            float fromFov = mainCamera.fieldOfView;
            float elapsedTime = 0f;
        
            while (elapsedTime < lerpTime)
            {
                float t = elapsedTime / lerpTime;
                elapsedTime += Time.deltaTime;
        
                cameraTransform.localPosition = Vector3.Lerp(fromCameraPosition, cameraPositionToLerpTo, t);
                gunTransform.localPosition = Vector3.Lerp(fromGunPosition, gunPositionToLerpTo, t);
                mainCamera.fieldOfView = Mathf.Lerp(fromFov, fovToLerpTo, t);
        
                yield return new WaitForEndOfFrame();
            }
        
            cameraTransform.localPosition = cameraPositionToLerpTo;
            gunTransform.localPosition = gunPositionToLerpTo;
            mainCamera.fieldOfView = fovToLerpTo;
        }
    }
}