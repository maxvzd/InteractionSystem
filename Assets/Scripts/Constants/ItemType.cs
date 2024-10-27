using System;
using System.Collections.Generic;

namespace Constants
{
    public enum ItemType
    {
        Rifle,
        Pistol,
        Blunt,
        Sharp,
        Axe,
        Headgear,
        OuterTorso,
        InnerTorso,
        Trousers,
        Belt,
        Socks,
        Gloves,
        Boots,
        Mask,
        Backpack,
        Liquid,
        Food,
        Junk,
        Book,
        Consumable
    }

    public enum ItemCategory
    {
        Weapon,
        Clothing,
        Consumables,
        Book,
        Misc,
        None
    }

    public static class ItemTypeCategory
    {
        private static readonly Dictionary<ItemType, ItemCategory> ItemToCategory = new()
        {
            {ItemType.Rifle, ItemCategory.Weapon},
            {ItemType.Pistol, ItemCategory.Weapon},
            {ItemType.Blunt, ItemCategory.Weapon},
            {ItemType.Sharp, ItemCategory.Weapon},
            {ItemType.Axe, ItemCategory.Weapon},
            {ItemType.Headgear, ItemCategory.Clothing},
            {ItemType.OuterTorso, ItemCategory.Clothing},
            {ItemType.InnerTorso, ItemCategory.Clothing},
            {ItemType.Trousers, ItemCategory.Clothing},
            {ItemType.Belt, ItemCategory.Clothing},
            {ItemType.Socks, ItemCategory.Clothing},
            {ItemType.Gloves, ItemCategory.Clothing},
            {ItemType.Boots, ItemCategory.Clothing},
            {ItemType.Mask, ItemCategory.Clothing},
            {ItemType.Backpack, ItemCategory.Clothing},
            {ItemType.Liquid, ItemCategory.Consumables},
            {ItemType.Food, ItemCategory.Consumables},
            {ItemType.Junk, ItemCategory.Misc},
            {ItemType.Book, ItemCategory.Book},
            {ItemType.Consumable, ItemCategory.Consumables},

        };

        public static ItemCategory GetCategory(ItemType itemType) => ItemToCategory.GetValueOrDefault(itemType, ItemCategory.None);
    }
}