using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GunStuff
{
    public class GunRecoil : MonoBehaviour
    {
        [SerializeField] private float recoilSpeed;

        private IEnumerator _recoilLerpRoutine;
        private IEnumerator _recoilEventInvokeRoutine;
        private Vector3 _aimTargetPosAtEndOfRecoil;
        private Vector3 _originalFulcrumPosition;
        public EventHandler RecoilFinished;

        public void AddRecoil(object sender, GunFiredEventArgs recoilArgs)
        {
            if (_recoilLerpRoutine is not null)
            {
                StopCoroutine(_recoilLerpRoutine);
            }
            
            if (_recoilEventInvokeRoutine is not null)
            {
                StopCoroutine(_recoilEventInvokeRoutine);
            }

            GunComponentsPositionData positionData = recoilArgs.PositionData;
            Transform gunTransform = positionData.GunMesh;
            
            IEnumerable<ILerpComponent> lerpComponents = new List<ILerpComponent>
            {
                new LerpLocalVector(gunTransform.localPosition + Vector3.forward * recoilArgs.BackwardsRecoil, gunTransform),
                new LerpLocalQuaternion(gunTransform.localRotation * Quaternion.Euler(new Vector3(recoilArgs.RotationRecoil, Random.Range(-2, 2), 0)), gunTransform)
            };

            _recoilLerpRoutine = Lerper.Lerp(lerpComponents, recoilSpeed);
            StartCoroutine(_recoilLerpRoutine);
            
            _recoilEventInvokeRoutine = InvokeRecoilFinished(recoilSpeed);
            StartCoroutine(_recoilEventInvokeRoutine);
        }

        private IEnumerator InvokeRecoilFinished(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            RecoilFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}