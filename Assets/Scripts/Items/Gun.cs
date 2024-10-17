using PlayerAiming;
using UnityEngine;

namespace Items
{
    public class Gun : Item
    {
        private GunEquipper _currentPlayerEquipper;
        
        public override void EquipItem(Transform player)
        {
            GameObject gunGameObject = transform.gameObject;
            _currentPlayerEquipper = player.GetComponent<GunEquipper>();
            _currentPlayerEquipper.EquipGun(gunGameObject);
        }

        public override void UnEquipItem()
        {
            _currentPlayerEquipper.UnEquipGun();
            _currentPlayerEquipper = null;
        }
    }
}