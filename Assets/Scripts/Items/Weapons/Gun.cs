using System;
using System.Collections.Generic;
using GunStuff;
using Items.ItemInterfaces;
using Items.Properties;
using UnityEngine;

namespace Items.Weapons
{
    public abstract class Gun : BaseItem, IWeapon
    {
        public GunProperties GunProperties => gunProperties;
        public override IInteractableProperties Properties => gunProperties;
        public EquipmentSlot EquipmentSlot => EquipmentSlot.Weapon;
        public EquippedPosition EquippedPosition => equippedPosition;
        public override bool IsEquippable => true;
        public override IItemProperties ItemProperties => GunProperties;
        
        [SerializeField] private EquippedPosition equippedPosition;
        [SerializeField] private GunProperties gunProperties;
        private List<IGunFireBehaviour> _fireModes;
        private IGunFireBehaviour _currentFireBehaviour;
        private int _selectedFireMode = 0;
        private bool _triggerDown;

        private void Start()
        {
            List<IGunFireBehaviour> fireModes = new List<IGunFireBehaviour>();
            foreach (FireMode fireMode in GunProperties.AvailableFireModes)
            {
                switch (fireMode)
                {
                    case FireMode.SemiAuto:
                        fireModes.Add(new SemiAutoFireBehaviour(GunProperties));
                        break;
                    case FireMode.Auto:
                        fireModes.Add(new FullAutoFireBehaviour(GunProperties));
                        break;
                    default:
                        break;
                }
            }

            _fireModes = fireModes;
            _currentFireBehaviour = _fireModes[0];
        }

        private void Update()
        {
            if (_triggerDown)
            {
                _currentFireBehaviour.Fire(this);
            }
        }

        public void AttackDown()
        {
            TriggerDown();
        }

        public void AttackUp()
        {
            TriggerUp();
        }

        private void TriggerDown()
        {
            _triggerDown = true;
        }

        private void TriggerUp()
        {
            _triggerDown = false;
            _currentFireBehaviour.TriggerUp();
        }
        
        public void SwitchFireMode()
        {
            if (_triggerDown) return;
            
            _selectedFireMode++;
            if (_selectedFireMode > _fireModes.Count - 1)
            {
                _selectedFireMode = 0;
            }
            _currentFireBehaviour = _fireModes.Count > 0 ? _fireModes[_selectedFireMode] : new SemiAutoFireBehaviour(GunProperties);
            Debug.Log($"Switching to: {_currentFireBehaviour.FireMode}");
        }
    }
}