using System;
using Constants;
using Items;
using UI.HUD;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerInteractionSystem : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float maxInteractDistance;
    private PlayerHoldItemSystem _playerHoldItemSystem;
    private PlayerOpenDoorSystem _playerOpenDoorSystem;
    private PlayerToHudCommunication _hud;
    [SerializeField] private UIDocument hudDoc;

    private Transform _currentlyAimedAtTransform;

    private void Start()
    {
        _playerHoldItemSystem = GetComponent<PlayerHoldItemSystem>();
        _playerOpenDoorSystem = GetComponent<PlayerOpenDoorSystem>();

        _hud = hudDoc.GetComponent<PlayerToHudCommunication>();
        hudDoc = null;
    }
    
    private void Update()
    {
        bool shouldResetCrosshair = true;
        if (!_playerHoldItemSystem.IsHoldingItem)
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
        }

        if (Input.GetButtonDown(InputConstants.UseKey))
        {
            if (_playerOpenDoorSystem.IsHoldingHandle)
            {
                _playerOpenDoorSystem.ReleaseHandle();
                return;
            }
            
            if (!_playerHoldItemSystem.IsHoldingItem)
            {
                string layer = LayerMask.LayerToName(_currentlyAimedAtTransform.gameObject.layer);
                switch (layer)
                {
                    case LayerConstants.LAYER_GUN:
                    case LayerConstants.LAYER_ITEM:
                        _playerHoldItemSystem.PickupItem(_currentlyAimedAtTransform);
                        break;
                    case LayerConstants.LAYER_DOOR:
                        _playerOpenDoorSystem.OpenDoor(_currentlyAimedAtTransform);
                        break;
                    default:
                        break;
                }
            }
            else 
            {
                Vector3 origin = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
                Vector3 dir = mainCamera.transform.forward;
                Ray ray = new Ray(origin, dir);
                if (Physics.Raycast(ray, out RaycastHit hit, maxInteractDistance, LayerMask.GetMask(LayerConstants.LAYER_TERRAIN)))
                {
                    if (hit.normal == Vector3.up)
                    {
                        _playerHoldItemSystem.PlaceItem(hit.point, 0.5f);
                        return;
                    }
                }
                _playerHoldItemSystem.DropItem();
            }
        }
    }
}