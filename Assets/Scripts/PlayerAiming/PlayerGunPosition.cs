using System;
using System.Collections;
using Constants;
using GunStuff;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace PlayerAiming
{
    public class PlayerGunPosition : MonoBehaviour
    {
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private UnityEvent playerAiming;
        [SerializeField] private UnityEvent playerNotAiming;
        [SerializeField] private float distanceFromRearSight;
        [SerializeField] private float recoverySpeed;
        
        private IEnumerator _aimLerper;
        private IEnumerator _weaponPositionLerper;
        private float _originalFOV;
        private Transform _originalParent;
        private Vector3 _originalCameraPosition;
        private Vector3 _originalGunPosition;
        private bool _gunIsEquipped;
        private PlayerInput _playerInput;
        private InputAction _aimAction;
        private GunPositionData _currentlyEquippedGunPositionData;
        private Vector3 _currentCrouchOffset;
        private Vector3 _currentAimOffset;

        private void Start()
        {
            _originalFOV = mainCamera.fieldOfView;

            Transform cameraTransform = mainCamera.transform;
            _originalParent = cameraTransform.parent;
            _originalCameraPosition = cameraTransform.localPosition;

            _playerInput = GetComponent<PlayerInput>();
            _aimAction = _playerInput.actions[InputConstants.AimAction];
            PlayerCrouchBehaviour crouchBehaviour = GetComponent<PlayerCrouchBehaviour>();
            crouchBehaviour.PlayerUnCrouched += OnPlayerUnCrouched;
            crouchBehaviour.PlayerCrouched += OnPlayerCrouched;
        }

        private void OnPlayerCrouched(object sender, EventArgs e)
        {
            if (!_gunIsEquipped) return;

            _currentCrouchOffset = -Vector3.up * PlayerCrouchBehaviour.CROUCH_DISTANCE;
            StartGunLerp(_currentlyEquippedGunPositionData.GunFulcrum, _originalGunPosition + _currentCrouchOffset, 0.2f);
        }

        private void OnPlayerUnCrouched(object sender, EventArgs e)
        {
            if (!_gunIsEquipped) return;

            _currentCrouchOffset = Vector3.zero;
            StartGunLerp(_currentlyEquippedGunPositionData.GunFulcrum, _originalGunPosition, 0.2f);
        }

        public void EquipGun(GunPositionData posData)
        {
            _gunIsEquipped = true;
            _originalGunPosition = posData.GunFulcrum.localPosition;
            _currentlyEquippedGunPositionData = posData;
        }

        public void UnEquipGun()
        {
            _gunIsEquipped = false;

            _currentlyEquippedGunPositionData = null;
            _originalGunPosition = Vector3.zero;
        }

        private void Update()
        {
            if (!_gunIsEquipped) return;

            Transform cameraTransform = mainCamera.transform;
            Transform fulcrum = _currentlyEquippedGunPositionData.GunFulcrum;
            Transform rearSight = _currentlyEquippedGunPositionData.RearSight;
            Transform gunMesh = _currentlyEquippedGunPositionData.GunMesh;
            
            if (_aimAction.WasPressedThisFrame())
            {
                _currentAimOffset = _currentlyEquippedGunPositionData.AimPosition;
                cameraTransform.parent = rearSight;
                StartCameraLerp(
                    cameraTransform,
                    new Vector3(0, 0, distanceFromRearSight),
                    60f,
                    0.2f);

                playerAiming.Invoke();
            }

            if (_aimAction.WasReleasedThisFrame())
            {
                 cameraTransform.parent = _originalParent;
                 _currentAimOffset = Vector3.zero;
                StartCameraLerp(cameraTransform,
                    _originalCameraPosition,
                    _originalFOV,
                    0.2f);

                playerNotAiming.Invoke();
            }

            fulcrum.localPosition = Vector3.MoveTowards(
                fulcrum.localPosition,
                _originalGunPosition + _currentCrouchOffset + _currentAimOffset,
                recoverySpeed * Time.deltaTime);

            gunMesh.localRotation = Quaternion.RotateTowards(
                gunMesh.localRotation, 
                Quaternion.identity, 
                recoverySpeed * (180f / Mathf.PI) * Time.deltaTime);
            
            Quaternion lookAtRotation = Quaternion.LookRotation(fulcrum.position - aimTarget.position);
            fulcrum.rotation = lookAtRotation;
        }

        private void StartCameraLerp(
            Transform cameraTransform,
            Vector3 cameraPositionToLerpTo,
            float fovToLerpTo,
            float lerpTime)
        {
            if (!ReferenceEquals(_aimLerper, null))
            {
                StopCoroutine(_aimLerper);
            }

            _aimLerper = LerpCamera(cameraTransform, cameraPositionToLerpTo, fovToLerpTo, lerpTime);
            StartCoroutine(_aimLerper);
        }

        private void StartGunLerp(
            Transform gunTransform,
            Vector3 gunPosToLerpTo,
            float lerpTime)
        {
            if (!ReferenceEquals(_weaponPositionLerper, null))
            {
                StopCoroutine(_weaponPositionLerper);
            }

            _weaponPositionLerper = LerpGun(gunTransform, gunPosToLerpTo, lerpTime);
            StartCoroutine(_weaponPositionLerper);
        }

        private IEnumerator LerpCamera(
            Transform cameraTransform,
            Vector3 cameraPositionToLerpTo,
            float fovToLerpTo,
            float lerpTime)
        {
            Vector3 fromCameraPosition = cameraTransform.localPosition;
            float fromFov = mainCamera.fieldOfView;
            float elapsedTime = 0f;

            while (elapsedTime < lerpTime)
            {
                float t = elapsedTime / lerpTime;
                elapsedTime += Time.deltaTime;

                cameraTransform.localPosition = Vector3.Lerp(fromCameraPosition, cameraPositionToLerpTo, t);
                mainCamera.fieldOfView = Mathf.Lerp(fromFov, fovToLerpTo, t);

                yield return new WaitForEndOfFrame();
            }

            cameraTransform.localPosition = cameraPositionToLerpTo;
            mainCamera.fieldOfView = fovToLerpTo;
        }

        private IEnumerator LerpGun(
            Transform gunTransform,
            Vector3 posToLerpTo,
            float lerpTime)
        {
            Vector3 currentPosition = gunTransform.localPosition;

            float elapsedTime = 0f;

            while (elapsedTime < lerpTime)
            {
                float t = elapsedTime / lerpTime;
                elapsedTime += Time.deltaTime;

                gunTransform.localPosition = Vector3.Lerp(currentPosition, posToLerpTo, t);
                yield return new WaitForEndOfFrame();
            }

            gunTransform.localPosition = posToLerpTo;
        }
    }
}