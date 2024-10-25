using Items.UITemplates;
using UnityEngine.UIElements;

namespace UI.Inventory
{
    public class InventoryItemController
    {
        private Label _itemName;
        private VisualElement _itemImage;
        
        public void SetVisualElement(VisualElement visualElement)
        {
            _itemName = visualElement.Q<Label>("ItemName");
            _itemImage = visualElement.Q<VisualElement>("ItemImage");
        }

        public void SetModel(UIItemModel model)
        {
            _itemName.text = model.Name;
            _itemImage.style.backgroundImage = model.InventoryIcon;
        }
    }
}