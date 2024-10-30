using System;
using Constants;
using Items.ItemInterfaces;
using UnityEngine;

namespace Items.UITemplates
{
    public interface IUIItemModel
    {
        string Name { get; }
        string Description { get; }
        Texture2D InventoryIcon  { get; }
        float Volume { get; }
        float Weight { get; }
        ItemType Type { get; }
        Guid ItemId { get; }
    } 
    
    public struct UIItemModel : IUIItemModel
    {

        public string Name { get; }
        public string Description { get; }
        public Texture2D InventoryIcon { get; }
        public float Volume { get; }
        public float Weight { get; }
        public ItemType Type { get; }
        public Guid ItemId { get; }

        public UIItemModel(IItem item)
        {
            Name = item.ItemProperties.ItemName;
            Description = item.UIProperties.Description;
            InventoryIcon = item.UIProperties.InventoryIcon;
            Volume = item.ItemProperties.Volume;
            Weight = item.ItemProperties.Weight;
            Type = item.ItemProperties.Type;
            ItemId = item.ItemId;
        }
    }

    public struct EmptyUIItemModel :IUIItemModel
    {
        public string Name => string.Empty;
        public string Description => string.Empty;
        public Texture2D InventoryIcon => Texture2D.blackTexture;
        public float Volume => 0f;
        public float Weight => 0f;
        public ItemType Type => ItemType.None;
        public Guid ItemId => Guid.Empty;
    }
}