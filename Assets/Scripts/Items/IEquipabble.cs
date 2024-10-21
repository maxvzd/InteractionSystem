using UnityEngine;

namespace Items
{
    public interface IEquipabble
    {
        void EquipItem(Transform player);
        void UnEquipItem();
    }
}