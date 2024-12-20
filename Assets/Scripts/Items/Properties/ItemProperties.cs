﻿using System;
using Constants;
using Items.ItemInterfaces;
using UnityEngine;

namespace Items.Properties
{
    [CreateAssetMenu(menuName = "ItemProperties/ItemProperties")]
    public class ItemProperties : InteractableProperties, IItemProperties
    {
        [SerializeField] private string description;
        [SerializeField] private float weight;
        [SerializeField] private float volume;
        [SerializeField] private ItemType type;
        [SerializeField] private string prefabId;

        public string Description => description;
        public Guid PrefabId => Guid.Parse(prefabId);
        public ItemType Type => type;
        public float Weight => weight;
        public float Volume => volume;
    }
}