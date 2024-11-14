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
    private bool _gunIsReadyToFire;

    private void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        _attackAction = input.actions[InputConstants.FireAction];
        _playerEquipment = GetComponent<PlayerEquipper>();
        _playerGunPosition = GetComponent<PlayerGunPosition>();
        _playerGunPosition.GunIsNotReadyToFire += OnGunIsNotReadyToFire;
        _playerGunPosition.GunIsReadyToFire += GunIsReadyToFire;
    }
    
    private void Update()
    {
        if (!_playerEquipment.IsWeaponEquipped || !_gunIsReadyToFire) return;
        
        if (_attackAction.WasPressedThisFrame())
        {
            _playerEquipment.EquipmentSlots.Weapon.AttackDown();
        }

        if (_attackAction.WasReleasedThisFrame())
        {
            _playerEquipment.EquipmentSlots.Weapon.AttackUp();
        }
    }

    private void GunIsReadyToFire(object sender, EventArgs args)
    {
        _gunIsReadyToFire = true;
    }

    private void OnGunIsNotReadyToFire(object sender, EventArgs args)
    {
        _gunIsReadyToFire = false;
        if (_playerEquipment.IsWeaponEquipped)
        {
            _playerEquipment.EquipmentSlots.Weapon.AttackUp();
        }
    }
}