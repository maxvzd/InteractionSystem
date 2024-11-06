using GunStuff.FireBehaviour;
using Items.Weapons;

namespace GunStuff.FireModes
{
    public class FullAutoFireMode : BaseFireMode
    {
        public override FireMode FireMode => FireMode.Auto;
        
        public FullAutoFireMode(Gun gun, IShotFireBehaviour shotFireBehaviour) : base(gun, shotFireBehaviour)
        {
        }
        
         public override void TriggerUp()
         {
             //nothing to do
         }

        public override bool Fire()
        {
            if (!RoundsPerMinuteLock) return false;
            
            RoundsPerMinuteLock = false;
            ShotFireBehaviour.Fire(Gun);
            Gun.StartCoroutine(WaitForNextRoundToBeReadyToFire());
            return true;
        }
    }
}