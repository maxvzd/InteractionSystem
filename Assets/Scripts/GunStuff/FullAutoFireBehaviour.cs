using Items.Properties;
using Items.Weapons;
using UnityEngine;

namespace GunStuff
{
    public class FullAutoFireBehaviour : BaseFireBehaviour
    {
        public override FireMode FireMode => FireMode.Auto;
        
        public FullAutoFireBehaviour(GunProperties properties) : base(properties)
        {
        }
        
         public override void TriggerUp()
         {
             //nothing to do
         }

        public override void Fire(Gun gun)
        {
            if (RoundsPerMinuteLock)
            {
                RoundsPerMinuteLock = false;
                Debug.Log("Firing auto");
    
                gun.StartCoroutine(WaitForNextRoundToBeReadyToFire());
            }
        }
    }
}