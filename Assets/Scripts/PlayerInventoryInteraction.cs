﻿using Constants;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerInventoryInteraction : MonoBehaviour
{
    [SerializeField] private UIDocument inventoryUI;
    
    private PlayerWearableEquipment _equipment;
    private InputAction _openInventoryAction;
    private Animator _animator;
    private bool _isBackpackOut;
    private HoldInputButton _heldChecker;
    private float _buttonHeldTime;
    private bool _uiIsHidden;

    private void Start()
    {
        //_inventory = new List<IItem>();
        _equipment = GetComponent<PlayerWearableEquipment>();
        PlayerInput input = GetComponent<PlayerInput>();
        _openInventoryAction = input.actions[InputConstants.OpenInventoryAction];

        _animator = GetComponent<Animator>();
        _heldChecker = new HoldInputButton();

        HideUI();
    }

    private void GetBackpackOut()
    {
        _isBackpackOut = true;
        
        _animator.SetTrigger(AnimatorConstants.UnEquipBackpackTrigger);
        
        _equipment.Backpack.Transform.localPosition += _equipment.Backpack.BackpackOutPositionOffset;
        _equipment.Backpack.Transform.localEulerAngles += _equipment.Backpack.BackpackOutRotationOffset;
        //Animator backpackAnimator =_equipment.Backpack.Transform.GetComponent<Animator>();
        //backpackAnimator.SetBool(AnimatorConstants.BackpackIsOut, _isBackpackOut);
    }

    private void PutBackpackAway()
    {
        _isBackpackOut = false;
        
        _equipment.Backpack.Transform.localPosition -= _equipment.Backpack.BackpackOutPositionOffset;
        _equipment.Backpack.Transform.localEulerAngles -= _equipment.Backpack.BackpackOutRotationOffset;
        
        _animator.SetTrigger(AnimatorConstants.EquipBackpackTrigger);
        //Animator backpackAnimator =_equipment.Backpack.Transform.GetComponent<Animator>();
        //backpackAnimator.SetBool(AnimatorConstants.BackpackIsOut, _isBackpackOut);
    }

    private void HideUI()
    {
        if (_uiIsHidden) return;
        
        inventoryUI.rootVisualElement.style.display = DisplayStyle.None;
        _uiIsHidden = true;
    }

    private void ShowUI()
    {
        if (!_uiIsHidden) return;
        
        inventoryUI.rootVisualElement.style.display = DisplayStyle.Flex;
        _uiIsHidden = false;
    }
    
    private void Update()
    {
        HeldButtonDetails heldDetails = _heldChecker.CheckForButtonHold(_openInventoryAction, Time.deltaTime);

        if (heldDetails.WasButtonHeld)
        {
            if (_isBackpackOut)
            {
                PutBackpackAway();
            }
            HideUI();
            return;
        }
        
        if (heldDetails.WasButtonPressed)
        {
            
            if (!_isBackpackOut && _equipment.IsBackpackEquipped)
            {
                GetBackpackOut();
                return;
            }
            
            if (!_uiIsHidden)
            {
                HideUI();
            }
            else
            {
                ShowUI();
            }
        }
        
    }
}