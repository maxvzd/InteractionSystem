using System;
using System.Collections;
using Constants;
using GunStuff;
using Items;
using Items.ItemInterfaces;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace PlayerAiming
{
    public class PlayerGunPosition : MonoBehaviour
    {
        private Vector3 TotalTargetPosition => _targetGunPosition + _currentCrouchOffset + _currentAimOffset;

        [SerializeField] private Transform aimTarget;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private UnityEvent playerAiming;
        [SerializeField] private UnityEvent playerNotAiming;
        [SerializeField] private float distanceFromRearSight;
        [SerializeField] private float recoverySpeed;
        [SerializeField] private Transform rightShoulderBone;
        [SerializeField] private AnimationCurve recoveryCurve;

        private IEnumerator _cameraLerper;
        private IEnumerator _weaponPositionLerper;
        private float _originalFOV;
        private Transform _originalParent;
        private Vector3 _originalCameraPosition;
        private Vector3 _originalGunPosition;
        private Vector3 _currentCrouchOffset;
        private Vector3 _currentAimOffset;
        private bool _gunIsEquipped;
        private PlayerInput _playerInput;
        private InputAction _aimAction;
        private GunComponentsPositionData _currentlyEquippedGunComponents;
        private GunPositions _currentlyEquippedGunPositions;
        private Vector3 _targetGunPosition;
        private Quaternion _targetGunRotation;
        private bool _isGunAvoidingWall;
        private GunRecoil _gunRecoil;

        private const float ActionTime = 0.2f;
        public EventHandler GunRaised;
        public EventHandler GunLowered;

        public void EquipGun(IGun gun)
        {
            _gunIsEquipped = true;

            _originalGunPosition = gun.EquippedPosition.EquippedLocalPosition;
            
            _targetGunPosition = _originalGunPosition;
            _targetGunRotation = Quaternion.Euler(gun.EquippedPosition.EquippedLocalRotation);

            _currentlyEquippedGunComponents = gun.Components;
            _currentlyEquippedGunPositions = gun.StatePositions;

            _gunRecoil = gun.RecoilBehaviour;
            _gunRecoil.RecoilFinished += OnRecoilFinished;
            
            StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition, _targetGunRotation, ActionTime);
        }

        public void UnEquipGun()
        {
            _gunIsEquipped = false;
            _gunRecoil.RecoilFinished -= OnRecoilFinished;
        }

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

        private void Update()
        {
            if (!_gunIsEquipped) return;

            Vector3 raiseWeaponRayStart = rightShoulderBone.position;
            Vector3 direction = (aimTarget.position - raiseWeaponRayStart).normalized;
            const float distance = 1.1f;
            const float weaponRadius = 0.1f;

            Transform cameraTransform = mainCamera.transform;
            Transform fulcrum = _currentlyEquippedGunComponents.GunFulcrum;
            
            Ray raiseWeaponRay = new Ray(raiseWeaponRayStart, direction);
            bool objectIsInFrontOfPlayer = Physics.SphereCast(
                raiseWeaponRay, 
                weaponRadius, 
                distance,
                ~LayerMask.GetMask(LayerConstants.LAYER_PLAYER));
            
            if (objectIsInFrontOfPlayer && !_isGunAvoidingWall)
            {
                _isGunAvoidingWall = true;
                _targetGunPosition = _currentlyEquippedGunPositions.AvoidingObjectPosition;
                _targetGunRotation = Quaternion.Euler(_currentlyEquippedGunPositions.AvoidingObjectRotation);
                StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition, _targetGunRotation, ActionTime);
                GunRaised?.Invoke(this,EventArgs.Empty);
            }
            else if (!objectIsInFrontOfPlayer && _isGunAvoidingWall)
            {
                _isGunAvoidingWall = false;
                _targetGunPosition = _originalGunPosition;
                _targetGunRotation = Quaternion.identity;
                StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition, _targetGunRotation, ActionTime);
                GunLowered?.Invoke(this,EventArgs.Empty);
            }

            if (!_isGunAvoidingWall)
            {
                if (_aimAction.WasPressedThisFrame())
                {
                    _currentAimOffset = _currentlyEquippedGunComponents.AimPosition;
                    cameraTransform.parent = _currentlyEquippedGunComponents.RearSight;
                    StartCameraLerp(
                        cameraTransform,
                        new Vector3(0, 0, distanceFromRearSight),
                        60f,
                        0.2f);

                    playerAiming.Invoke();
                    StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition, _targetGunRotation, ActionTime);
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
                    StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition, _targetGunRotation, ActionTime);
                }
            }

            if (!_isGunAvoidingWall)
            {
                Quaternion lookAtRotation = Quaternion.LookRotation(fulcrum.position - aimTarget.position);
                fulcrum.rotation = lookAtRotation;
            }
        }

        private void OnPlayerCrouched(object sender, EventArgs e)
        {
            _currentCrouchOffset = -Vector3.up * PlayerCrouchBehaviour.CROUCH_DISTANCE;
            if (!_gunIsEquipped) return;

            StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition,
                Quaternion.identity, ActionTime);
        }

        private void OnPlayerUnCrouched(object sender, EventArgs e)
        {
            _currentCrouchOffset = Vector3.zero;
            if (!_gunIsEquipped) return;
            
            StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition,
                Quaternion.identity, ActionTime);
        }

        private void OnRecoilFinished(object sender, EventArgs e)
        {
            StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition, _targetGunRotation, ActionTime);
        }

        private void StartCameraLerp(
            Transform cameraTransform,
            Vector3 cameraPositionToLerpTo,
            float fovToLerpTo,
            float lerpTime)
        {
            if (!ReferenceEquals(_cameraLerper, null))
            {
                StopCoroutine(_cameraLerper);
            }

            _cameraLerper = LerpCamera(cameraTransform, cameraPositionToLerpTo, fovToLerpTo, lerpTime);
            StartCoroutine(_cameraLerper);
        }

        private void StartGunLerp(
            GunComponentsPositionData components,
            Vector3 gunPosToLerpTo,
            Quaternion rotToLerpTo,
            float lerpTime)
        {
            if (!ReferenceEquals(_weaponPositionLerper, null))
            {
                StopCoroutine(_weaponPositionLerper);
            }

            _weaponPositionLerper = LerpGun(components, gunPosToLerpTo, rotToLerpTo, lerpTime);
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
            GunComponentsPositionData components,
            Vector3 posToLerpTo,
            Quaternion rotToLerpTo,
            float lerpTime)
        {
            Transform fulcrumTransform = components.GunFulcrum;
            Transform gunMesh = components.GunMesh;
            Vector3 currentPosition = fulcrumTransform.localPosition;
            Quaternion currentRotation = gunMesh.localRotation;

            float elapsedTime = 0f;

            while (elapsedTime < lerpTime)
            {
                float t = elapsedTime / lerpTime;
                elapsedTime += Time.deltaTime;

                fulcrumTransform.localPosition = Vector3.Lerp(currentPosition, posToLerpTo, t);
                gunMesh.localRotation = Quaternion.Lerp(currentRotation, rotToLerpTo, t);
                yield return new WaitForEndOfFrame();
            }

            fulcrumTransform.localPosition = posToLerpTo;
            gunMesh.localRotation = rotToLerpTo;
        }
    }
}