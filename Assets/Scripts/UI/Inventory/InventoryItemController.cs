using Items.UITemplates;
using UnityEngine.UIElements;

namespace UI.Inventory
{
    public class InventoryItemController
    {
        private Label _itemNameLabel;
        private Label _itemVolumeLabel;
        private Label _itemWeightLabel;
        private Label _itemCategoryLabel;
        private VisualElement _itemImage;
        
        public void SetVisualElement(VisualElement visualElement)
        {
            _itemNameLabel = visualElement.Q<Label>("ItemNameLabel");
            _itemVolumeLabel = visualElement.Q<Label>("ItemVolumeLabel");
            _itemWeightLabel = visualElement.Q<Label>("ItemWeightLabel");
            _itemCategoryLabel = visualElement.Q<Label>("ItemCategoryLabel");
            _itemImage = visualElement.Q<VisualElement>("ItemImage");
        }

        public void SetModel(UIItemModel model)
        {
            _itemNameLabel.text = model.Name;
            _itemVolumeLabel.text = model.Volume.ToString("F");
            _itemWeightLabel.text = model.Weight.ToString("F");;
            _itemCategoryLabel.text = model.Type.ToString();
            _itemImage.style.backgroundImage = model.InventoryIcon;
        }
    }
}