using Items.Properties;

namespace Items.ItemInterfaces
{
    public interface IItemProperties
    {
        string Description { get; }
        float Weight { get; }
        float Volume { get; }
    }
}