using UnityEngine;

namespace Items.UITemplates
{
    public interface IUIItemProperties
    {
        Texture2D InventoryIcon { get; }
        string Description { get; }
    }
}