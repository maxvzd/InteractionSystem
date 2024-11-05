using Constants;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackBehaviour : MonoBehaviour
{
    private InputAction _attackAction;
    private PlayerEquipper _playerEquipment;

    private void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        _attackAction = input.actions[InputConstants.FireAction];

        _playerEquipment = GetComponent<PlayerEquipper>();
    }

    private void Update()
    {
        if (!_playerEquipment.IsWeaponEquipped) return;
        
        if (_attackAction.WasPressedThisFrame())
        {
            _playerEquipment.EquipmentSlots.Weapon.AttackDown();
        }

        if (_attackAction.WasReleasedThisFrame())
        {
            _playerEquipment.EquipmentSlots.Weapon.AttackUp();
        }
    }
}