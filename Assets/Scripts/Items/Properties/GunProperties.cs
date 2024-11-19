using System.Collections.Generic;
using GunStuff;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items.Properties
{
    public class GunProperties : WeaponProperties
    {
        //Metres per second
        public float MuzzleVelocity => muzzleVelocity;
        //Metres per second
        public float EffectiveRange => effectiveRange;
        public float VerticalRecoil => verticalRecoil;
        public float BackwardsRecoil => backwardsRecoil;
        public float RotationRecoil => rotationRecoil;
        public float RoundsPerMinute => roundsPerMinute;
        public AudioClip FireSound => fireSound;
        public AudioClip EmptySound => emptySound;
        public IReadOnlyList<FireMode> AvailableFireModes => availableFireModes;
        
        [SerializeField] private float verticalRecoil;
        [SerializeField] private float backwardsRecoil;
        [SerializeField] private float rotationRecoil;
        [SerializeField] private float roundsPerMinute;
        [SerializeField] private AudioClip fireSound;
        [SerializeField] private AudioClip emptySound;
        [SerializeField] private float muzzleVelocity;
        [SerializeField] private float effectiveRange;
        [SerializeField] private List<FireMode> availableFireModes;
    }
}