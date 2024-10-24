using Items.ItemInterfaces;
using UnityEngine;

namespace Items.Properties
{
    [CreateAssetMenu(menuName = "ItemProperties/WeaponProperties")]
    public class WeaponProperties: ItemProperties
    {
        [SerializeField] private float damage;
    }
}