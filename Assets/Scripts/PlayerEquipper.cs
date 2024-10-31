using System;
using System.Collections.Generic;
using Constants;
using GunStuff;
using Items.ItemInterfaces;
using UnityEngine;

public class PlayerEquipper : MonoBehaviour
{
    public bool IsBackpackEquipped => _backpackSlot is not null;
    public bool IsWeaponEquipped => _weaponSlot is not null;
    public PlayerEquipmentSlots EquipmentSlots => new PlayerEquipmentSlots(_backpackSlot, _weaponSlot);
    
    [SerializeField] private Transform backpackSocket;
    
    //Slots 
    private IBackpack _backpackSlot;
    private IWeapon _weaponSlot;

    //Player components
    private GunEquipper _gunEquipper;
    private Animator _animator;

    private List<IWearableContainer> _playerContainers;
    public IEnumerable<IWearableContainer> PlayerContainers => _playerContainers;
    

    private void Start()
    {
        _gunEquipper = GetComponent<GunEquipper>();
        _animator = GetComponent<Animator>();

        _playerContainers = new List<IWearableContainer>();
    }

    public bool EquipItem(IEquippable item, Transform itemTransform)
    {
        if (item is IWearableContainer container)
        {
            _playerContainers.Add(container);
        }
        
        switch (item.EquipmentSlot)
        {
            case EquipmentSlot.Hands:
                break;
            case EquipmentSlot.UnderTorso:
                break;
            case EquipmentSlot.OverTorso:
                break;
            case EquipmentSlot.Legs:
                break;
            case EquipmentSlot.Feet:
                break;
            case EquipmentSlot.Head:
                break;
            case EquipmentSlot.Face:
                break;
            case EquipmentSlot.Back:
                IBackpack backpack = CastToType<IBackpack>(item);
                if (backpack is null) return false;
                return EquipItem(backpack, itemTransform);
            case EquipmentSlot.Wrist:
                break;
            case EquipmentSlot.Weapon:
                if (IsWeaponEquipped) return false;
                IWeapon weapon = CastToType<IWeapon>(item);
                return EquipItem(weapon, itemTransform);
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }

    public void UnEquipItem(EquipmentSlot slot)
    {
        switch (slot)
        {
            case EquipmentSlot.Hands:
                break;
            case EquipmentSlot.UnderTorso:
                break;
            case EquipmentSlot.OverTorso:
                break;
            case EquipmentSlot.Legs:
                break;
            case EquipmentSlot.Feet:
                break;
            case EquipmentSlot.Head:
                break;
            case EquipmentSlot.Face:
                break;
            case EquipmentSlot.Back:
                break;
            case EquipmentSlot.Wrist:
                break;
            case EquipmentSlot.Weapon:
                _gunEquipper.UnEquipGun();
                _weaponSlot = null;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
        }
    }

    private T CastToType<T>(IEquippable itemToCast) where T : IEquippable
    {
        try
        {
            return (T)itemToCast;
        }
        catch (InvalidCastException)
        {
            return default;
        }
    }

    private bool EquipItem(IBackpack wearableContainer, Transform backpackTransform)
    {
        if (_backpackSlot is not null) return false;

        _animator.SetTrigger(AnimatorConstants.EquipBackpackTrigger);
        //_backpackTransform = backpackTransform;
        Animator backpackAnimator = backpackTransform.GetComponent<Animator>();
        if (backpackAnimator is not null)
        {
            backpackAnimator.SetTrigger(AnimatorConstants.EquipBackpackTrigger);
        }

        backpackTransform.SetParent(backpackSocket);
        LayerManager.ChangeLayerOfItem(backpackTransform, LayerMask.NameToLayer(LayerConstants.LAYER_PLAYER), TagConstants.PlayerTag);
        _backpackSlot = wearableContainer;
        backpackTransform.localPosition = wearableContainer.EquippedPosition.EquippedLocalPosition;
        backpackTransform.localEulerAngles = wearableContainer.EquippedPosition.EquippedLocalRotation;
        return true;
    }

    private bool EquipItem(IWeapon weapon, Transform itemTransform)
    {
        switch (weapon.ItemProperties.Type)
        {
            case ItemType.Rifle:
                if (!_gunEquipper.EquipRifle(itemTransform, weapon)) return false;
                _weaponSlot = weapon;
                return true;
            case ItemType.Pistol:
                if (!_gunEquipper.EquipPistol(itemTransform, weapon)) return false;
                _weaponSlot = weapon;
                return true;
            default:
                _weaponSlot = null;
                return false;
        }
    }

    public void GetBackpackOut()
    {
        backpackSocket.localPosition += _backpackSlot.BackpackOutPositionOffset;
        backpackSocket.localEulerAngles += _backpackSlot.BackpackOutRotationOffset;
    }

    public void PutBackpackAway()
    {
        backpackSocket.localPosition -= _backpackSlot.BackpackOutPositionOffset;
        backpackSocket.localEulerAngles -= _backpackSlot.BackpackOutRotationOffset;
    }

    public void RemoveItemFromWearableContainers(IItem item)
    {
        foreach (IWearableContainer container in _playerContainers)
        {
            if (container.RemoveItem(item))
            {
                return;
            }
        }
    }

    public AddItemToBackpackResult AddItem(IItem item)
    {
        return _backpackSlot.AddItem(item);
    }
}