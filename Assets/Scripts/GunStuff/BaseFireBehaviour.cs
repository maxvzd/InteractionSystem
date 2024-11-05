using System.Collections;
using Items.Properties;
using Items.Weapons;
using UnityEngine;

namespace GunStuff
{
    public abstract class BaseFireBehaviour : IGunFireBehaviour
    {
        public abstract FireMode FireMode { get; }

        public abstract void Fire(Gun gun);
        public abstract void TriggerUp();

        protected bool RoundsPerMinuteLock;
        private readonly float _weaponLockWaitTime;

        protected BaseFireBehaviour(GunProperties properties)
        {
            RoundsPerMinuteLock = true;
            float roundsPerMinute = properties.RoundsPerMinute;
            float roundsPerSecond = roundsPerMinute / 60f;
            _weaponLockWaitTime = 1 / roundsPerSecond;
        }
        
        protected IEnumerator WaitForNextRoundToBeReadyToFire()
        {
            yield return new WaitForSeconds(_weaponLockWaitTime);
            RoundsPerMinuteLock = true;
        }
    }
}