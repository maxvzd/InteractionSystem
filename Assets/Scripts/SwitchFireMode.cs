using Constants;
using Items.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchFireMode : MonoBehaviour
{
    private PlayerEquipper _playerEquipment;
    private InputAction _switchFireModeAction;

    private void Start()
    {
        _playerEquipment = GetComponent<PlayerEquipper>();
        PlayerInput input = GetComponent<PlayerInput>();
        _switchFireModeAction = input.actions[InputConstants.SwitchFireMode];
    }

    private void Update()
    {
        if (!_playerEquipment.IsWeaponAGun) return;
        
        if (_switchFireModeAction.WasPerformedThisFrame())
        {
            if (_playerEquipment.EquipmentSlots.Weapon is not Gun gun) return;
            gun.SwitchFireMode();
        }
    }
}