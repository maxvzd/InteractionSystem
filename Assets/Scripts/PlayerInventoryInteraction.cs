using System;
using System.Collections.Generic;
using Constants;
using Items.ItemInterfaces;
using UI.Inventory;
using UI.Inventory.Controllers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerInventoryInteraction : MonoBehaviour
{
    [SerializeField] private UIDocument inventoryUI;

    private PlayerEquipper _equipment;
    private InputAction _openInventoryAction;
    private InputAction _closeInventoryAction;
    private Animator _animator;
    private bool _isBackpackOut;
    private float _buttonHeldTime;
    private bool _uiIsHidden;
    private PlayerInput _input;
    private InventoryController _inventoryController;

    private void Start()
    {
        _equipment = GetComponent<PlayerEquipper>();
        _animator = GetComponent<Animator>();

        _input = GetComponent<PlayerInput>();
        _openInventoryAction = _input.actions[InputConstants.OpenInventoryAction];
        _closeInventoryAction = _input.actions[InputConstants.CloseInventoryAction];

        _inventoryController = new InventoryController(inventoryUI.rootVisualElement);
        _inventoryController.RetrieveItemClicked += RetrieveItemClicked;
        
        //_equipment.ItemAdded

        HideUI();
    }
    
    public IReadOnlyDictionary<Guid, IItem> Inventory
    {
        get
        {
            Dictionary<Guid, IItem> items = new Dictionary<Guid, IItem>(); 
            foreach (IWearableContainer wearableContainer in _equipment.PlayerContainers)
            {
                items.AddRange(wearableContainer.Inventory);
            }
            return items;
        }
    }

    private void RetrieveItemClicked(object sender, ItemEventArgs e)
    {
        IItem item = Inventory[e.ItemID];
        GameObject itemPrefab = ItemIdLookUp.Instance.GetItemPrefab(item.ItemProperties.PrefabId);
        if (itemPrefab != null)
        {
            Instantiate(itemPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
            _equipment.RemoveItemFromWearableContainers(item);
        }
    }

    private void GetBackpackOut()
    {
        _isBackpackOut = true;

        _equipment.GetBackpackOut();
        _animator.SetTrigger(AnimatorConstants.UnEquipBackpackTrigger);

        //Animator backpackAnimator =_equipment.Backpack.Transform.GetComponent<Animator>();
        //backpackAnimator.SetBool(AnimatorConstants.BackpackIsOut, _isBackpackOut);
    }

    private void PutBackpackAway()
    {
        _isBackpackOut = false;

        _equipment.PutBackpackAway();
        _animator.SetTrigger(AnimatorConstants.EquipBackpackTrigger);

        //Animator backpackAnimator =_equipment.Backpack.Transform.GetComponent<Animator>();
        //backpackAnimator.SetBool(AnimatorConstants.BackpackIsOut, _isBackpackOut);
    }

    private void HideUI()
    {
        if (_uiIsHidden) return;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inventoryUI.rootVisualElement.style.display = DisplayStyle.None;
        _uiIsHidden = true;

        _input.actions.Enable();
    }

    private void ShowUI()
    {
        if (!_uiIsHidden) return;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        //IEnumerable<IItem> items = _equipment.Inventory;
        _inventoryController.PopulateItems(Inventory);

        inventoryUI.rootVisualElement.style.display = DisplayStyle.Flex;
        _uiIsHidden = false;

        _input.actions.Disable();
        _openInventoryAction.Enable();
    }

    private void Update()
    {
        if (_closeInventoryAction.WasPerformedThisFrame())
        {
            if (_isBackpackOut)
            {
                PutBackpackAway();
            }

            HideUI();
        }
        else if (_openInventoryAction.WasPerformedThisFrame())
        {
            if (!_isBackpackOut && _equipment.IsBackpackEquipped)
            {
                GetBackpackOut();
            }
            else if (!_uiIsHidden)
            {
                HideUI();
            }
            else
            {
                ShowUI();
            }
        }
    }

    //Add support for multiple instance of wearable container
    public AddItemToBackpackResult AddItemToInventory(IItem item)
        => _isBackpackOut ? _equipment.Backpack.AddItem(item) : AddItemToBackpackResult.BackpackIsNotOut;
}