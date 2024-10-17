using RootMotion.FinalIK;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private bool _isEquippable;
        public bool IsEquippable => _isEquippable;

        private InteractionObject _interactionObject;
        public InteractionObject InteractionObject => _interactionObject;

        private void Start()
        {
            _interactionObject = GetComponent<InteractionObject>();
        }

        public virtual void EquipItem(Transform player)
        {
            //Do nothing
        }
        
        public virtual void UnEquipItem()
        {
            //Do nothing
        }
    }
}