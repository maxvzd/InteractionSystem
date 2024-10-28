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
        WeaponsAndTools,
        Clothing,
        Consumables,
        Books,
        Misc,
        None
    }

    public static class ItemTypeCategory
    {
        private static readonly Dictionary<ItemType, ItemCategory> ItemToCategory = new()
        {
            {ItemType.Rifle, ItemCategory.WeaponsAndTools},
            {ItemType.Pistol, ItemCategory.WeaponsAndTools},
            {ItemType.Blunt, ItemCategory.WeaponsAndTools},
            {ItemType.Sharp, ItemCategory.WeaponsAndTools},
            {ItemType.Axe, ItemCategory.WeaponsAndTools},
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
            {ItemType.Book, ItemCategory.Books},
            {ItemType.Consumable, ItemCategory.Consumables},
        };

        public static ItemCategory GetCategory(ItemType itemType) => ItemToCategory.GetValueOrDefault(itemType, ItemCategory.None);
    }
}