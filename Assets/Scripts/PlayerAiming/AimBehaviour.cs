using System;
using System.Collections;
using Constants;
using GunStuff;
using Items.ItemInterfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PlayerAiming
{
    public class AimBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private UnityEvent playerAiming;
        [SerializeField] private UnityEvent playerNotAiming;
        
        private Transform _rearSight;
        private Transform _gunFulcrum;

        private IEnumerator _weaponPositionLerper;
        private float _originalFOV;

        private Transform _originalParent;
        private Vector3 _originalCameraPosition;
        private Vector3 _originalGunPosition;
        private Vector3 _aimOffset;
        private bool _gunIsEquipped;
        private PlayerInput _playerInput;
        private InputAction _aimAction;
        
        private void Start()
        {
            _originalFOV = mainCamera.fieldOfView;

            Transform cameraTransform = mainCamera.transform;
            _originalParent = cameraTransform.parent;
            _originalCameraPosition = cameraTransform.localPosition;
            
            _playerInput = GetComponent<PlayerInput>();
            _aimAction = _playerInput.actions[InputConstants.AimAction];
        }

        public void EquipGun(GunPositionData posData, IEquippable weaponInfo)
        {
            _gunIsEquipped = true;
            _gunFulcrum = posData.GunFulcrum;
            _rearSight = posData.RearSight;

            _originalGunPosition = weaponInfo.EquippedPosition.EquippedLocalPosition;
            _aimOffset = posData.AimPosition;
        }

        public void UnEquipGun()
        {
            _gunIsEquipped = false;
            _gunFulcrum = null;
            _rearSight = null;

            _originalGunPosition = Vector3.zero;
            _aimOffset = Vector3.zero;
        }

        private void Update()
        {
            if (!_gunIsEquipped) return;

            Transform cameraTransform = mainCamera.transform;
            if (_aimAction.WasPressedThisFrame())
            {
                Vector3 targetGunPos = _originalGunPosition + _aimOffset;
                
                cameraTransform.parent = _rearSight;
                StartAimLerp(
                    cameraTransform,
                    Vector3.zero,
                    _gunFulcrum,
                    targetGunPos,
                    40,
                    0.2f);

                playerAiming.Invoke();
            }

            if (_aimAction.WasReleasedThisFrame())
            {
                cameraTransform.parent = _originalParent;
                StartAimLerp(cameraTransform,
                    _originalCameraPosition,
                    _gunFulcrum,
                    _originalGunPosition,
                    _originalFOV,
                    0.2f);

                playerNotAiming.Invoke();
            }

            Quaternion lookAtRotation = Quaternion.LookRotation(_gunFulcrum.position - aimTarget.position);
            _gunFulcrum.rotation = lookAtRotation;

            Quaternion lookAtRotationSight = Quaternion.LookRotation(_rearSight.position - aimTarget.position);
            _rearSight.rotation = lookAtRotationSight;
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