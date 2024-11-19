using System.Collections.Generic;
using System.Linq;
using Constants;
using GunStuff.Ammunition;
using Items.ItemInterfaces;
using Items.Weapons;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunReloadBehaviour: MonoBehaviour
{
    private InputAction _reloadAction;
    private PlayerEquipper _playerEquipment;

    private void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        _reloadAction = input.actions[InputConstants.ReloadAction];

        _playerEquipment = GetComponent<PlayerEquipper>();
    }

    private void Update()
    {
        if (!_playerEquipment.IsWeaponAGun) return;
        
        if (_reloadAction.WasPerformedThisFrame())
        {
            if (_playerEquipment.EquipmentSlots.Weapon is IMagazineGun gun)
            {
                ReloadMagazineGun(gun);
            }
        }
    }

    private void ReloadMagazineGun(IMagazineGun gun)
    {
        List<Magazine> magazines = new List<Magazine>();
        foreach (IWearableContainer container in _playerEquipment.PlayerContainers)
        {
            magazines.AddRange(container.Inventory.Values.Where(x => x is Magazine));
        }

        Magazine magazineToUse = magazines.OrderByDescending(x => x.CurrentBullets).First();
        gun.ReloadMagazine(magazineToUse);
    }
}