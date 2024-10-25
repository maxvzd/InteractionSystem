using Constants;
using Items.ItemInterfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.HUD
{
    public class CrossHairManager : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private VisualElement _crossHairElement;
        private Label _itemNameLabel;

        private void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            
            _crossHairElement = _uiDocument.rootVisualElement.Q<VisualElement>(UiConstants.CrossHair);
            _itemNameLabel = _uiDocument.rootVisualElement.Q<Label>(UiConstants.ItemNameLabel);
        }

        private void ChangeCrossHair(Texture2D texture)
        {
            StyleBackground background = _crossHairElement.style.backgroundImage;
            background.value = Background.FromTexture2D(texture);
            _crossHairElement.style.backgroundImage = background;
        }

        public void ShowCrossHair(IInteractable itemToShow)
        {
            _itemNameLabel.text = itemToShow.Properties.ItemName;
            ChangeCrossHair(itemToShow.Properties.InteractIcon);
            _uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            _uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        }
    }
}
