using System.Collections;
using Items;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerPickUpItemSystem : MonoBehaviour
{
    public bool IsHoldingItem => _isHoldingItem;
    public bool HasItemEquipped => _hasItemEquipped;
    public bool IsInInteraction => _interactionSystem.IsInInteraction(FullBodyBipedEffector.RightHand);

    [SerializeField] private Transform rightArmIkTarget;

    private Transform _currentlyHeldItem;
    private bool _isHoldingItem;
    private bool _hasItemEquipped;
    private IPhysicsItem _physicsItem;

    private OffsetPose _offsetPose;
    private float _holdWeight;
    private IEnumerator _holdWeightCoRoutine;
    private IEnumerator _placeItemCoRoutine;

    private FullBodyBipedIK _ik;
    private LookAtIK _lookAtIk;
    private InteractionSystem _interactionSystem;

    private Vector3 _headPosition;
    private IEquipabble _equipItem;

    private Vector3 _heldPosition;
    private Quaternion _heldRotation;
    private Transform _heldSocket;

    private void Start()
    {
        _ik = GetComponent<FullBodyBipedIK>();
        _lookAtIk = GetComponent<LookAtIK>();
        _interactionSystem = GetComponent<InteractionSystem>();
        _interactionSystem.OnInteractionStop += OnInteractionStop;
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
            _currentlyHeldItem = currentlyHeldItem;
            _equipItem = _currentlyHeldItem.GetComponent<IEquipabble>();
            _offsetPose = currentlyHeldItem.GetComponent<OffsetPose>();
            _physicsItem = currentlyHeldItem.GetComponent<IPhysicsItem>();
            
            if (_physicsItem is not null)
            {
                _physicsItem.DisablePhysics();
            }
            
            _isHoldingItem = true;
            StartHoldWeightCoRoutine(1);
        }
    }

    private void OnInteractionStop(FullBodyBipedEffector effectorType, InteractionObject interactionObject)
    {
        if (_currentlyHeldItem is not null)
        {
            _heldPosition = _currentlyHeldItem.localPosition;
            _heldRotation = _currentlyHeldItem.localRotation;
            _heldSocket = _currentlyHeldItem.parent;
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
        if (_physicsItem is not null)
        {
            _physicsItem.EnablePhysics();
        }
        
        _currentlyHeldItem.transform.SetParent(null);
        
        _heldPosition = Vector3.zero;
        _heldRotation = Quaternion.identity;
        _heldSocket = null;
        
        Poser rightHandPoser = _ik.solver.rightHandEffector.bone.GetComponent<Poser>();
        if (rightHandPoser is not null)
        {
            rightHandPoser.weight = 0f;
        }

        if (_equipItem is not null && _equipItem.IsEquipped)
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

    public void EquipItem()
    {
        if (_equipItem is null || _hasItemEquipped) return;
        
        _hasItemEquipped = true;
        _equipItem.EquipItem(transform);
    }

    public void UnEquipItem()
    {
        if (!_hasItemEquipped || _equipItem is null) return;

        _hasItemEquipped = false;
        _equipItem.UnEquipItem();

        _currentlyHeldItem.parent = _heldSocket;
        _currentlyHeldItem.localPosition = _heldPosition;
        _currentlyHeldItem.localRotation = _heldRotation;
    }
}