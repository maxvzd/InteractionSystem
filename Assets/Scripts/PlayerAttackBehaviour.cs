using System;
using Constants;
using PlayerAiming;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackBehaviour : MonoBehaviour
{
    private InputAction _attackAction;
    private PlayerEquipper _playerEquipment;
    private PlayerGunPosition _playerGunPosition;
    private bool _gunIsRaised;

    private void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        _attackAction = input.actions[InputConstants.FireAction];
        _playerEquipment = GetComponent<PlayerEquipper>();
        _playerGunPosition = GetComponent<PlayerGunPosition>();
        _playerGunPosition.GunLowered += OnGunLowered;
        _playerGunPosition.GunRaised += OnGunRaised;
    }
    
    private void Update()
    {
        if (!_playerEquipment.IsWeaponEquipped || _gunIsRaised) return;
        
        if (_attackAction.WasPressedThisFrame())
        {
            _playerEquipment.EquipmentSlots.Weapon.AttackDown();
        }

        if (_attackAction.WasReleasedThisFrame())
        {
            _playerEquipment.EquipmentSlots.Weapon.AttackUp();
        }
    }

    private void OnGunRaised(object sender, EventArgs args)
    {
        _gunIsRaised = true;
        if (_playerEquipment.IsWeaponEquipped)
        {
            _playerEquipment.EquipmentSlots.Weapon.AttackUp();
        }
    }

    private void OnGunLowered(object sender, EventArgs args)
    {
        _gunIsRaised = false;
    }
}