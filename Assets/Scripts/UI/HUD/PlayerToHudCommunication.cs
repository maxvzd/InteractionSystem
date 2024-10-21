using UnityEngine;
using UnityEngine.UIElements;

namespace UI.HUD
{
    public class PlayerToHudCommunication : MonoBehaviour
    {
        private CrosshairIconManager _crosshairManager;
        private UIDocument _uiDocument;

        private void Start()
        {
            _crosshairManager = GetComponent<CrosshairIconManager>();
        }

        public void ChangeCrossHair(Texture2D crossHairFileName)
        {
            _crosshairManager.ChangeCrossHair(crossHairFileName);
        }

        public void ResetCrosshair()
        {
            _crosshairManager.ChangeCrosshairToDefault();
        }
    }
}
