using Items.ItemInterfaces;
using Items.Properties;
using UnityEngine;

namespace Items
{
    public class Item : BaseItem
    {
        public override IItemProperties ItemProperties => itemProperties;
        public override IInteractableProperties Properties => itemProperties;
        
        [SerializeField] private ItemProperties itemProperties;
        
        public override void RestoreProperties(IItem item) { }
    }
}