using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using Items.ItemInterfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace GunStuff.PlayerAiming
{
    public class PlayerGunPosition : MonoBehaviour
    {
        public EventHandler GunIsReadyToFire;
        public EventHandler GunIsNotReadyToFire;
        public UnityEvent playerAiming;
        public UnityEvent playerNotAiming;

        private Vector3 TotalGunTargetPosition => _targetGunPosition + _currentCrouchOffset + _currentAimOffset;

        private Vector3 TotalTargetRearSightPosition =>
            _originalRearSightPosition + _currentCrouchOffset + _currentAimOffset;

        private Vector3 TotalTargetFrontSightPosition =>
            _originalFrontSightPosition + _currentCrouchOffset + _currentAimOffset;

        [SerializeField] private Transform aimTarget;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float distanceFromRearSight;
        [SerializeField] private Transform rightShoulderBone;
        [SerializeField] private float maxTurnDegrees;
        [SerializeField] private float lowerWeaponSpeed;

        private IEnumerator _cameraLerper;
        private IEnumerator _weaponPositionLerper;
        private IEnumerator _recoilRecoveryLerp;
        private float _originalFOV;
        private Transform _originalParent;
        private Vector3 _originalCameraPosition;
        private Vector3 _originalGunPosition;
        private Vector3 _originalRearSightPosition;
        private Vector3 _originalFrontSightPosition;
        private Vector3 _currentCrouchOffset;
        private Vector3 _currentAimOffset;
        private bool _gunIsEquipped;
        private PlayerInput _playerInput;
        private InputAction _aimAction;
        private InputAction _lowerRaiseGunAction;
        private GunComponentsPositionData _currentlyEquippedGunComponents;
        private GunPositions _currentlyEquippedGunPositions;
        private bool _gunIsAvoidingWall;
        private GunRecoil _gunRecoil;
        private bool _gunIsLowered;
        private Vector3 _targetGunPosition;
        private Quaternion _targetGunRotation;

        private const float ACTION_TIME = 0.2f;

        private CoRoutineStarter _cameraLerp;
        private CoRoutineStarter _weaponLerp;
        private CoRoutineStarter _recoverLerp;
        private PlayerMovement _playerMovement;


        public void EquipGun(IGun gun)
        {
            _gunIsEquipped = true;
            _currentlyEquippedGunComponents = gun.Components;
            _currentlyEquippedGunPositions = gun.StatePositions;
            _gunRecoil = gun.RecoilBehaviour;
            _gunRecoil.RecoilFinished += OnRecoilFinished;

            _currentlyEquippedGunComponents.GunFulcrum.localPosition = gun.EquippedPosition.EquippedLocalPosition;

            _originalGunPosition = _currentlyEquippedGunComponents.GunMesh.localPosition;
            _originalFrontSightPosition = _currentlyEquippedGunComponents.FrontSight.localPosition;
            _originalRearSightPosition = _currentlyEquippedGunComponents.RearSight.localPosition;
            ReadyWeapon(0);
        }

        public void UnEquipGun()
        {
            _gunIsEquipped = false;
            _gunRecoil.RecoilFinished -= OnRecoilFinished;
            _currentlyEquippedGunComponents.GunMesh.localRotation = Quaternion.identity;
            _currentlyEquippedGunComponents.GunMesh.localPosition = _originalGunPosition;
            _currentlyEquippedGunComponents.FrontSight.localPosition = _originalFrontSightPosition;
            _currentlyEquippedGunComponents.RearSight.localPosition = _originalRearSightPosition;
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

            _cameraLerp = new CoRoutineStarter(this);
            _weaponLerp = new CoRoutineStarter(this);
            _recoverLerp = new CoRoutineStarter(this);

            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            if (!_gunIsEquipped) return;

            bool playerIsRunning = _playerMovement.CurrentSpeed.y > lowerWeaponSpeed;
            float playerSpeed = _playerMovement.CurrentSpeed.y;
            if (playerIsRunning)
            {
                LowerGun();
            }

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
                    ReadyWeapon(playerSpeed);
                }
            }

            if (!_gunIsAvoidingWall)
            {
                Transform cameraTransform = mainCamera.transform;

                if (_aimAction.WasPressedThisFrame())
                {
                    if (_gunIsLowered) ReadyWeapon(playerSpeed);
                    AimGun(cameraTransform, playerSpeed);
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
                        ReadyWeapon(playerSpeed);
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
            fulcrum.rotation = Quaternion.RotateTowards(fulcrum.rotation, lookAtRotation, maxTurnDegrees);
        }

        private void LowerGun()
        {
            _gunIsLowered = true;
            _targetGunPosition = _currentlyEquippedGunPositions.GunDownPosition;
            _targetGunRotation = Quaternion.Euler(_currentlyEquippedGunPositions.GunDownRotation);

            UnAimGun(mainCamera.transform);

            StartGunLerp(
                TotalGunTargetPosition,
                TotalTargetRearSightPosition,
                TotalTargetFrontSightPosition,
                _targetGunRotation,
                ACTION_TIME);
            GunIsNotReadyToFire?.Invoke(this, EventArgs.Empty);
        }

        private void AvoidWall()
        {
            _gunIsAvoidingWall = true;
            _targetGunPosition = _currentlyEquippedGunPositions.AvoidingObjectPosition;
            _targetGunRotation = Quaternion.Euler(_currentlyEquippedGunPositions.AvoidingObjectRotation);
            UnAimGun(mainCamera.transform);

            StartGunLerp(
                TotalGunTargetPosition,
                TotalTargetRearSightPosition,
                TotalTargetFrontSightPosition,
                _targetGunRotation,
                ACTION_TIME);
            GunIsNotReadyToFire?.Invoke(this, EventArgs.Empty);
        }

        private void ReadyWeapon(float playerSpeed)
        {
            if (playerSpeed > 1) return;

            _gunIsAvoidingWall = false;
            _gunIsLowered = false;

            _targetGunPosition = _originalGunPosition;
            _targetGunRotation = Quaternion.identity;

            StartGunLerp(
                TotalGunTargetPosition,
                TotalTargetRearSightPosition,
                TotalTargetFrontSightPosition,
                _targetGunRotation,
                ACTION_TIME);
            GunIsReadyToFire?.Invoke(this, EventArgs.Empty);
        }

        private void AimGun(Transform cameraTransform, float playerSpeed)
        {
            if (playerSpeed > 1) return;

            _currentAimOffset = _currentlyEquippedGunComponents.AimPosition;
            cameraTransform.parent = _currentlyEquippedGunComponents.RearSight;
            StartCameraLerp(
                new Vector3(0, 0, distanceFromRearSight),
                60f,
                ACTION_TIME);

            playerAiming.Invoke();
            StartGunLerp(
                TotalGunTargetPosition,
                TotalTargetRearSightPosition,
                TotalTargetFrontSightPosition,
                _targetGunRotation,
                ACTION_TIME);
        }

        private void UnAimGun(Transform cameraTransform)
        {
            cameraTransform.parent = _originalParent;
            _currentAimOffset = Vector3.zero;
            StartCameraLerp(
                _originalCameraPosition,
                _originalFOV,
                ACTION_TIME);

            playerNotAiming.Invoke();
            StartGunLerp(
                TotalGunTargetPosition,
                TotalTargetRearSightPosition,
                TotalTargetFrontSightPosition,
                _targetGunRotation,
                ACTION_TIME);
        }

        private void OnPlayerCrouched(object sender, EventArgs e)
        {
            _currentCrouchOffset = -Vector3.up * PlayerCrouchBehaviour.CROUCH_DISTANCE;
            if (!_gunIsEquipped) return;

            StartGunLerp(
                TotalGunTargetPosition,
                TotalTargetRearSightPosition,
                TotalTargetFrontSightPosition,
                _targetGunRotation,
                ACTION_TIME);
        }

        private void OnPlayerUnCrouched(object sender, EventArgs e)
        {
            _currentCrouchOffset = Vector3.zero;
            if (!_gunIsEquipped) return;

            StartGunLerp(
                TotalGunTargetPosition,
                TotalTargetRearSightPosition,
                TotalTargetFrontSightPosition,
                _targetGunRotation,
                ACTION_TIME);
        }

        private void OnRecoilFinished(object sender, EventArgs e)
        {
            StartRecoverLerp(
                TotalGunTargetPosition,
                _targetGunRotation,
                ACTION_TIME);
        }

        private void StartCameraLerp(
            Vector3 cameraPositionToLerpTo,
            float fovToLerpTo,
            float lerpTime)
        {
            _cameraLerp.Start(
                Lerper.Lerp(
                    new List<ILerpComponent>
                    {
                        new LerpLocalVector(cameraPositionToLerpTo, mainCamera.transform),
                        new LerpFOV(fovToLerpTo, mainCamera)
                    },
                    lerpTime));
        }

        private void StartGunLerp(
            Vector3 gunPosTarget,
            Vector3 rearSightPosTarget,
            Vector3 frontSightPosTarget,
            Quaternion rotToLerpTo,
            float lerpTime)
        {
            _weaponLerp.Start(
                Lerper.Lerp(
                    new List<ILerpComponent>
                    {
                        new LerpLocalVector(gunPosTarget, _currentlyEquippedGunComponents.GunMesh),
                        new LerpLocalQuaternion(rotToLerpTo, _currentlyEquippedGunComponents.GunMesh),
                        new LerpLocalVector(rearSightPosTarget, _currentlyEquippedGunComponents.RearSight),
                        new LerpLocalVector(frontSightPosTarget, _currentlyEquippedGunComponents.FrontSight)
                    },
                    lerpTime));
        }

        private void StartRecoverLerp(
            Vector3 gunPosTarget,
            Quaternion rotToLerpTo,
            float lerpTime)
        {
            _recoverLerp.Start(
                Lerper.Lerp(
                    new List<ILerpComponent>
                    {
                        new LerpLocalVector(gunPosTarget, _currentlyEquippedGunComponents.GunMesh),
                        new LerpLocalQuaternion(rotToLerpTo, _currentlyEquippedGunComponents.GunMesh)
                    },
                    lerpTime));
        }
    }
}