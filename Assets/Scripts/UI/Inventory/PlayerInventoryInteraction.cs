using System.Collections.Generic;
using System.Linq;
using Constants;
using Items.ItemInterfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace UI.Inventory
{
    public class PlayerInventoryInteraction : MonoBehaviour
    {
        [SerializeField] private UIDocument inventoryUI;
        [SerializeField] private VisualTreeAsset inventoryItemVisualTemplate;
    
        private PlayerWearableEquipment _equipment;
        private InputAction _openInventoryAction;
        private InputAction _closeInventoryAction;
        private Animator _animator;
        private bool _isBackpackOut;
        //private HoldInputButton _heldChecker;
        private float _buttonHeldTime;
        private bool _uiIsHidden;

        public UnityEvent uiShown;
        public UnityEvent uiHidden;

        private void Start()
        {
            //_inventory = new List<IItem>();
            _equipment = GetComponent<PlayerWearableEquipment>();
            PlayerInput input = GetComponent<PlayerInput>();
            _openInventoryAction = input.actions[InputConstants.OpenInventoryAction];
            _closeInventoryAction = input.actions[InputConstants.CloseInventoryAction];

            _animator = GetComponent<Animator>();
            //_heldChecker = new HoldInputButton();

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
            uiHidden.Invoke();
        }

        private void ShowUI()
        {
            if (!_uiIsHidden) return;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            IReadOnlyList<IItem> items = _equipment.IsBackpackEquipped ? _equipment.Backpack.Inventory : new List<IItem>(); 
            
            InventoryListController inventoryListController = new InventoryListController();
            inventoryListController.InitialiseItemList(inventoryUI.rootVisualElement, inventoryItemVisualTemplate, items);
        
            inventoryUI.rootVisualElement.style.display = DisplayStyle.Flex;
            _uiIsHidden = false;
            uiShown.Invoke();
        }
    
        private void Update()
        {
            //HeldButtonDetails heldDetails = _heldChecker.CheckForButtonHold(_openInventoryAction, Time.deltaTime);

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
        {
            if (_isBackpackOut)
            {
                return _equipment.Backpack.AddItem(item);
            }

            return AddItemToBackpackResult.BackpackIsNotOut;
        }
    }
}