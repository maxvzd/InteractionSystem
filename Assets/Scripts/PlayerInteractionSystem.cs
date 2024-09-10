using System;
using System.Collections;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerInteractionSystem : MonoBehaviour
{
    [SerializeField] private Transform lookAtBase;
    [SerializeField] private Transform rightArmIkTarget;

    private OffsetPose _offsetPose;
    private InteractionSystem _interactionSystem;
    private FullBodyBipedIK _ik;
    private Transform _lookAtTarget;
    private Transform _currentlyHeldItem;
    private bool _isHoldingItem;
    private float _holdWeight;
    private Animator _animator;


    private IEnumerator _holdWeightEnumerator;
    private IEnumerator _placeItemCoRoutine;


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

        _interactionSystem = GetComponent<InteractionSystem>();
        _ik = GetComponent<FullBodyBipedIK>();
        _animator = GetComponent<Animator>();
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

            if (!_isHoldingItem)
            {
                if (Physics.SphereCast(sphereRay, 0.1f, out RaycastHit hit, distance, LayerMask.GetMask("Item")))
                {
                    Transform pickedUpItem = hit.transform;
                    InteractionObject interactionObject = pickedUpItem.gameObject.GetComponent<InteractionObject>();

                    if (ReferenceEquals(interactionObject, null)) return;

                    _interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, interactionObject, true);

                    _currentlyHeldItem = pickedUpItem;
                    _offsetPose = pickedUpItem.GetComponent<OffsetPose>();
                    _isHoldingItem = true;
                    _animator.SetBool(Constants.IsHoldingItem, true);
                    StartHoldWeightCoRoutine(1);
                }
            }
            else
            {
                Ray ray = new Ray(basePosition, direction);
                if (Physics.Raycast(ray, out RaycastHit hit, distance, LayerMask.GetMask("Terrain")))
                {
                    if (hit.normal == Vector3.up)
                    {
                        StartPlaceItemCoRoutine(hit.point, 0.5f);
                        return;
                    }
                }
                DropItem();
            }
        }
    }

    private void StartPlaceItemCoRoutine(Vector3 position, float timeInSeconds)
    {
        if (_placeItemCoRoutine is not null)
        {
            StopCoroutine(_placeItemCoRoutine);
        }

        _placeItemCoRoutine = PlaceItem(position, timeInSeconds);
        StartCoroutine(_placeItemCoRoutine);
    }

    private IEnumerator PlaceItem(Vector3 position, float timeInSeconds)
    {
        while (_interactionSystem.IsInInteraction(FullBodyBipedEffector.RightHand))
        {
            yield return new WaitForEndOfFrame();
        }

        rightArmIkTarget.transform.position = position + Vector3.up * 0.1f;
        _ik.solver.rightHandEffector.target = rightArmIkTarget;

        while (_ik.solver.rightHandEffector.positionWeight < 1)
        {
            float deltaTime = Time.deltaTime * 1 / timeInSeconds;

            _ik.solver.rightHandEffector.positionWeight += deltaTime;

            yield return new WaitForEndOfFrame();
        }

        _ik.solver.rightHandEffector.positionWeight = 1;

        DropItem();

        while (_ik.solver.rightHandEffector.positionWeight > 0)
        {
            float deltaTime = Time.deltaTime * 1 / timeInSeconds;

            _ik.solver.rightHandEffector.positionWeight -= deltaTime;

            yield return new WaitForEndOfFrame();
        }

        _ik.solver.rightHandEffector.positionWeight = 0;
        _ik.solver.rightHandEffector.target = null;
    }

    private void DropItem()
    {
        _currentlyHeldItem.transform.SetParent(null);
        _currentlyHeldItem.GetComponent<Rigidbody>().isKinematic = false;
        _isHoldingItem = false;
        _animator.SetBool(Constants.IsHoldingItem, false);
        StartHoldWeightCoRoutine(0);
    }

    private void StartHoldWeightCoRoutine(float weightToUpdateTo)
    {
        if (!ReferenceEquals(_holdWeightEnumerator, null))
        {
            StopCoroutine(_holdWeightEnumerator);
        }

        _holdWeightEnumerator = UpdateHoldWeight(weightToUpdateTo, 0.5f);
        StartCoroutine(_holdWeightEnumerator);
    }

    private IEnumerator UpdateHoldWeight(float weightToUpdateTo, float timeInSeconds)
    {
        if (_holdWeight < weightToUpdateTo)
        {
            while (_holdWeight < weightToUpdateTo)
            {
                _holdWeight += Time.deltaTime * 1 / timeInSeconds;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (_holdWeight > weightToUpdateTo)
            {
                _holdWeight -= Time.deltaTime * 1 / timeInSeconds;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void LateUpdate()
    {
        if (_isHoldingItem)
        {
            _offsetPose.Apply(_ik.solver, _holdWeight, _ik.transform.rotation);
        }
        //_ik.solver.rightHandEffector.positionOffset += cameraTransform.rotation * holdOffset * _holdWeight;
    }
}