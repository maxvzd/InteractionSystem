﻿using System;
using Constants;
using Items.ItemInterfaces;
using UnityEngine;

namespace Items.UITemplates
{
    public struct UIItemModel
    {
        public string Name { get; }
        public string Description { get; }
        public Texture2D InventoryIcon  { get; }
        public float Volume { get; }
        public float Weight { get; }
        public ItemType Type { get; }
        
        public UIItemModel(IItem item)
        {
            Name = item.ItemProperties.ItemName;
            Description = item.UIProperties.Description;
            InventoryIcon = item.UIProperties.InventoryIcon;
            Volume = item.ItemProperties.Volume;
            Weight = item.ItemProperties.Weight;
            Type = item.ItemProperties.Type;
        }
    }
}