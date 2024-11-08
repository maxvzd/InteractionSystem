using System.Collections;
using UnityEngine;

namespace GunStuff
{
    public class GunRecoil : MonoBehaviour
    {
        [SerializeField] private float recoveryTime;
        [SerializeField] private float recoilSpeed;

        private IEnumerator _recoilLerpRoutine;
        private bool _isRecoiling;
        private float _recoveryTimeElapsed;
        private Vector3 _aimTargetPosAtEndOfRecoil;
        private Vector3 _originalFulcrumPosition;
        private Vector3 _rotationAtEndOfRecoil;
        private Vector3 _fulcrumPositionAtEndOfRecoil;
        private bool _gunHasBeenFired;
        private GunPositionData _posData;

        private void Start()
        {
            _originalFulcrumPosition = Vector3.zero;
        }

        private void Update()
        {
            if (_isRecoiling || !_gunHasBeenFired) return;

            if (_recoveryTimeElapsed < recoveryTime)
            {
                float t = _recoveryTimeElapsed / recoveryTime;

                Transform fulcrumTransform = _posData.GunFulcrum;
                Transform gunTransform = _posData.GunMesh;

                fulcrumTransform.localPosition = Vector3.Lerp(_fulcrumPositionAtEndOfRecoil, _originalFulcrumPosition, t);
                gunTransform.localEulerAngles = Vector3.Lerp(_rotationAtEndOfRecoil, Vector3.zero, t);

                _recoveryTimeElapsed += Time.deltaTime;
            }
        }

        public void AddRecoil(object sender, GunFiredEventArgs eventArgs)
        {
            _gunHasBeenFired = true;

            if (_recoilLerpRoutine is not null)
            {
                StopCoroutine(_recoilLerpRoutine);
            }

            _recoilLerpRoutine = AddRecoilCoRoutine(recoilSpeed, eventArgs.Recoil, eventArgs.PositionData);
            StartCoroutine(_recoilLerpRoutine);
        }

        private IEnumerator AddRecoilCoRoutine(float lerpTime, float recoilAmount, GunPositionData positionData)
        {
            _posData = positionData;

            float timeElapsed = 0f;
            Transform fulcrumTransform = positionData.GunFulcrum;
            Transform gunTransform = positionData.GunMesh;
            
            //TODO: Fix crouching/aiming positions
            if (_originalFulcrumPosition == Vector3.zero) _originalFulcrumPosition = fulcrumTransform.localPosition;

            Vector3 currentFulcrumPos = fulcrumTransform.localPosition;
            Vector3 targetFulcrumPos = currentFulcrumPos - Vector3.forward * (recoilAmount * 0.005f);
            Vector3 currentGunMeshRot = positionData.GunMesh.localEulerAngles;

            //TODO Add some random sway n stuff
            //Vector3 targetGunMeshRot = currentGunMeshRot + new Vector3(recoilAmount, Random.Range(-5, 5), 0);
            Vector3 targetGunMeshRot = currentGunMeshRot + new Vector3(recoilAmount, 0, 0);

            _isRecoiling = true;
            while (timeElapsed < lerpTime)
            {
                float t = timeElapsed / lerpTime;
                fulcrumTransform.localPosition = Vector3.Lerp(currentFulcrumPos, targetFulcrumPos, t);
                gunTransform.localEulerAngles = Vector3.Lerp(currentGunMeshRot, targetGunMeshRot, t);

                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;
            }

            _isRecoiling = false;
            fulcrumTransform.localPosition = targetFulcrumPos;
            gunTransform.localEulerAngles = targetGunMeshRot;

            _fulcrumPositionAtEndOfRecoil = fulcrumTransform.localPosition;
            _rotationAtEndOfRecoil = gunTransform.localEulerAngles;

            _recoveryTimeElapsed = 0f;
        }
    }
}