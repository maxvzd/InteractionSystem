using System.Collections.Generic;
using Constants;
using Unity.VisualScripting;
using UnityEngine.UIElements;

namespace UI.Inventory.Controllers
{
    public class EquipmentPanelController
    {
        public EquipmentPanelController(VisualElement root)
        {
            VisualElement headSlot = root.Q<VisualElement>(InventoryUIConstants.HeadSlot);
            VisualElement torsoSlot = root.Q<VisualElement>(InventoryUIConstants.TorsoSlot);
            Button beltSlot = root.Q<Button>(InventoryUIConstants.BeltSlot);

            AddTabs(new List<string>()
            {
                "Headgear",
                "Mask"
            }, headSlot);
            AddTabs(new List<string>()
            {
                "Inner Torso",
                "Outer Torso",
                "Armour"
            }, torsoSlot);
        }

        private void AddTabs(List<string> tabs, VisualElement equipmentSlot)
        {
            TabView tabView = equipmentSlot.Q<TabView>(InventoryUIConstants.MainTabView);

            int i = 1;
            foreach(string tabTitle in tabs)
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