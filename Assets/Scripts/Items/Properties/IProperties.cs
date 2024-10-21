using UnityEngine;

namespace Items.Properties
{
    public interface IProperties
    {
        Texture2D InteractIcon { get; }
        string ItemName { get; }
    }
}