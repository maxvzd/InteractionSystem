using System;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerInteractionSystem : MonoBehaviour
{
    [SerializeField] private Transform lookAtBase;

    private Transform _lookAtTarget;
    private InteractionSystem _interactionSystem;
    private PlayerHoldItemSystem _playerHoldItemSystem;
    
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
        _interactionSystem = GetComponent<InteractionSystem>();
    }

    private void Update()
    {
        if (Input.GetButtonDown(Constants.UseKey))
        {
            var basePosition = lookAtBase.position;
            var targetPosition = _lookAtTarget.position;

            Vector3 direction = targetPosition - basePosition;
            float distance = Vector3.Distance(targetPosition, basePosition);
            Ray sphereRay = new Ray(basePosition, direction);

            if (!_playerHoldItemSystem.IsHoldingItem)
            {
                if (Physics.SphereCast(sphereRay, 0.1f, out RaycastHit hit, distance, LayerMask.GetMask("Item")))
                {
                    Transform pickedUpItem = hit.transform;
                    InteractionObject interactionObject = pickedUpItem.gameObject.GetComponent<InteractionObject>();

                    if (ReferenceEquals(interactionObject, null)) return;

                    _interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, interactionObject, true);
                    _playerHoldItemSystem.SetCurrentlyHeldItem(pickedUpItem);
                }
            }
            else
            {
                Ray ray = new Ray(basePosition, direction);
                if (Physics.Raycast(ray, out RaycastHit hit, distance, LayerMask.GetMask("Terrain")))
                {
                    if (hit.normal == Vector3.up)
                    {
                        //StartPlaceItemCoRoutine(hit.point, 0.5f);
                        _playerHoldItemSystem.PlaceItem(hit.point, 0.5f);
                        return;
                    }
                }

                _playerHoldItemSystem.DropItem();
            }
        }
    }
}