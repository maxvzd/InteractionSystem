using GunStuff.FireBehaviour;
using Items.Weapons;

namespace GunStuff.FireModes
{
    public class FullAutoFireMode : BaseFireMode
    {
        private bool _hasClicked;
        public override FireMode FireMode => FireMode.Auto;
        
        public FullAutoFireMode(Gun gun) : base(gun)
        {
        }
        
         public override void TriggerUp()
         {
             _hasClicked = false;
         }

        public override bool TriggerDown()
        {
            if (!RoundsPerMinuteLock || _hasClicked ) return false;
            if (Gun.Ammunition is null || Gun.Ammunition.CurrentBullets <= 0)
            {
                _hasClicked = true;
            }
            
            RoundsPerMinuteLock = false;
            Gun.StartCoroutine(WaitForNextRoundToBeReadyToFire());
            return true;
        }
    }
}