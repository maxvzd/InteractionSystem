using Constants;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.HUD
{
    public class CrosshairIconManager : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private VisualElement _crossHairElement;

        private bool _isCustomCrosshair;
        
        private void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            
            _crossHairElement = _uiDocument.rootVisualElement.Q<VisualElement>(UiConstants.CrossHair);
        }

        public void ChangeCrossHair(Texture2D texture)
        {
            StyleBackground background = _crossHairElement.style.backgroundImage;
            background.value = Background.FromTexture2D(texture);
            _crossHairElement.style.backgroundImage = background;
            _isCustomCrosshair = true;
        }

        public void ChangeCrosshairToDefault()
        {
            if (!_isCustomCrosshair) return;

            _crossHairElement.style.backgroundImage = null;
            _isCustomCrosshair = false;
        }
    }
}
