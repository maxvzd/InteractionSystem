using DealDamage;
using HitReactions;
using Items.ItemInterfaces;
using UnityEngine;

namespace GunStuff.FireBehaviour
{
    public class SingleShotFireBehaviour : IShotFireBehaviour
    {
        public bool Fire(IGun gun)
        {
            ShootRayCast(gun);
            gun.AudioSource.pitch = Random.Range(0.8f, 1);
            gun.AudioSource.PlayOneShot(gun.GunProperties.FireSound);
            
            return true;
        }

        private void ShootRayCast(IGun gun)
        {
            GunComponentsPositionData positionData = gun.Components;
            Vector3 fireDirection = (positionData.FrontSight.position - positionData.RearSight.position).normalized;
            
            Ray ray = new Ray(positionData.MuzzlePosition, fireDirection);
            if (!Physics.Raycast(ray, out RaycastHit hit, gun.GunProperties.EffectiveRange)) return;
            
            hit.transform.TryGetComponent(out LimbHealth receiveDamage);
            if (receiveDamage is not null)
            {
                receiveDamage.Receive(gun.GunProperties.Damage, hit.point);
            }
                
            hit.transform.TryGetComponent(out ReactToHit react);
            if (react is not null)
            {
                react.React(hit, ray.direction);
            }
        }
    }
}