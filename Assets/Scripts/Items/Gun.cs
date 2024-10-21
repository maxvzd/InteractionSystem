using Items.Properties;
using UnityEngine;

namespace Items
{
    public abstract class Gun : MonoBehaviour, IInteractable, IEquipabble
    {
        [SerializeField] private GunProperties gunProperties;
        public GunProperties GunProperties => gunProperties;
        public IProperties Properties => gunProperties;
        
        public abstract void EquipItem(Transform player);

        public abstract void UnEquipItem();
    }
}