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

        private CoRoutineStarter _recoilLerpRoutine;
        private CoRoutineStarter _recoilEventInvokeRoutine;
        private Vector3 _aimTargetPosAtEndOfRecoil;
        private Vector3 _originalFulcrumPosition;
        public EventHandler RecoilFinished;

        private void Start()
        {
            _recoilLerpRoutine = new CoRoutineStarter(this);
            _recoilEventInvokeRoutine = new CoRoutineStarter(this);
        }

        public void AddRecoil(object sender, GunFiredEventArgs recoilArgs)
        {
            GunComponentsPositionData positionData = recoilArgs.PositionData;
            Transform gunTransform = positionData.GunMesh;
            
            IEnumerable<ILerpComponent> lerpComponents = new List<ILerpComponent>
            {
                new LerpLocalVector(gunTransform.localPosition + Vector3.forward * recoilArgs.BackwardsRecoil, gunTransform),
                new LerpLocalQuaternion(gunTransform.localRotation * Quaternion.Euler(new Vector3(recoilArgs.RotationRecoil, Random.Range(-2, 2), 0)), gunTransform)
            };

            _recoilLerpRoutine.Start(Lerper.Lerp(lerpComponents, recoilSpeed));
            _recoilEventInvokeRoutine.Start(InvokeRecoilFinished(recoilSpeed));
        }

        private IEnumerator InvokeRecoilFinished(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            RecoilFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}