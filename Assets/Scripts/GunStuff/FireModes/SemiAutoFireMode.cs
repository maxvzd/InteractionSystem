using Items.Weapons;

namespace GunStuff.FireModes
{
    public class SemiAutoFireMode : BaseFireMode
    {
        public override  FireMode FireMode => FireMode.SemiAuto;
        
        private bool _hasFiredWithTriggerDown;
        
        public SemiAutoFireMode(Gun gun) : base(gun)
        {
        }
        
        public override bool TriggerDown()
        {
            if (!RoundsPerMinuteLock || _hasFiredWithTriggerDown) return false;
            
            RoundsPerMinuteLock = false;
            _hasFiredWithTriggerDown = true;
            Gun.StartCoroutine(WaitForNextRoundToBeReadyToFire());
            return true;
        }
        
        public override void TriggerUp()
        {
            _hasFiredWithTriggerDown = false;
        }
    }
}