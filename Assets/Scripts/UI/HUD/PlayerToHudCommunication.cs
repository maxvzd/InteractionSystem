using UnityEngine;
using UnityEngine.UIElements;

namespace UI.HUD
{
    public class PlayerToHudCommunication : MonoBehaviour
    {
        private CrossHairManager _crosshairManager;
        private UIDocument _uiDocument;

        private void Start()
        {
            _crosshairManager = GetComponent<CrossHairManager>();
        }

        public void ChangeCrossHair(Texture2D crossHairFileName)
        {
            _crosshairManager.ChangeCrossHair(crossHairFileName);
        }

        public void ResetCrosshair()
        {
            _crosshairManager.ChangeCrosshairToDefault();
        }

        public void ShowItemName(string itemName)
        {
            _crosshairManager.ShowItemName(itemName);
        }

        public void HideItemName()
        {
            _crosshairManager.HideItemName();
        }
    }
}
