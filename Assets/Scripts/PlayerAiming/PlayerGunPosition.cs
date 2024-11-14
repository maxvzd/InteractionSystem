using System;
using System.Collections;
using Constants;
using GunStuff;
using Items.ItemInterfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace PlayerAiming
{
    public class PlayerGunPosition : MonoBehaviour
    {
        public EventHandler GunIsReadyToFire;
        public EventHandler GunIsNotReadyToFire;
        
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
        private InputAction _lowerRaiseGunAction;
        private GunComponentsPositionData _currentlyEquippedGunComponents;
        private GunPositions _currentlyEquippedGunPositions;
        private Vector3 _targetGunPosition;
        private Quaternion _targetGunRotation;
        private bool _gunIsAvoidingWall;
        private GunRecoil _gunRecoil;
        private bool _gunIsLowered;
        private const float ACTION_TIME = 0.2f;

        public void EquipGun(IGun gun)
        {
            _gunIsEquipped = true;
            _currentlyEquippedGunComponents = gun.Components;
            _currentlyEquippedGunPositions = gun.StatePositions;
            _gunRecoil = gun.RecoilBehaviour;
            _gunRecoil.RecoilFinished += OnRecoilFinished;

            _originalGunPosition = gun.EquippedPosition.EquippedLocalPosition;
            ReadyWeapon();
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
            _lowerRaiseGunAction = _playerInput.actions[InputConstants.RaiseLowerGunAction];
            PlayerCrouchBehaviour crouchBehaviour = GetComponent<PlayerCrouchBehaviour>();
            crouchBehaviour.PlayerUnCrouched += OnPlayerUnCrouched;
            crouchBehaviour.PlayerCrouched += OnPlayerCrouched;
        }

        private void Update()
        {
            if (!_gunIsEquipped) return;

            if (!_gunIsLowered)
            {
                Vector3 raiseWeaponRayStart = rightShoulderBone.position;
                Vector3 direction = (aimTarget.position - raiseWeaponRayStart).normalized;
                const float distance = 1.1f;
                const float weaponRadius = 0.1f;

                Ray raiseWeaponRay = new Ray(raiseWeaponRayStart, direction);
                bool objectIsInFrontOfPlayer = Physics.SphereCast(
                    raiseWeaponRay,
                    weaponRadius,
                    distance,
                    ~LayerMask.GetMask(LayerConstants.LAYER_PLAYER));

                if (objectIsInFrontOfPlayer && !_gunIsAvoidingWall)
                {
                    AvoidWall();
                }
                else if (!objectIsInFrontOfPlayer && _gunIsAvoidingWall)
                {
                    ReadyWeapon();
                }
            }

            if (!_gunIsAvoidingWall)
            {
                Transform cameraTransform = mainCamera.transform;

                if (_aimAction.WasPressedThisFrame())
                {
                    if (_gunIsLowered) ReadyWeapon();
                    AimGun(cameraTransform);
                }

                if (_aimAction.WasReleasedThisFrame())
                {
                    UnAimGun(cameraTransform);
                }

                if (_lowerRaiseGunAction.WasPerformedThisFrame())
                {
                    if (!_gunIsLowered)
                    {
                        LowerGun();
                    }
                    else
                    {
                        ReadyWeapon();
                    }
                }
            }
            
            Transform fulcrum = _currentlyEquippedGunComponents.GunFulcrum;
            Quaternion lookAtRotation = Quaternion.LookRotation(fulcrum.position - aimTarget.position);

            
            if (_gunIsAvoidingWall || _gunIsLowered)
            {
                Vector3 currentRotation = transform.eulerAngles;
                currentRotation.y += 180;
                Quaternion noLookRotation = Quaternion.Euler(currentRotation);

                float dot = Quaternion.Dot(noLookRotation, lookAtRotation);
                float t = -0.15f * dot + 0.15f; //y = mx + C and 0.3f weight on lookRotation
                lookAtRotation = Quaternion.Lerp(noLookRotation, lookAtRotation, t);
            }

            fulcrum.rotation = lookAtRotation;
        }

        private void LowerGun()
        {
            _gunIsLowered = true;
            _targetGunPosition = _currentlyEquippedGunPositions.GunDownPosition;
            _targetGunRotation = Quaternion.Euler(_currentlyEquippedGunPositions.GunDownRotation);
            StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition, _targetGunRotation, ACTION_TIME);
            GunIsNotReadyToFire?.Invoke(this, EventArgs.Empty);
        }

        private void AvoidWall()
        {
            _gunIsAvoidingWall = true;
            _targetGunPosition = _currentlyEquippedGunPositions.AvoidingObjectPosition;
            _targetGunRotation = Quaternion.Euler(_currentlyEquippedGunPositions.AvoidingObjectRotation);
            StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition, _targetGunRotation, ACTION_TIME);
            GunIsNotReadyToFire?.Invoke(this, EventArgs.Empty);
        }

        private void ReadyWeapon()
        {
            _gunIsAvoidingWall = false;
            _gunIsLowered = false;
            _targetGunPosition = _originalGunPosition;
            _targetGunRotation = Quaternion.identity;
            StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition, _targetGunRotation, ACTION_TIME);
            GunIsReadyToFire?.Invoke(this, EventArgs.Empty);
        }

        private void AimGun(Transform cameraTransform)
        {
            _currentAimOffset = _currentlyEquippedGunComponents.AimPosition;
            cameraTransform.parent = _currentlyEquippedGunComponents.RearSight;
            StartCameraLerp(
                cameraTransform,
                new Vector3(0, 0, distanceFromRearSight),
                60f,
                0.2f);

            playerAiming.Invoke();
            StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition, _targetGunRotation, ACTION_TIME);
        }

        private void UnAimGun(Transform cameraTransform)
        {
            cameraTransform.parent = _originalParent;
            _currentAimOffset = Vector3.zero;
            StartCameraLerp(cameraTransform,
                _originalCameraPosition,
                _originalFOV,
                0.2f);

            playerNotAiming.Invoke();
            StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition, _targetGunRotation, ACTION_TIME);
        }

        private void OnPlayerCrouched(object sender, EventArgs e)
        {
            _currentCrouchOffset = -Vector3.up * PlayerCrouchBehaviour.CROUCH_DISTANCE;
            if (!_gunIsEquipped) return;

            StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition,
                _targetGunRotation, ACTION_TIME);
        }

        private void OnPlayerUnCrouched(object sender, EventArgs e)
        {
            _currentCrouchOffset = Vector3.zero;
            if (!_gunIsEquipped) return;

            StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition,
                _targetGunRotation, ACTION_TIME);
        }

        private void OnRecoilFinished(object sender, EventArgs e)
        {
            StartGunLerp(_currentlyEquippedGunComponents, TotalTargetPosition, _targetGunRotation, ACTION_TIME);
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