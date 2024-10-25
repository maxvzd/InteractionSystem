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
    private CrossHairManager _crossHair;

    private PlayerInput _playerInput;
    private InputAction _interactAction;
    private InputAction _longInteractAction;

    private void Start()
    {
        _pickUpItemSystem = GetComponent<PlayerPickUpItemSystem>();
        _playerOpenDoorSystem = GetComponent<PlayerOpenDoorSystem>();

        _crossHair = hudDoc.GetComponent<CrossHairManager>();
        hudDoc = null;

        _playerInput = GetComponent<PlayerInput>();
        _interactAction = _playerInput.actions[InputConstants.InteractAction];
        _longInteractAction = _playerInput.actions[InputConstants.LongPressInteractAction];
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
                //Debug.DrawRay(origin, dir * maxInteractDistance, Color.green);
                if (hit.transform.gameObject.CompareTag(TagConstants.InteractableTag))
                {
                    IInteractable interactable = hit.transform.GetComponent<IInteractable>();
                    if (interactable?.Properties is not null)
                    {
                        interactableItem = hit.transform;
                        _crossHair.ShowCrossHair(interactable);
                    }
                }
            }
            // else
            // {
            //     Debug.DrawRay(origin, dir * maxInteractDistance, Color.red);
            // }
        }

        if (interactableItem is null)
        {
            _crossHair.Hide();
        }

        if (_pickUpItemSystem.IsInInteraction) return;

        if (_pickUpItemSystem.IsHoldingItem)
        {
            if (_longInteractAction.WasPerformedThisFrame())
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
            else if (_interactAction.WasPerformedThisFrame())
            {
                AddItemToBackpackResult addItemToBackpackResult = _pickUpItemSystem.TryAddItemToBackpack();
                if (addItemToBackpackResult == AddItemToBackpackResult.BackpackIsNotOut)
                {
                    _pickUpItemSystem.EquipItem();
                }
            }
        } 
        else if (_interactAction.WasPerformedThisFrame())
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
        }
    }
}

public enum AddItemToBackpackResult
{
    TooMuchWeight,
    TooMuchVolume,
    NoBackpackEquipped,
    BackpackIsNotOut,
    NoItemInHand,
    Succeeded
}