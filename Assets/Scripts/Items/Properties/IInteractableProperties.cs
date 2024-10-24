using UnityEngine;

namespace Items.Properties
{
    public interface IInteractableProperties
    {
        Texture2D InteractIcon { get; }
        string ItemName { get; }
    }
}