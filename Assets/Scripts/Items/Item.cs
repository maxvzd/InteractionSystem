using Constants;
using Items.ItemInterfaces;
using Items.Properties;
using UnityEngine;

namespace Items
{
    public class Item : BaseItem
    {
        [SerializeField] private ItemProperties itemProperties;
        [SerializeField] private ItemType type;
        
        public override IItemProperties ItemProperties => itemProperties;
        public override IInteractableProperties Properties => itemProperties;
    }
}