using System.Collections.Generic;
using UnityEngine;

namespace GunStuff
{
    public class PlayerRecoil : MonoBehaviour
    {
        [SerializeField] private float recoilTime;

        private CoRoutineStarter _lerpCoRoutine;
        private bool _playerIsAiming;

        public void OnPlayerAiming()
        {
            _playerIsAiming = true;
        }

        public void OnPlayerNotAiming()
        {
            _playerIsAiming = false;
        }
        
        public void AddRecoil(object sender, GunFiredEventArgs recoilArgs)
        {
            float recoil = -recoilArgs.VerticalRecoil;
            if (_playerIsAiming)
            {
                recoil *= 0.3f;
            }
            Quaternion targetRotation = transform.rotation * Quaternion.Euler(recoil, 0, 0);
            
            IList<ILerpComponent> lerpComponents = new List<ILerpComponent>
            {
                new LerpQuaternion(targetRotation, transform)
            };
            
            _lerpCoRoutine.Start(Lerper.Lerp(lerpComponents, recoilTime));
        }

        private void Start()
        {
            _lerpCoRoutine = new CoRoutineStarter(this);
        }
    }
}