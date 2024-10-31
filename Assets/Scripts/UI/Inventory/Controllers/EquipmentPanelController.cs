using System;
using System.Collections.Generic;
using Constants;
using Items.UITemplates;
using UnityEngine.UIElements;

namespace UI.Inventory.Controllers
{
    public class EquipmentPanelController
    {
        public EventHandler<Guid> ItemUnequipped;
        private readonly Button _backpackSlot;
        private IUIItemModel _backpackModel;

        public EquipmentPanelController(VisualElement root)
        {
            VisualElement headSlot = root.Q<VisualElement>(InventoryUIConstants.HeadSlot);
            VisualElement torsoSlot = root.Q<VisualElement>(InventoryUIConstants.TorsoSlot);

            _backpackSlot = root.Q<Button>(InventoryUIConstants.BackpackSlot);
            _backpackSlot.clicked += BackpackSlotOnClicked;

            AddTabs(new List<string>
            {
                "Headgear",
                "Mask"
            }, headSlot);

            AddTabs(new List<string>
            {
                "Inner Torso",
                "Outer Torso",
                "Armour"
            }, torsoSlot);
        }

        private void BackpackSlotOnClicked()
        {
            ItemUnequipped?.Invoke(this, _backpackModel.ItemId);
            _backpackModel = new EmptyUIItemModel();
            _backpackSlot.iconImage = null;
            _backpackSlot.text = "Backpack";
        }

        public void UpdateModel(PlayerEquipmentSlots equipmentSlots)
        {
            if (equipmentSlots.Backpack is not null)
            {
                _backpackModel = new UIItemModel(equipmentSlots.Backpack);
                _backpackSlot.iconImage = _backpackModel.InventoryIcon;
                _backpackSlot.text = string.Empty;
            }
        }

        private void AddTabs(List<string> tabs, VisualElement equipmentSlot)
        {
            TabView tabView = equipmentSlot.Q<TabView>(InventoryUIConstants.MainTabView);

            int i = 1;
            foreach (string tabTitle in tabs)
            {
                Tab tab = new Tab(i.ToString());

                Button button = new Button
                {
                    text = tabTitle
                };
                button.AddToClassList("equipment-slot-button");
                tab.contentContainer.Add(button);
                tabView.Add(tab);
                i++;
            }
        }
    }
}