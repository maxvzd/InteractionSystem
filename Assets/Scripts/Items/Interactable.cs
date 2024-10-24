using Items.ItemInterfaces;
using Items.Properties;
using UnityEngine;

namespace Items
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractableProperties interactableProperties;
        public IInteractableProperties Properties => interactableProperties;
    }
}