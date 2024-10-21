using UnityEngine;
using UnityEngine.Serialization;

namespace Items.Properties
{
    [CreateAssetMenu(menuName = "ItemProperties/InteractableProperties")]
    public class InteractableProperties : ScriptableObject, IProperties
    {
        public Texture2D InteractIcon => interactIcon;
        [SerializeField] private Texture2D interactIcon;

        public string ItemName => itemName;
        [FormerlySerializedAs("objectName")] [SerializeField] private string itemName;
    }
}