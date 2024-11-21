using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using GunStuff;
using GunStuff.Ammunition;
using Items.ItemInterfaces;
using Items.Weapons;
using RootMotion.FinalIK;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunReloadBehaviour: MonoBehaviour
{
    private InputAction _reloadAction;
    private PlayerEquipper _playerEquipment;
    private Animator _animator;
    private IKHandPlacement _ikManager;
    private FullBodyBipedIK _ik;

    private void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        _reloadAction = input.actions[InputConstants.ReloadAction];

        _playerEquipment = GetComponent<PlayerEquipper>();
        _animator = GetComponent<Animator>();
        _ikManager = GetComponent<IKHandPlacement>();
        AnimationEventListener animationListener = GetComponent<AnimationEventListener>();
        animationListener.FinishedReloading += FinishedReloading;   
    }

    private void FinishedReloading(object sender, EventArgs e)
    {
        _ikManager.EnableIk(.1f);
    }

    private void Update()
    {
        if (!_playerEquipment.IsWeaponAGun) return;
        
        if (_reloadAction.WasPerformedThisFrame())
        {
            if (_playerEquipment.EquipmentSlots.Weapon is MagazineGun gun)
            {
                ReloadMagazineGun(gun);
            }
        }
    }

    private void ReloadMagazineGun(MagazineGun gun)
    {
        List<Magazine> magazines = new List<Magazine>();
        foreach (IWearableContainer container in _playerEquipment.PlayerContainers)
        {
            magazines.AddRange(container.Inventory.Values.Where(x => x is Magazine mag && mag.MagazineType == gun.AcceptedMagazine));
        }

        Magazine magazineToUse = magazines.OrderByDescending(x => x.CurrentBullets).First();
        if (gun.ReloadMagazine(magazineToUse))
        {
            _ikManager.DisableIk(0.1f);
            _animator.SetTrigger(AnimatorConstants.ReloadAssaultRifleTrigger);
        }
    }
}