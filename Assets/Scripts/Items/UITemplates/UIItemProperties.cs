using UnityEngine;

namespace Items.UITemplates
{
    [CreateAssetMenu]
    public class UIItemProperties : ScriptableObject, IUIItemProperties
    {
        [SerializeField] private Texture2D inventoryIcon;
        [SerializeField] private string description;


        public Texture2D InventoryIcon => inventoryIcon;
        public string Description => description;
    }
}