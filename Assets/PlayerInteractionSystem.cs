using System;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerInteractionSystem : MonoBehaviour
{
    [SerializeField] private InteractionSystem interactionSystem;
    [SerializeField] private Transform lookAtBase;
    private Transform _lookAtTarget;
    private Transform _currentlyHeldItem;
    private bool _isHoldingItem;


    // Start is called before the first frame update
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
    }

    private void Update()
    {
        if (Input.GetButtonDown(Constants.UseKey))
        {
            if (!_isHoldingItem)
            {
                var basePosition = lookAtBase.position;
                var targetPosition = _lookAtTarget.position;

                Vector3 direction = targetPosition - basePosition;
                float distance = Vector3.Distance(targetPosition, basePosition);

                Ray sphereRay = new Ray(basePosition, direction);
                if (Physics.SphereCast(sphereRay, 0.1f, out RaycastHit hit, distance, LayerMask.GetMask("Item")))
                {
                    Transform pickedUpItem = hit.transform;
                    InteractionObject interactionObject = pickedUpItem.gameObject.GetComponent<InteractionObject>();

                    if (ReferenceEquals(interactionObject, null)) return;

                    _currentlyHeldItem = pickedUpItem;

                    interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, interactionObject, true);
                    _isHoldingItem = true;
                }
            }
            else
            {
                //interactionSystem.
                _currentlyHeldItem.transform.SetParent(null);
                _currentlyHeldItem.GetComponent<Rigidbody>().isKinematic = false;
                _isHoldingItem = false;
            }
        }
    }
}