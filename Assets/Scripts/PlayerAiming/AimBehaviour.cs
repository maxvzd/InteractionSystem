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

        //private Vector3 _originalPosition;
        private IEnumerator _weaponPositionLerper;
        private float _originalFOV;

        private Transform _originalParent;
        private Vector3 _originalPos;

        public event EventHandler<EventArgs> PlayerAiming;
        public event EventHandler<EventArgs> PlayerNotAiming;

        private void Start()
        {
            //_originalPosition = rearSightTransform.localPosition;
            //_originalPosition = gunFulcrum.localPosition;
            _originalFOV = mainCamera.fieldOfView;

            _originalParent = mainCamera.transform.parent;
            _originalPos = mainCamera.transform.localPosition;
        }

        private void Update()
        {
            // if (Input.GetButtonDown(Constants.Fire2Key))
            // {
            //     Ray middleOfScreen = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            //
            //     //Debug.DrawRay(middleOfScreen.origin, middleOfScreen.direction, Color.red, 3f);
            //
            //     Vector3 desiredRearSightPositionInWorld = middleOfScreen.GetPoint(0.5f);
            //
            //     Vector3 desiredFulcrumPositionInWorld = gunFulcrum.position + (desiredRearSightPositionInWorld - rearSightTransform.position);
            //     
            //     Debug.DrawLine(rearSightTransform.position, desiredRearSightPositionInWorld, Color.green, 3f);
            //     Debug.DrawLine(gunFulcrum.position, desiredFulcrumPositionInWorld, Color.red, 3f);
            //     
            //     Debug.DrawRay(rearSightTransform.position, Vector3.up, Color.yellow, 3f);
            //     Debug.DrawRay(gunFulcrum.position, Vector3.up, Color.yellow, 3f);
            //     Debug.DrawRay(desiredRearSightPositionInWorld, Vector3.up, Color.magenta, 3f);
            //     Debug.DrawRay(desiredFulcrumPositionInWorld, Vector3.up, Color.magenta, 3f);
            //     
            //     //Vector3 localPoint = rearSightTransform.parent.InverseTransformPoint(desiredRearSightPositionInWorld);
            //     Vector3 localPoint = gunFulcrum.parent.InverseTransformPoint(desiredFulcrumPositionInWorld);
            //     
            //     StartWeaponLerpCoroutine(localPoint, 40, 0.2f);
            //
            //     PlayerAiming?.Invoke(this, EventArgs.Empty);
            // }
            //
            // if (Input.GetButtonUp(Constants.Fire2Key))
            // {
            //     StartWeaponLerpCoroutine(_originalPosition, _originalFOV, 0.2f);
            //     PlayerNotAiming?.Invoke(this, EventArgs.Empty);
            // }
            //
            // Quaternion lookAtRotation = Quaternion.LookRotation(gunFulcrum.position - aimTarget.position);
            // gunFulcrum.rotation = lookAtRotation;
            //
            // Quaternion lookAtRotationSight = Quaternion.LookRotation(rearSightTransform.position - aimTarget.position);
            // rearSightTransform.rotation = lookAtRotationSight;

            var cameraTransform = mainCamera.transform;
            if (Input.GetButtonDown(Constants.Fire2Key))
            {
                cameraTransform.parent = rearSightTransform;
                cameraTransform.localPosition = Vector3.zero;
                StartFovLerpCoroutine(40, 0.2f);
                
                PlayerAiming?.Invoke(this, EventArgs.Empty);
            }

            if (Input.GetButtonUp(Constants.Fire2Key))
            {
                cameraTransform.parent = _originalParent;
                cameraTransform.localPosition = _originalPos;
                StartFovLerpCoroutine(_originalFOV, 0.2f);
                
                PlayerNotAiming?.Invoke(this, EventArgs.Empty);
            }

            Quaternion lookAtRotation = Quaternion.LookRotation(gunFulcrum.position - aimTarget.position);
            gunFulcrum.rotation = lookAtRotation;

            Quaternion lookAtRotationSight = Quaternion.LookRotation(rearSightTransform.position - aimTarget.position);
            rearSightTransform.rotation = lookAtRotationSight;
        }

        // private void StartWeaponLerpCoroutine(Transform transformToLerp, Vector3 positionToLerpTo, float fovToLerpTo, float lerpTime)
        // {
        //     if (!ReferenceEquals(_weaponPositionLerper, null))
        //     {
        //         StopCoroutine(_weaponPositionLerper);
        //     }
        //
        //     _weaponPositionLerper = LerpTransformToPoint(transformToLerp, positionToLerpTo, fovToLerpTo, lerpTime);
        //     StartCoroutine(_weaponPositionLerper);
        // }

        private void StartFovLerpCoroutine(float fovToLerpTo, float lerpTime)
        {
            if (!ReferenceEquals(_weaponPositionLerper, null))
            {
                StopCoroutine(_weaponPositionLerper);
            }

            _weaponPositionLerper = LerpCameraFov(fovToLerpTo, lerpTime);
            StartCoroutine(_weaponPositionLerper);
        }

        // private IEnumerator LerpTransformToPoint(Transform transformToLerp, Vector3 positionToLerpTo, float fovToLerpTo, float lerpTime)
        // {
        //     Vector3 fromPosition = rearSightTransform.localPosition;
        //     float fromFov = mainCamera.fieldOfView;
        //     float elapsedTime = 0f;
        //
        //     while (elapsedTime < lerpTime)
        //     {
        //         float t = elapsedTime / lerpTime;
        //         elapsedTime += Time.deltaTime;
        //
        //         rearSightTransform.localPosition = Vector3.Lerp(fromPosition, positionToLerpTo, t);
        //         mainCamera.fieldOfView = Mathf.Lerp(fromFov, fovToLerpTo, t);
        //
        //         yield return new WaitForEndOfFrame();
        //     }
        //
        //     rearSightTransform.localPosition = positionToLerpTo;
        //     mainCamera.fieldOfView = fovToLerpTo;
        // }

        private IEnumerator LerpCameraFov(float fovToLerpTo, float lerpTime)
        {
            float fromFov = mainCamera.fieldOfView;
            float elapsedTime = 0f;

            while (elapsedTime < lerpTime)
            {
                float t = elapsedTime / lerpTime;
                elapsedTime += Time.deltaTime;

                mainCamera.fieldOfView = Mathf.Lerp(fromFov, fovToLerpTo, t);

                yield return new WaitForEndOfFrame();
            }

            mainCamera.fieldOfView = fovToLerpTo;
        }
    }
}