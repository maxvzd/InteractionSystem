using System.Collections.Generic;
using GunStuff;
using UnityEngine;

namespace Items.Properties
{
    public class GunProperties : WeaponProperties
    {
        //Metres per second
        public float MuzzleVelocity => muzzleVelocity;
        //Metres per second
        public float EffectiveRange => effectiveRange;
        public float Recoil => recoil;
        public float RoundsPerMinute => roundsPerMinute;
        public AudioClip FireSound => fireSound;
        public IReadOnlyList<FireMode> AvailableFireModes => availableFireModes;
        
        [SerializeField] private float recoil;
        [SerializeField] private float roundsPerMinute;
        [SerializeField] private AudioClip fireSound;
        [SerializeField] private float muzzleVelocity;
        [SerializeField] private float effectiveRange;
        [SerializeField] private List<FireMode> availableFireModes;
    }
}