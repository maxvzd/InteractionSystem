using Items.ItemInterfaces;
using UnityEngine;

namespace Items.Properties
{
    [CreateAssetMenu(menuName = "ItemProperties/WeaponProperties")]
    public class WeaponProperties: ItemProperties
    {
        public float Damage => damage;
        
        [SerializeField] private float damage;
    }
}