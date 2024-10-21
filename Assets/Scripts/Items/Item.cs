using Items.Properties;
using RootMotion.FinalIK;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour, IInteractable
    {
        private InteractionObject _interactionObject;
        public InteractionObject InteractionObject => _interactionObject;
        
        public ItemProperties ItemProperties => itemProperties;
        [SerializeField] private ItemProperties itemProperties;
        public IProperties Properties => itemProperties;

        private void Start()
        {
            _interactionObject = GetComponent<InteractionObject>();
        }

    }
}