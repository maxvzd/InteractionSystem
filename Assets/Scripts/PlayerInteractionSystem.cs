using System;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerInteractionSystem : MonoBehaviour
{
    [SerializeField] private Transform lookAtBase;

    private Transform _lookAtTarget;
    //private InteractionSystem _interactionSystem;
    private PlayerHoldItemSystem _playerHoldItemSystem;
    private PlayerOpenDoorSystem _playerOpenDoorSystem;
    
    private void Start()
    {
        int childCount = lookAtBase.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = lookAtBase.GetChild(i);
            if (child.name == "LookAtTarget")
            {
                _lookAtTarget = child;
                break;
            }
        }

        if (ReferenceEquals(_lookAtTarget, null)) throw new Exception("Couldn't find lookAtTarget");

        _playerHoldItemSystem = GetComponent<PlayerHoldItemSystem>();
        _playerOpenDoorSystem = GetComponent<PlayerOpenDoorSystem>();
    }

    private void Update()
    {
        if (Input.GetButtonDown(Constants.UseKey))
        {
            if (_playerOpenDoorSystem.IsHoldingHandle)
            {
                _playerOpenDoorSystem.RemoveHandFromHandle();
                return;
            }
            
            var basePosition = lookAtBase.position;
            var targetPosition = _lookAtTarget.position;

            Vector3 direction = targetPosition - basePosition;
            float distance = Vector3.Distance(targetPosition, basePosition);

            if (!_playerHoldItemSystem.IsHoldingItem)
            {
                Ray sphereRay = new Ray(basePosition, direction);
                if (Physics.SphereCast(sphereRay, 0.1f, out RaycastHit hit, distance, LayerMask.GetMask(Constants.LAYER_ITEM, Constants.LAYER_DOOR)))
                {
                    string layer = LayerMask.LayerToName(hit.transform.gameObject.layer);
                    
                    switch (layer)
                    {
                        case Constants.LAYER_ITEM:
                            _playerHoldItemSystem.PickupItem(hit.transform);
                            break;
                        case Constants.LAYER_DOOR:
                            _playerOpenDoorSystem.OpenDoor(hit.transform);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                Ray ray = new Ray(basePosition, direction);
                if (Physics.Raycast(ray, out RaycastHit hit, distance, LayerMask.GetMask("Terrain")))
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