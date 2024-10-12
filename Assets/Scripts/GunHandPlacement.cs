using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class GunHandPlacement : MonoBehaviour
{
    [SerializeField] private Transform currentlyEquippedGun;
    [SerializeField] private Transform rightHandIkTarget;
    [SerializeField] private Transform leftHandIkTarget;
    [SerializeField] private Transform leftHandIkHint;
    [SerializeField] private Transform rightHandIkHint;
    
    private Vector3 _rightHandPos;
    private Quaternion _rightHandRot;
    
    private Vector3 _leftHandPos;
    private Quaternion _leftHandRot;
    private FullBodyBipedIK _fullBodyIk;
    private GunHandPositions _handPos;

    // Start is called before the first frame update
    public void Start()
    {
        _handPos = currentlyEquippedGun.GetComponent<GunHandPositions>();

        _fullBodyIk = GetComponentInChildren<FullBodyBipedIK>();

        _fullBodyIk.solver.rightHandEffector.target = rightHandIkTarget;
        _fullBodyIk.solver.rightHandEffector.positionWeight = 1;
        _fullBodyIk.solver.rightHandEffector.rotationWeight = 1;
        
        _fullBodyIk.solver.rightArmChain.bendConstraint.bendGoal = rightHandIkHint;
        _fullBodyIk.solver.rightArmChain.bendConstraint.weight = 1;
        
        _fullBodyIk.solver.leftHandEffector.target = leftHandIkTarget;
        _fullBodyIk.solver.leftHandEffector.positionWeight = 1;
        _fullBodyIk.solver.leftHandEffector.rotationWeight = 1;
        
        _fullBodyIk.solver.leftArmChain.bendConstraint.bendGoal = leftHandIkHint;
        _fullBodyIk.solver.leftArmChain.bendConstraint.weight = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        _rightHandPos = _handPos.RightHandIkTargetPosition;
        _rightHandRot = _handPos.RightHandIkTargetRotation;
        _leftHandPos = _handPos.LeftHandIkTargetPosition;
        _leftHandRot = _handPos.LeftHandIkTargetRotation;
        
        rightHandIkTarget.SetPositionAndRotation(_rightHandPos, _rightHandRot);
        leftHandIkTarget.SetPositionAndRotation(_leftHandPos, _leftHandRot);
    }
}
