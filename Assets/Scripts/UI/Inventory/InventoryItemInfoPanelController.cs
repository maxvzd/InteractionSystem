using Constants;
using Items.ItemInterfaces;
using Items.UITemplates;
using UnityEngine.UIElements;

namespace UI.Inventory
{
    public class InventoryItemInfoPanelController
    {
        private readonly Label _itemTitleLabel;
        private readonly Label _itemDescriptionLabel;
        private readonly Label _itemWeightLabel;
        private readonly Label _itemVolumeLabel;

        public InventoryItemInfoPanelController(VisualElement root)
        {
            _itemTitleLabel = root.Q<Label>(InventoryUIConstants.ItemTitle);
            _itemDescriptionLabel = root.Q<Label>(InventoryUIConstants.ItemDescription);
            _itemWeightLabel = root.Q<Label>(InventoryUIConstants.ItemWeight);
            _itemVolumeLabel = root.Q<Label>(InventoryUIConstants.ItemVolume);

            _itemTitleLabel.text = string.Empty;
            _itemDescriptionLabel.text = string.Empty;
            _itemWeightLabel.text = string.Empty;
            _itemVolumeLabel.text = string.Empty;
        }

        public void SetItem(UIItemModel item)
        {
            _itemTitleLabel.text = item.Name;
            _itemDescriptionLabel.text = item.Description;
            _itemWeightLabel.text = $"Weight: {item.Weight:F}";
            _itemVolumeLabel.text = $"Volume: {item.Volume:F}";
        }
    }
}