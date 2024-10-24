using Constants;
using Items;
using Items.ItemInterfaces;
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

    private PlayerPickUpItemSystem _pickUpItemSystem;
    private PlayerOpenDoorSystem _playerOpenDoorSystem;
    private PlayerToHudCommunication _hud;

    //private Transform _currentlyAimedAtTransform;
    private PlayerInput _playerInput;
    private InputAction _interactAction;

    private HoldInputButton _heldChecker;

    private void Start()
    {
        _pickUpItemSystem = GetComponent<PlayerPickUpItemSystem>();
        _playerOpenDoorSystem = GetComponent<PlayerOpenDoorSystem>();

        _hud = hudDoc.GetComponent<PlayerToHudCommunication>();
        hudDoc = null;

        _playerInput = GetComponent<PlayerInput>();
        _interactAction = _playerInput.actions[InputConstants.InteractAction];
        _heldChecker = new HoldInputButton();
    }

    private void Update()
    {
        Transform interactableItem = null;
        if (!_pickUpItemSystem.IsHoldingItem)
        {
            Vector3 origin = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
            Vector3 dir = mainCamera.transform.forward;
            Ray sphereRay = new Ray(origin, dir);
            
            if (Physics.SphereCast(sphereRay, 0.1f, out RaycastHit hit, maxInteractDistance, ~LayerMask.GetMask(LayerConstants.LAYER_PLAYER)))
            {
                Debug.DrawRay(origin, dir * maxInteractDistance, Color.green);
                if (hit.transform.gameObject.CompareTag(TagConstants.InteractableTag))
                {
                    IInteractable interactable = hit.transform.GetComponent<IInteractable>();
                    if (interactable?.Properties is not null)
                    {
                        interactableItem = hit.transform;
                        _hud.ChangeCrossHair(interactable.Properties.InteractIcon);
                        _hud.ShowItemName(interactable.Properties.ItemName);
                    }
                }
            }
            else { Debug.DrawRay(origin, dir * maxInteractDistance, Color.red);}
        }

        if (interactableItem is null)
        {
            _hud.ResetCrosshair();
            _hud.HideItemName();
        }

        if (_pickUpItemSystem.IsInInteraction) return;

        HeldButtonDetails heldDetails = _heldChecker.CheckForButtonHold(_interactAction, Time.deltaTime);

        if (_pickUpItemSystem.IsHoldingItem && heldDetails.WasButtonHeld)
        {
            //if (!_pickUpItemSystem.HasItemEquipped)
            //{
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
            //}
            //else
            //{
            //_pickUpItemSystem.UnEquipItem();
            //}
        }
        else if (heldDetails.WasButtonPressed)
        {
            if (_playerOpenDoorSystem.IsHoldingHandle)
            {
                _playerOpenDoorSystem.ReleaseHandle();
                return;
            }

            if (!_pickUpItemSystem.IsHoldingItem)
            {
                if (interactableItem is null) return;
                
                string layer = LayerMask.LayerToName(interactableItem.gameObject.layer);
                switch (layer)
                {
                    case LayerConstants.LAYER_GUN:
                    case LayerConstants.LAYER_ITEM:
                        _pickUpItemSystem.PickupItem(interactableItem);
                        break;
                    case LayerConstants.LAYER_DOOR:
                        _playerOpenDoorSystem.OpenDoor(interactableItem);
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