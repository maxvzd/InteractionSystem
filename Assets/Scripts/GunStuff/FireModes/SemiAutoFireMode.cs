using GunStuff.FireBehaviour;
using Items.Weapons;

namespace GunStuff.FireModes
{
    public class SemiAutoFireMode : BaseFireMode
    {
        public override  FireMode FireMode => FireMode.SemiAuto;
        
        private bool _hasFiredWithTriggerDown;
        
        public SemiAutoFireMode(Gun gun, IShotFireBehaviour shotFireBehaviour) : base(gun, shotFireBehaviour)
        {
        }
        
        public override bool Fire()
        {
            if (!RoundsPerMinuteLock || _hasFiredWithTriggerDown) return false;
            
            RoundsPerMinuteLock = false;
            _hasFiredWithTriggerDown = ShotFireBehaviour.Fire(Gun);
            Gun.StartCoroutine(WaitForNextRoundToBeReadyToFire());
            return true;
        }
        
        public override void TriggerUp()
        {
            _hasFiredWithTriggerDown = false;
        }
    }
}