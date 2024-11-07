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
            return true;
        }

        private void ShootRayCast(IGun gun)
        {
            GunPositionData positionData = gun.PositionData;
            Vector3 direction = (positionData.FrontSight.position - positionData.RearSight.position).normalized;
            
            Ray ray = new Ray(positionData.MuzzlePosition, direction);
            //Debug.DrawRay(muzzleTransform, -transform.forward * props.EffectiveRange, Color.green, 1f);
            if (Physics.Raycast(ray, out RaycastHit hit, gun.GunProperties.EffectiveRange))
            {
                Debug.DrawRay(ray.origin, ray.direction * gun.GunProperties.EffectiveRange, Color.green, 1f);
            
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
                Debug.DrawRay(ray.origin, ray.direction * gun.GunProperties.EffectiveRange, Color.red, 1f);
            }
        }
    }
}