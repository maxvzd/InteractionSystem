using Constants;
using Items.Properties;

namespace Items.ItemInterfaces
{
    public interface IItemProperties : IInteractableProperties
    {
        ItemType Type { get; }
        float Weight { get; }
        float Volume { get; }
    }
}