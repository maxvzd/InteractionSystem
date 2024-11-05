using System;
using DealDamage.DeadLimbEffects;
using RootMotion.Dynamics;
using UnityEngine;

namespace DealDamage
{
    public class LimbHealth : MonoBehaviour
    {
        public float Health => _health;

        //[SerializeField] private bool isLethal;
        [SerializeField] private float limbDamageModifier = 1f;
        [SerializeField] private LimbDeathEffect deathEffect;
        [SerializeField] private LimbType limbType;

        public LimbType LimbType => limbType;
        public bool zombieIsDead;

        private float _health = 100f;
        private bool _limbIsDead;

        private AudioSource _audioSource;
        private AudioClip _bulletImpactSound;
        private BehaviourPuppet _puppet;

        public delegate void LimbHitEventHandler(object sender, HitEventArgs e);

        public event LimbHitEventHandler LimbHit;

        public delegate void LimbCrippledEventHandler(object sender, LimbCrippledEventArgs e);

        public event LimbCrippledEventHandler LimbCrippled;

        public void Receive(float damage, Vector3 hitLocation)
        {
            _audioSource.clip = _bulletImpactSound;
            _audioSource.PlayDelayed(0.1f);

            if (zombieIsDead) return;

            float damageWithModifier = damage * limbDamageModifier;
            
            //main health takes modified damage
            LimbHit?.Invoke(this, new HitEventArgs(damageWithModifier, hitLocation, limbType));

            if (_limbIsDead) return;

            //limb takes full damage
            _health -= damage;
            _health = Mathf.Max(0, _health);

            if (deathEffect is not null && _health <= 0)
            {
                _limbIsDead = true;
                LimbCrippled?.Invoke(this, new LimbCrippledEventArgs(deathEffect));
            }
        }

        private void SetAudioSource(AudioSource source)
        {
            _audioSource = source;
        }

        private void AddBulletImpactSound(AudioClip bulletImpactSound)
        {
            _bulletImpactSound = bulletImpactSound;
        }

        public void SetupLimb(AudioSource source, AudioClip bulletImpactSound)
        {
            SetAudioSource(source);
            AddBulletImpactSound(bulletImpactSound);
        }
    }

    public class HitEventArgs : EventArgs
    {
        public float Damage { get; }
        public Vector3 HitLocation { get; }

        public LimbType LimbType { get; }

        public HitEventArgs(float damage, Vector3 hitLocation, LimbType limbType)
        {
            Damage = damage;
            HitLocation = hitLocation;
            LimbType = limbType;
        }
    }

    public class LimbCrippledEventArgs : EventArgs
    {
        public LimbCrippledEventArgs(LimbDeathEffect deathEffect)
        {
            DeathEffect = deathEffect;
        }

        public LimbDeathEffect DeathEffect { get; }
    }

    public enum LimbType
    {
        Head,
        Pelvis,
        Torso,
        RightArm,
        LeftArm,
        RightLeg,
        LeftLeg
    }
}