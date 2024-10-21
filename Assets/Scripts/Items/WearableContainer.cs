using Items.Properties;
using UnityEngine;

namespace Items
{
    public class WearableContainer : MonoBehaviour, IInteractable
    {
        [SerializeField] private WearableContainerProperties wearableContainerProperties;
        public IProperties Properties => wearableContainerProperties;
    }
}