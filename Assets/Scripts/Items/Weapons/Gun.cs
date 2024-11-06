using System;
using System.Collections.Generic;
using GunStuff;
using GunStuff.FireBehaviour;
using GunStuff.FireModes;
using Items.ItemInterfaces;
using Items.Properties;
using UnityEngine;

namespace Items.Weapons
{
    public abstract class Gun : BaseItem, IGun
    {
        public Transform CurrentAimAtTarget { get; set; } 
        
        public GunProperties GunProperties => gunProperties;
        public override IInteractableProperties Properties => gunProperties;
        public EquipmentSlot EquipmentSlot => EquipmentSlot.Weapon;
        public EquippedPosition EquippedPosition => equippedPosition;
        public override bool IsEquippable => true;
        public override IItemProperties ItemProperties => gunProperties;
        public Vector3 MuzzlePosition => muzzleTransform.position;
        public event EventHandler<GunFiredEventArgs> GunFired;

        [SerializeField] private EquippedPosition equippedPosition;
        [SerializeField] private GunProperties gunProperties;
        [SerializeField] private Transform muzzleTransform;

        private List<IFireMode> _fireModes;
        private IFireMode _currentFireMode;
        private int _selectedFireMode;
        private bool _triggerDown;

        private void Start()
        {
            IShotFireBehaviour shotFireBehaviour = new SingleShotFireBehaviour();

            List<IFireMode> fireModes = new List<IFireMode>();
            foreach (FireMode fireMode in GunProperties.AvailableFireModes)
            {
                switch (fireMode)
                {
                    case FireMode.SemiAuto:
                        fireModes.Add(new SemiAutoFireMode(this, shotFireBehaviour));
                        break;
                    case FireMode.Auto:
                        fireModes.Add(new FullAutoFireMode(this, shotFireBehaviour));
                        break;
                    default:
                        break;
                }
            }

            _fireModes = fireModes;
            _currentFireMode = _fireModes[0];
        }

        private void Update()
        {
            if (_triggerDown)
            {
                if (_currentFireMode.Fire())
                {
                    GunFired?.Invoke(this, new GunFiredEventArgs(GunProperties.Recoil));
                }
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
            _currentFireMode.TriggerUp();
        }

        public void SwitchFireMode()
        {
            if (_triggerDown || _fireModes is null || _fireModes.Count < 1) return;

            _selectedFireMode++;
            if (_selectedFireMode > _fireModes.Count - 1)
            {
                _selectedFireMode = 0;
            }

            _currentFireMode = _fireModes[_selectedFireMode]; // : new SemiAutoFireMode(GunProperties);
            Debug.Log($"Switching to: {_currentFireMode.FireMode}");
        }
    }
}

public class GunFiredEventArgs : EventArgs
{
    public float Recoil { get; }
    
    public GunFiredEventArgs(float recoil)
    {
        Recoil = recoil;
    }

}