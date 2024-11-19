using System.Collections;
using GunStuff.FireBehaviour;
using Items.ItemInterfaces;
using Items.Properties;
using Items.Weapons;
using UnityEngine;

namespace GunStuff.FireModes
{
    public abstract class BaseFireMode : IFireMode
    {
        public abstract FireMode FireMode { get; }
        public abstract bool TriggerDown();
        public abstract void TriggerUp();

        protected bool RoundsPerMinuteLock;
        protected readonly Gun Gun;
        private readonly float _weaponLockWaitTime;

        protected BaseFireMode(Gun gun)
        {
            Gun = gun;
            RoundsPerMinuteLock = true;
            float roundsPerMinute = gun.GunProperties.RoundsPerMinute;
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