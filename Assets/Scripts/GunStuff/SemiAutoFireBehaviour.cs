using Items.Properties;
using Items.Weapons;
using UnityEngine;

namespace GunStuff
{
    public class SemiAutoFireBehaviour : BaseFireBehaviour
    {
        public override  FireMode FireMode => FireMode.SemiAuto;
        
        private bool _hasFiredWithTriggerDown;
        
        public SemiAutoFireBehaviour(GunProperties properties) : base(properties)
        {
        }
        
        public override void Fire(Gun gun)
        {
            if (RoundsPerMinuteLock && !_hasFiredWithTriggerDown)
            {
                RoundsPerMinuteLock = false;
                _hasFiredWithTriggerDown = true;
                Debug.Log("Firing semi");
                gun.StartCoroutine(WaitForNextRoundToBeReadyToFire());
            }
        }
        
        public override void TriggerUp()
        {
            _hasFiredWithTriggerDown = false;
        }
    }
}