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
        public event EventHandler<GunFiredEventArgs> GunFired;
        public GunComponentsPositionData Components { get; private set; }
        public AudioSource AudioSource { get; private set; }
        public GunRecoil RecoilBehaviour { get; private set; }
        public GunPositions StatePositions => positions;

        [SerializeField] private EquippedPosition equippedPosition;
        [SerializeField] private GunProperties gunProperties;
        [SerializeField] private GunPositions positions;

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
            Components = GetComponent<GunComponentsPositionData>();
            AudioSource = GetComponent<AudioSource>();
            RecoilBehaviour = GetComponent<GunRecoil>();
        }

        private void Update()
        {
            if (_triggerDown)
            {
                if (_currentFireMode.Fire())
                {
                    GunFired?.Invoke(this, new GunFiredEventArgs(
                        GunProperties.VerticalRecoil, 
                        GunProperties.BackwardsRecoil, 
                        GunProperties.RotationRecoil, 
                        Components));
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
    public float VerticalRecoil { get; }
    public float BackwardsRecoil { get; }
    public float RotationRecoil { get; }
    public GunComponentsPositionData PositionData { get; }
    
    public GunFiredEventArgs(float verticalRecoil, float backwardsRecoil, float rotationRecoil, GunComponentsPositionData positionData)
    {
        VerticalRecoil = verticalRecoil;
        BackwardsRecoil = backwardsRecoil;
        PositionData = positionData;
        RotationRecoil = rotationRecoil;
    }

}