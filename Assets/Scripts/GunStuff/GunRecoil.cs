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
        [SerializeField] private float damperWhenAiming;

        private CoRoutineStarter _recoilLerpRoutine;
        private CoRoutineStarter _recoilEventInvokeRoutine;
        private Vector3 _aimTargetPosAtEndOfRecoil;
        private Vector3 _originalFulcrumPosition;
        public EventHandler RecoilFinished;
        private bool _playerIsAiming;

        public void AddRecoil(object sender, GunFiredEventArgs recoilArgs)
        {
            GunComponentsPositionData positionData = recoilArgs.PositionData;
            Transform gunTransform = positionData.GunMesh;

            float verticalRecoil = recoilArgs.RotationRecoil;
            float backwardsRecoil = recoilArgs.BackwardsRecoil;
            float horizontalRecoil = Random.Range(-2, 2);

            if (_playerIsAiming)
            {
                backwardsRecoil *= damperWhenAiming;
                verticalRecoil *= damperWhenAiming;
                horizontalRecoil *= damperWhenAiming;
            }
            
            IEnumerable<ILerpComponent> lerpComponents = new List<ILerpComponent>
            {
                new LerpLocalVector(gunTransform.localPosition + Vector3.forward * backwardsRecoil, gunTransform),
                new LerpLocalQuaternion(gunTransform.localRotation * Quaternion.Euler(new Vector3(verticalRecoil, horizontalRecoil, 0)), gunTransform)
            };

            _recoilLerpRoutine.Start(Lerper.Lerp(lerpComponents, recoilSpeed));
            _recoilEventInvokeRoutine.Start(InvokeRecoilFinished(recoilSpeed));
        }

        public void PlayerIsAiming()
        {
            _playerIsAiming = true;
        }

        public void PlayerIsNotAiming()
        {
            _playerIsAiming = false;
        }
        
        private void Start()
        {
            _recoilLerpRoutine = new CoRoutineStarter(this);
            _recoilEventInvokeRoutine = new CoRoutineStarter(this);
        }

        private IEnumerator InvokeRecoilFinished(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            RecoilFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}