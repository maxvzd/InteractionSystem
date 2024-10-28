using System.Collections.Generic;
using Constants;
using Items.ItemInterfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace UI.Inventory
{
    public class PlayerInventoryInteraction : MonoBehaviour
    {
        [SerializeField] private UIDocument inventoryUI;
    
        private PlayerWearableEquipment _equipment;
        private InputAction _openInventoryAction;
        private InputAction _closeInventoryAction;
        private Animator _animator;
        private bool _isBackpackOut;
        private float _buttonHeldTime;
        private bool _uiIsHidden;
        private PlayerInput _input;

        private void Start()
        {
            _equipment = GetComponent<PlayerWearableEquipment>();
            _animator = GetComponent<Animator>();
            
            _input = GetComponent<PlayerInput>();
            _openInventoryAction = _input.actions[InputConstants.OpenInventoryAction];
            _closeInventoryAction = _input.actions[InputConstants.CloseInventoryAction];
            
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

            IReadOnlyList<IItem> items = _equipment.IsBackpackEquipped ? _equipment.Backpack.Inventory : new List<IItem>(); 

            InventoryTabController tabController = new InventoryTabController(inventoryUI.rootVisualElement, items);
        
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

        public AddItemToBackpackResult AddItemToInventory(IItem item) 
            => _isBackpackOut ? _equipment.Backpack.AddItem(item) : AddItemToBackpackResult.BackpackIsNotOut;
    }
}