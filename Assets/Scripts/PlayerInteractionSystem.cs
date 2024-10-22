using Constants;
using Items;
using UI.HUD;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerInteractionSystem : MonoBehaviour
{
    //Serialised Components
    [SerializeField] private Camera mainCamera;
    [SerializeField] private UIDocument hudDoc;
    
    // Serialised Options
    [SerializeField] private float maxInteractDistance;
    [SerializeField] private float howLongButtonShouldBeHeld;
    
    private PlayerPickUpItemSystem _pickUpItemSystem;
    private PlayerOpenDoorSystem _playerOpenDoorSystem;
    private PlayerToHudCommunication _hud;

    private Transform _currentlyAimedAtTransform;
    private PlayerInput _playerInput;
    private InputAction _interactAction;

    private float _interactHeldTime;
    
    private void Start()
    {
        _pickUpItemSystem = GetComponent<PlayerPickUpItemSystem>();
        _playerOpenDoorSystem = GetComponent<PlayerOpenDoorSystem>();

        _hud = hudDoc.GetComponent<PlayerToHudCommunication>();
        hudDoc = null;

        _playerInput = GetComponent<PlayerInput>();
        _interactAction = _playerInput.actions[InputConstants.InteractAction];
    }

    private void Update()
    {
        bool shouldResetCrosshair = true;
        if (!_pickUpItemSystem.IsHoldingItem)
        {
            Vector3 origin = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
            Vector3 dir = mainCamera.transform.forward;
            Ray sphereRay = new Ray(origin, dir);
            //Debug.DrawRay(origin, dir * maxInteractDistance, Color.green);

            if (Physics.SphereCast(sphereRay, 0.1f, out RaycastHit hit, maxInteractDistance, ~LayerMask.GetMask(LayerConstants.LAYER_PLAYER)))
            {
                if (_currentlyAimedAtTransform != hit.transform)
                {
                    _currentlyAimedAtTransform = hit.transform;

                    if (_currentlyAimedAtTransform.gameObject.CompareTag(TagConstants.Interactable))
                    {
                        IInteractable interactable = hit.transform.GetComponent<IInteractable>();

                        if (interactable is not null)
                        {
                            if (interactable.Properties is not null)
                            {
                                _hud.ChangeCrossHair(interactable.Properties.InteractIcon);
                                _hud.ShowItemName(interactable.Properties.ItemName);
                                shouldResetCrosshair = false;
                            }
                        }
                    }
                }
                else
                {
                    shouldResetCrosshair = false;
                }
            }
        }

        if (shouldResetCrosshair)
        {
            _hud.ResetCrosshair();
            _hud.HideItemName();
        }

        if (_pickUpItemSystem.IsInInteraction) return;
        
        if (_interactAction.WasPressedThisFrame())
        {
            _interactHeldTime = 0f;
        }

        if (_interactAction.IsPressed())
        {
            if (_interactHeldTime < howLongButtonShouldBeHeld)
            {
                _interactHeldTime += Time.deltaTime;
            }
        }

        bool interactButtonWasPressed = false;
        bool interactButtonWasHeld = false;
        if (_interactAction.WasReleasedThisFrame())
        {
            interactButtonWasPressed = true;
            if (_interactHeldTime > howLongButtonShouldBeHeld)
            {
                interactButtonWasHeld = true;
            }
        }

        if (_pickUpItemSystem.IsHoldingItem && interactButtonWasHeld)
        {
            if (!_pickUpItemSystem.HasItemEquipped)
            {
                Vector3 origin = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
                Vector3 dir = mainCamera.transform.forward;
                Ray ray = new Ray(origin, dir);
                if (Physics.Raycast(ray, out RaycastHit hit, maxInteractDistance, LayerMask.GetMask(LayerConstants.LAYER_TERRAIN)))
                {
                    //TODO: Replace with dot product?
                    if (hit.normal == Vector3.up)
                    {
                        _pickUpItemSystem.PlaceItem(hit.point, 0.5f);
                        return;
                    }
                }
                _pickUpItemSystem.DropItem();
            }
            else
            {
                _pickUpItemSystem.UnEquipItem();
            }
            
        } 
        else if (interactButtonWasPressed)
        {
            if (_playerOpenDoorSystem.IsHoldingHandle)
            {
                _playerOpenDoorSystem.ReleaseHandle();
                return;
            }

            if (!_pickUpItemSystem.IsHoldingItem)
            {
                string layer = LayerMask.LayerToName(_currentlyAimedAtTransform.gameObject.layer);
                switch (layer)
                {
                    case LayerConstants.LAYER_GUN:
                    case LayerConstants.LAYER_ITEM:
                        _pickUpItemSystem.PickupItem(_currentlyAimedAtTransform);
                        break;
                    case LayerConstants.LAYER_DOOR:
                        _playerOpenDoorSystem.OpenDoor(_currentlyAimedAtTransform);
                        break;
                }
            }
            else
            {
                _pickUpItemSystem.EquipItem();
            }
        }
    }
}