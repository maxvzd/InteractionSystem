using Constants;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.HUD
{
    public class CrossHairManager : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private VisualElement _crossHairElement;

        private bool _isCustomCrosshair;
        private Label _itemNameLabel;

        private void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            
            _crossHairElement = _uiDocument.rootVisualElement.Q<VisualElement>(UiConstants.CrossHair);
            _itemNameLabel = _uiDocument.rootVisualElement.Q<Label>(UiConstants.ItemNameLabel);
            _itemNameLabel.visible = false;
            //_itemNameLabel.style.borderLeftWidth = 0f;
        }

        public void ChangeCrossHair(Texture2D texture)
        {
            StyleBackground background = _crossHairElement.style.backgroundImage;
            background.value = Background.FromTexture2D(texture);
            _crossHairElement.style.backgroundImage = background;
            _isCustomCrosshair = true;
        }

        public void ShowItemName(string itemName)
        {
            _itemNameLabel.text = itemName;
            _itemNameLabel.visible = true;
        }

        public void HideItemName()
        {
            _itemNameLabel.text = string.Empty;
            _itemNameLabel.visible = false;
        }

        public void ChangeCrosshairToDefault()
        {
            if (!_isCustomCrosshair) return;

            _crossHairElement.style.backgroundImage = null;
            _isCustomCrosshair = false;
        }
    }
}
