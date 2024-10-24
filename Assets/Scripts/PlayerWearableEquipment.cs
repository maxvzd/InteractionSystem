using System;
using Constants;
using GunStuff;
using Items.ItemInterfaces;
using UnityEngine;

public class PlayerWearableEquipment : MonoBehaviour
{
    public bool IsBackpackEquipped => _backpackSlot != null;
    public IWearableContainer Backpack => _backpackSlot;

    //[SerializeField] private UIDocument inventoryUI;
    [SerializeField] private Transform backpackSocket;

    //Slots 
    private IWearableContainer _backpackSlot;
    private IWeapon _rightHandWeaponSlot;

    //Player components
    private GunEquipper _gunEquipper;
    private Animator _animator;

    private void Start()
    {
        _gunEquipper = GetComponent<GunEquipper>();
        _animator = GetComponent<Animator>();
    }

    public bool EquipItem(IEquippable item)
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
                return EquipItem(wearableContainer);
            case EquipmentSlot.Wrist:
                break;
            case EquipmentSlot.Weapon:
                IWeapon weapon = CastToType<IWeapon>(item);
                return EquipItem(weapon, item.Transform);
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }

    public void UnEquipItem(EquipmentSlot slot)
    {
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

    private bool EquipItem(IWearableContainer wearableContainer)
    {
        if (_backpackSlot is not null) return false;

        _animator.SetTrigger(AnimatorConstants.EquipBackpackTrigger);
        Transform itemTransform = wearableContainer.Transform;
        Animator backpackAnimator = itemTransform.GetComponent<Animator>();
        if (backpackAnimator is not null)
        {
            backpackAnimator.SetTrigger(AnimatorConstants.EquipBackpackTrigger);
        }

        itemTransform.SetParent(backpackSocket);
        LayerManager.ChangeLayerOfItem(itemTransform, LayerMask.NameToLayer(LayerConstants.LAYER_PLAYER), TagConstants.PlayerTag);
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