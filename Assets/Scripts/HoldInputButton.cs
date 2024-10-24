using UnityEngine.InputSystem;

namespace Constants
{
    public class HoldInputButton
    {
        private const float BUTTON_HOLD_TIME = 0.5f;
        private float _buttonHeldTime;
        
        public HeldButtonDetails CheckForButtonHold(InputAction inputAction, float deltaTime)
        {
            if (inputAction.WasPressedThisFrame())
            {
                _buttonHeldTime = 0f;
            }

            if (inputAction.IsPressed())
            {
                if (_buttonHeldTime < BUTTON_HOLD_TIME)
                {
                    _buttonHeldTime += deltaTime;
                }
            }


            bool interactButtonWasPressed = false;
            bool interactButtonWasHeld = false;
            if (inputAction.WasReleasedThisFrame())
            {
                interactButtonWasPressed = true;
                if (_buttonHeldTime > BUTTON_HOLD_TIME)
                {
                    interactButtonWasHeld = true;
                }

            }

            return new HeldButtonDetails(interactButtonWasPressed, interactButtonWasHeld);
        }
    }
}

public struct HeldButtonDetails
{
    public HeldButtonDetails(bool wasButtonPressed, bool wasButtonHeld)
    {
        WasButtonPressed = wasButtonPressed;
        WasButtonHeld = wasButtonHeld;
    }
    
    public bool WasButtonPressed { get; }
    public bool WasButtonHeld { get; }
}