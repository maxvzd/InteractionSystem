using System;
using System.Collections;
using Constants;
using Items;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHoldItemSystem : MonoBehaviour
{
    public bool IsHoldingItem => _isHoldingItem;

    [SerializeField] private Transform rightArmIkTarget;

    private Transform _currentlyHeldItem;
    private bool _isHoldingItem;

    private OffsetPose _offsetPose;
    private float _holdWeight;
    private IEnumerator _holdWeightCoRoutine;
    private IEnumerator _placeItemCoRoutine;

    private FullBodyBipedIK _ik;
    private LookAtIK _lookAtIk;
    private InteractionSystem _interactionSystem;

    private Vector3 _headPosition;
    private IEquipabble _equipItem;

    private void Start()
    {
        _ik = GetComponent<FullBodyBipedIK>();
        _lookAtIk = GetComponent<LookAtIK>();
        _interactionSystem = GetComponent<InteractionSystem>();
    }

    private void Update()
    {
        if (_offsetPose is null) return;
        _lookAtIk.solver.head.UpdateSolverState();
        _headPosition = _lookAtIk.solver.head.solverPosition;
    }

    private void LateUpdate()
    {
        if (_offsetPose is null) return;
        
        _offsetPose.Apply(_ik.solver, _holdWeight, transform.rotation, _headPosition);
    }

    public void PickupItem(Transform currentlyHeldItem)
    {
        Transform pickedUpItem = currentlyHeldItem;
        InteractionObject interactionObject = pickedUpItem.gameObject.GetComponent<InteractionObject>();

        if (interactionObject is null) return;

        _ik.solver.rightHandEffector.target = null;

        if (_interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, interactionObject, false))
        {
            _interactionSystem.OnInteractionStop += OnInteractionStop;

            _currentlyHeldItem = currentlyHeldItem;
            _equipItem = _currentlyHeldItem.GetComponent<IEquipabble>();
            
            _offsetPose = currentlyHeldItem.GetComponent<OffsetPose>();
            _isHoldingItem = true;

            StartHoldWeightCoRoutine(1);
        }
    }

    private void OnInteractionStop(FullBodyBipedEffector effectortype, InteractionObject interactionobject)
    {
        if (effectortype == FullBodyBipedEffector.RightHand)
        {
            StartCoroutine(EquipWeapon());
            _interactionSystem.OnInteractionStop -= OnInteractionStop;
        }
    }

    #region Start Coroutines

    private void StartPlaceItemCoRoutine(Vector3 position, float timeInSeconds)
    {
        if (_placeItemCoRoutine is not null)
        {
            StopCoroutine(_placeItemCoRoutine);
        }

        _placeItemCoRoutine = PlaceItemRoutine(position, timeInSeconds);
        StartCoroutine(_placeItemCoRoutine);
    }

    private void StartHoldWeightCoRoutine(float weightToUpdateTo)
    {
        if (!ReferenceEquals(_holdWeightCoRoutine, null))
        {
            StopCoroutine(_holdWeightCoRoutine);
        }

        _holdWeightCoRoutine = UpdateHoldWeight(weightToUpdateTo, 0.5f);
        StartCoroutine(_holdWeightCoRoutine);
    }

    #endregion

    #region Coroutines

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

    private IEnumerator PlaceItemRoutine(Vector3 position, float timeInSeconds)
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

    #endregion

    public void DropItem()
    {
        _currentlyHeldItem.transform.SetParent(null);
        _currentlyHeldItem.GetComponent<Rigidbody>().isKinematic = false;
        Poser rightHandPoser = _ik.solver.rightHandEffector.bone.GetComponent<Poser>();
        if (rightHandPoser is not null)
        {
            rightHandPoser.weight = 0f;
        }

        if (_equipItem is not null)
        {
            _equipItem.UnEquipItem();
        }
        _offsetPose = null;

        _isHoldingItem = false;
        StartHoldWeightCoRoutine(0);
    }

    public void PlaceItem(Vector3 position, float placeTime)
    {
        StartPlaceItemCoRoutine(position, placeTime);
    }

    private IEnumerator EquipWeapon()
    {
        //TODO hacky weird way to do this but idk what's changing the weight afterwards
        yield return new WaitForSeconds(1f);

        if (_equipItem is not null)
        {
            _equipItem.EquipItem(transform);
        }
    }
}