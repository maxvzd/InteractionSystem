using System.Collections;
using Items;
using Items.ItemInterfaces;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerPickUpItemSystem : MonoBehaviour
{
    public bool IsHoldingItem => !_currentlyHeldItem.IsEmpty;
    public bool IsInInteraction => _interactionSystem.IsInInteraction(FullBodyBipedEffector.RightHand);
    public bool IsItemEquippable => _currentlyHeldItem.IsEquippable;

    [SerializeField] private Transform rightArmIkTarget;

    private IItem _currentlyHeldItem;

    private float _holdWeight;
    private IEnumerator _holdWeightCoRoutine;
    private IEnumerator _placeItemCoRoutine;

    private FullBodyBipedIK _ik;
    private LookAtIK _lookAtIk;
    private InteractionSystem _interactionSystem;

    private Vector3 _headPosition;

    private Vector3 _heldPosition;
    private Quaternion _heldRotation;
    private Transform _heldSocket;

    private PlayerWearableEquipment _playerEquipmentSystem;
    private PlayerInventoryInteraction _playerInventory;

    private void Start()
    {
        _ik = GetComponent<FullBodyBipedIK>();
        _lookAtIk = GetComponent<LookAtIK>();

        _playerEquipmentSystem = GetComponent<PlayerWearableEquipment>();
        _playerInventory = GetComponent<PlayerInventoryInteraction>();

        _interactionSystem = GetComponent<InteractionSystem>();
        _interactionSystem.OnInteractionStop += OnInteractionStop;
        _currentlyHeldItem = new EmptyItem();
    }

    private void Update()
    {
        if (_currentlyHeldItem.IsEmpty) return;

        _lookAtIk.solver.head.UpdateSolverState();
        _headPosition = _lookAtIk.solver.head.solverPosition;
    }

    private void LateUpdate()
    {
        if (!_currentlyHeldItem.HasOffsetPose) return;

        _currentlyHeldItem.OffsetPose.Apply(_ik.solver, _holdWeight, transform.rotation, _headPosition);
    }

    public void PickupItem(Transform currentlyHeldItem)
    {
        Transform pickedUpItem = currentlyHeldItem;
        InteractionObject interactionObject = pickedUpItem.GetComponent<InteractionObject>();

        if (interactionObject is null) return;

        _ik.solver.rightHandEffector.target = null;

        if (_interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, interactionObject, false))
        {
            _currentlyHeldItem = currentlyHeldItem.GetComponent<IItem>();
            _currentlyHeldItem.DisablePhysics();

            StartHoldWeightCoRoutine(1);
        }
    }

    private void OnInteractionStop(FullBodyBipedEffector effectorType, InteractionObject interactionObject)
    {
        if (!_currentlyHeldItem.IsEmpty)
        {
            _heldPosition = _currentlyHeldItem.Transform.localPosition;
            _heldRotation = _currentlyHeldItem.Transform.localRotation;
            _heldSocket = _currentlyHeldItem.Transform.parent;
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
        if (_currentlyHeldItem.IsEmpty) return;

        _currentlyHeldItem.EnablePhysics();

        UnEquipItem();

        _currentlyHeldItem.Transform.SetParent(null);
        _currentlyHeldItem = new EmptyItem();

        _heldPosition = Vector3.zero;
        _heldRotation = Quaternion.identity;
        _heldSocket = null;

        ResetPoser();
        StartHoldWeightCoRoutine(0);
    }

    private void ResetPoser()
    {
        Poser rightHandPoser = _ik.solver.rightHandEffector.bone.GetComponent<Poser>();
        if (rightHandPoser is not null)
        {
            rightHandPoser.weight = 0f;
        }
    }

    public void PlaceItem(Vector3 position, float placeTime)
    {
        StartPlaceItemCoRoutine(position, placeTime);
    }

    public void EquipItem()
    {
        if (!_currentlyHeldItem.IsEquippable) return;

        IEquippable equippableItem = _currentlyHeldItem as IEquippable;
        if (_playerEquipmentSystem.EquipItem(equippableItem))
        {
            _currentlyHeldItem = new EmptyItem();
            ResetPoser();
        }
    }

    public void UnEquipItem()
    {
        // if (!_hasItemEquipped || _equipItem is null) return;
        //
        // //_hasItemEquipped = false;
        // _equipItem.UnEquipItem();
        //
        // _currentlyHeldItem.parent = _heldSocket;
        // _currentlyHeldItem.localPosition = _heldPosition;
        // _currentlyHeldItem.localRotation = _heldRotation;
    }

    public AddItemToBackpackResult TryAddItemToBackpack()
    {
        if (!IsHoldingItem) return AddItemToBackpackResult.NoItemInHand;
        
        AddItemToBackpackResult result = _playerInventory.AddItemToInventory(_currentlyHeldItem);
        if (result != AddItemToBackpackResult.Succeeded) return result;
        
        Destroy(_currentlyHeldItem.Transform.gameObject);
        _currentlyHeldItem = new EmptyItem();
        return AddItemToBackpackResult.Succeeded;
    }
}