using Items.ItemInterfaces;
using Items.Properties;
using UnityEngine;

namespace Items
{
    public class Item : BaseItem, IInteractable
    {
        [SerializeField] private ItemProperties itemProperties;
        public ItemProperties ItemProperties => itemProperties;
        public virtual IProperties Properties => itemProperties;
    }
}