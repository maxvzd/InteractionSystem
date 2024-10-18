using System;
using System.Collections;
using GunStuff;
using UnityEngine;

namespace PlayerAiming
{
    public class AimBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Camera mainCamera;

        private Transform _rearSight;
        private Transform _gunFulcrum;

        private IEnumerator _weaponPositionLerper;
        private float _originalFOV;

        private Transform _originalParent;
        private Vector3 _originalCameraPosition;
        private Vector3 _originalGunPosition;
        private Vector3 _targetGunPos;
        private bool _gunIsEquipped;

        public event EventHandler<EventArgs> PlayerAiming;
        public event EventHandler<EventArgs> PlayerNotAiming;

        private void Start()
        {
            _originalFOV = mainCamera.fieldOfView;

            Transform cameraTransform = mainCamera.transform;
            _originalParent = cameraTransform.parent;
            _originalCameraPosition = cameraTransform.localPosition;
        }

        public void EquipGun(GunPositionData posData)
        {
            _gunIsEquipped = true;
            _gunFulcrum = posData.GunFulcrum;
            _rearSight = posData.RearSight;

            _originalGunPosition = posData.GunLocalPosition;

            Transform currentTransform = transform;
            _targetGunPos = _originalGunPosition
                            + currentTransform.up * posData.AimPosition.y
                            + currentTransform.right * posData.AimPosition.x
                            + currentTransform.forward * posData.AimPosition.z;
        }

        public void UnEquipGun()
        {
            _gunIsEquipped = false;
            _gunFulcrum = null;
            _rearSight = null;

            _originalGunPosition = Vector3.zero;
            _targetGunPos = Vector3.zero;
        }

        private void Update()
        {
            if (!_gunIsEquipped) return;

            Transform cameraTransform = mainCamera.transform;
            if (Input.GetButtonDown(Constants.Fire2Key))
            {
                cameraTransform.parent = _rearSight;
                StartAimLerp(
                    cameraTransform,
                    Vector3.zero,
                    _gunFulcrum,
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
                    _gunFulcrum,
                    _originalGunPosition,
                    _originalFOV,
                    0.2f);

                PlayerNotAiming?.Invoke(this, EventArgs.Empty);
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