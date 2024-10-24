using UnityEngine;

namespace Items.ItemInterfaces
{
    public interface IWearableContainer : IEquippable, IContainer
    {
        Vector3 BackpackOutPositionOffset { get; }
        Vector3 BackpackOutRotationOffset { get; }
    }
}