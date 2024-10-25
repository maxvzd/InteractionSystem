using Items.ItemInterfaces;
using Items.Properties;
using UnityEngine;

namespace Items
{
    public class Item : BaseItem
    {
        [SerializeField] private ItemProperties itemProperties;
        public override IItemProperties ItemProperties => itemProperties;
        public override IInteractableProperties Properties => itemProperties;
    }
}