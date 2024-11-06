using DealDamage;
using HitReactions;
using Items.Weapons;
using UnityEngine;

namespace GunStuff.FireBehaviour
{
    public class SingleShotFireBehaviour : IShotFireBehaviour
    {
        public bool Fire(Gun gun)
        {
            ShootRayCast(gun);
            return true;
        }

        private void ShootRayCast(Gun gun)
        {
            Vector3 direction = (gun.CurrentAimAtTarget.position - gun.MuzzlePosition).normalized * gun.GunProperties.EffectiveRange;
            
            Ray ray = new Ray(gun.MuzzlePosition, direction);
            //Debug.DrawRay(muzzleTransform, -transform.forward * props.EffectiveRange, Color.green, 1f);
            if (Physics.Raycast(ray, out RaycastHit hit, gun.GunProperties.EffectiveRange))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.green, 1f);
            
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
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);
            }
        }
    }
}