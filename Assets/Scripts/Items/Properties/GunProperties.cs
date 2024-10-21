using UnityEngine;

namespace Items.Properties
{
    [CreateAssetMenu]
    public class GunProperties : WeaponProperties
    {
        [SerializeField] private float effectiveRange;
    }
}