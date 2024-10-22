﻿using GunStuff;
using UnityEngine;

namespace Items
{
    public class Rifle : Gun
    {
        private GunEquipper _currentPlayerEquipper;
        
        public override void EquipItem(Transform player)
        {
            GameObject gunGameObject = transform.gameObject;
            _currentPlayerEquipper = player.GetComponent<GunEquipper>();
            _currentPlayerEquipper.EquipRifle(gunGameObject);
            IsEquipped = true;
        }

        public override void UnEquipItem()
        {
            IsEquipped = false;
            _currentPlayerEquipper.UnEquipGun();
            _currentPlayerEquipper = null;
        }
    }
}