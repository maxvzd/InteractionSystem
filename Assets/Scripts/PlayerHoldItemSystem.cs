using System.Collections;
using Constants;
using Items;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerHoldItemSystem : MonoBehaviour
{
    public bool IsHoldingItem => _isHoldingItem;

    [SerializeField] private Transform rightArmIkTarget;

    private Item _currentlyHeldItem;
    private bool _isHoldingItem;

    private OffsetPose _offsetPose;
    private float _holdWeight;
    private IEnumerator _holdWeightCoRoutine;
    private IEnumerator _placeItemCoRoutine;

    private Animator _animator;
    private FullBodyBipedIK _ik;
    private InteractionSystem _interactionSystem;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _ik = GetComponent<FullBodyBipedIK>();
        _interactionSystem = GetComponent<InteractionSystem>();
    }

    private void LateUpdate()
    {
        if (_offsetPose is not null)
        {
            //_offsetPose.Apply(_ik.solver, _holdWeight, _ik.transform.rotation);
        }
    }

    public void PickupItem(Transform currentlyHeldItem)
    {
        Transform pickedUpItem = currentlyHeldItem;
        Item item = pickedUpItem.gameObject.GetComponent<Item>();

        if (ReferenceEquals(item, null)) return;

        _ik.solver.rightHandEffector.target = null;

        if (_interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, item.InteractionObject, false))
        {
            
            _interactionSystem.OnInteractionStop += OnInteractionStop;
            
            _currentlyHeldItem = item;
            _offsetPose = _currentlyHeldItem.GetComponent<OffsetPose>();
            _isHoldingItem = true;
            _animator.SetBool(AnimatorConstants.IsHoldingItem, true);

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
        _animator.SetBool(Constants.AnimatorConstants.IsHoldingItem, false);

        _currentlyHeldItem.UnEquipItem();

        _isHoldingItem = false;
        StartHoldWeightCoRoutine(0);
    }

    public void PlaceItem(Vector3 position, float placeTime)
    {
        StartPlaceItemCoRoutine(position, placeTime);
    }

    public IEnumerator EquipWeapon()
    {
        //TODO hacky weird way to do this but idk what's changing the weight afterwards
        yield return new WaitForSeconds(1f);
        
        if (_currentlyHeldItem.IsEquippable)
        {
            _currentlyHeldItem.EquipItem(transform);
        }
    }
}