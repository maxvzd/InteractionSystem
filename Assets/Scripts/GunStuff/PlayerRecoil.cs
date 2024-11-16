using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GunStuff
{
    public class PlayerRecoil : MonoBehaviour
    {
        [SerializeField] private float recoilTime;

        private IEnumerator _lerpCoRoutine;
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
            if (_lerpCoRoutine is not null)
            {
                StopCoroutine(_lerpCoRoutine);
            }
            
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
            _lerpCoRoutine = Lerper.Lerp(lerpComponents, recoilTime);
            StartCoroutine(_lerpCoRoutine);
        }
    }
}