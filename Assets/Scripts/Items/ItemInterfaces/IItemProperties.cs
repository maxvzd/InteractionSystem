using Items.Properties;

namespace Items.ItemInterfaces
{
    public interface IItemProperties : IInteractableProperties
    {
        float Weight { get; }
        float Volume { get; }
    }
}