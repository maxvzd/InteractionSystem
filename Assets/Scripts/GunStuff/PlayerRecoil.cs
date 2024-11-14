using System.Collections;
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
        
        public void AddRecoil(object sender, GunFiredEventArgs eventArgs)
        {
            if (_lerpCoRoutine is not null)
            {
                StopCoroutine(_lerpCoRoutine);
            }

            _lerpCoRoutine = AddRecoilCoRoutine(recoilTime, eventArgs);
            StartCoroutine(_lerpCoRoutine);
        }
        
        private IEnumerator AddRecoilCoRoutine(float lerpTime, GunFiredEventArgs recoilArgs)
        {
            float timeElapsed = 0f;

            Quaternion currentRotation = transform.rotation;
            float recoil = -recoilArgs.VerticalRecoil;
            if (_playerIsAiming)
            {
                recoil *= 0.3f;
            }
            Quaternion targetRotation = currentRotation * Quaternion.Euler(recoil, 0, 0);
            
            while (timeElapsed < lerpTime)
            {
                float t = timeElapsed / lerpTime;
                
                transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, t);

                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;
            }

            transform.rotation = targetRotation;
        }
    }
}