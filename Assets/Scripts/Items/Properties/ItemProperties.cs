using UnityEngine;

namespace Items.Properties
{
    [CreateAssetMenu]
    public class ItemProperties : InteractableProperties
    {
        [SerializeField] private string description;
        [SerializeField] private string weight;
        [SerializeField] private string volume;
    }
}