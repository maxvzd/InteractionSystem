using System.Collections;
using UnityEngine;

namespace GunStuff
{
    public class GunRecoil : MonoBehaviour
    {
        [SerializeField] private float recoveryTime;
        [SerializeField] private float recoilSpeed;

        private IEnumerator _recoilLerpRoutine;
        private Vector3 _aimTargetPosAtEndOfRecoil;
        private Vector3 _originalFulcrumPosition;

        public void AddRecoil(object sender, GunFiredEventArgs eventArgs)
        {
            if (_recoilLerpRoutine is not null)
            {
                StopCoroutine(_recoilLerpRoutine);
            }

            _recoilLerpRoutine = AddRecoilCoRoutine(recoilSpeed, eventArgs.Recoil, eventArgs.PositionData);
            StartCoroutine(_recoilLerpRoutine);
        }

        private IEnumerator AddRecoilCoRoutine(float lerpTime, float recoilAmount, GunPositionData positionData)
        {
            float timeElapsed = 0f;
            Transform fulcrumTransform = positionData.GunFulcrum;
            Transform gunTransform = positionData.GunMesh;

            Vector3 currentFulcrumPos = fulcrumTransform.localPosition;
            Vector3 targetFulcrumPos = currentFulcrumPos - Vector3.forward * (recoilAmount * 0.005f); //Magic number to dampen recoil - probably generated from player aiming skill eventurall
            
            Quaternion currentGunMeshRot = positionData.GunMesh.localRotation;
            Quaternion targetGunMeshRot = currentGunMeshRot * Quaternion.Euler(new Vector3(recoilAmount, Random.Range(-3, 3), 0));
            
            while (timeElapsed < lerpTime)
            {
                float t = timeElapsed / lerpTime;
                fulcrumTransform.localPosition = Vector3.Lerp(currentFulcrumPos, targetFulcrumPos, t);
                
                gunTransform.localRotation = Quaternion.Lerp(currentGunMeshRot, targetGunMeshRot, t);

                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;
            }
            
            fulcrumTransform.localPosition = targetFulcrumPos;
            gunTransform.localRotation = targetGunMeshRot;
        }
    }
}