using System;
using GunStuff;
using Items.ItemSlots;
using UnityEngine;

public class PlayerWearableEquipment : MonoBehaviour
{
    //Slots 
    private IWearableContainer _backpackSlot;
    private IWeapon _rightHandWeaponSlot;
    
    
    [SerializeField] private Transform backpackSocket;
    private GunEquipper _gunEquipper;

    private void Start()
    {
        _gunEquipper = GetComponent<GunEquipper>();
    }

    public bool  EquipItem(IEquipabble item, Transform itemTransform)
    {
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
                IWearableContainer wearableContainer = CastToType<IWearableContainer>(item);
                if (wearableContainer is null) return false;
                return EquipItem(wearableContainer, itemTransform);
            case EquipmentSlot.Wrist:
                break;
            case EquipmentSlot.Weapon:
                IWeapon weapon = CastToType<IWeapon>(item);
                return EquipItem(weapon, itemTransform);
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }

    public void UnEquipItem(EquipmentSlot slot)
    {
        
    }
    
    private T CastToType<T>(IEquipabble itemToCast) where T : IEquipabble
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

    private bool EquipItem(IWearableContainer wearableContainer, Transform itemTransform)
    {
        if (_backpackSlot is not null) return false;
        
        itemTransform.SetParent(backpackSocket);
        
        _backpackSlot = wearableContainer;
        itemTransform.localPosition = wearableContainer.EquippedPosition.EquippedLocalPosition;
        itemTransform.localEulerAngles = wearableContainer.EquippedPosition.EquippedLocalRotation;
        return true;
    }

    private bool EquipItem(IWeapon weapon, Transform itemTransform)
    {
        switch (weapon.WeaponType)
        {
            case WeaponType.Rifle:
                return _gunEquipper.EquipRifle(itemTransform, weapon);
            case WeaponType.Pistol:
                return _gunEquipper.EquipPistol(itemTransform, weapon);
            default:
                return false;
        }
    }
}

public enum EquipmentSlot
{
    Hands, //gloves etc
    UnderTorso, //tshirts
    OverTorso, //jackets
    Legs, //trousers
    Feet, //shoes/boots
    Head, //helmets/hats
    Face, //masks
    Back, //backpacks
    Wrist, //watches
    
    Weapon,
}