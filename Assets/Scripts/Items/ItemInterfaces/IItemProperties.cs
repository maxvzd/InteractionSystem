using System;
using Constants;
using Items.Properties;

namespace Items.ItemInterfaces
{
    public interface IItemProperties : IInteractableProperties
    {
        Guid PrefabId { get; }
        ItemType Type { get; }
        float Weight { get; }
        float Volume { get; }
    }
}