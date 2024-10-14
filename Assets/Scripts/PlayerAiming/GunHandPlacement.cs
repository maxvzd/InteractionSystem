using RootMotion.FinalIK;
using UnityEngine;

namespace PlayerAiming
{
    public class GunHandPlacement : MonoBehaviour
    {
        [SerializeField] private Transform rightHandIkTarget;
        [SerializeField] private Transform leftHandIkTarget;
        [SerializeField] private Transform leftHandIkHint;
        [SerializeField] private Transform rightHandIkHint;
    
        private Vector3 _rightHandPos;
        private Quaternion _rightHandRot;
    
        private Vector3 _leftHandPos;
        private Quaternion _leftHandRot;
        private FullBodyBipedIK _fullBodyIk;
        private GunPositionData _handPos;

    
        public void Awake()
        {
            _fullBodyIk = GetComponent<FullBodyBipedIK>();
        }

        public void EquipGun(GunPositionData posData)
        {
            _handPos = posData;
        
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
        
            rightHandIkTarget.SetParent(_handPos.RightHandIkTarget);
            rightHandIkTarget.localPosition = Vector3.zero;
            rightHandIkTarget.localEulerAngles = Vector3.zero;
        
            leftHandIkTarget.SetParent(_handPos.LeftHandIkTarget);
            leftHandIkTarget.localPosition = Vector3.zero;
            leftHandIkTarget.localEulerAngles = Vector3.zero;
        }

        public void UnEquipGun()
        {
            _fullBodyIk.solver.rightHandEffector.target = rightHandIkTarget;
            _fullBodyIk.solver.rightHandEffector.positionWeight = 0;
            _fullBodyIk.solver.rightHandEffector.rotationWeight = 0;
        
            _fullBodyIk.solver.rightArmChain.bendConstraint.bendGoal = rightHandIkHint;
            _fullBodyIk.solver.rightArmChain.bendConstraint.weight = 0;
        
            _fullBodyIk.solver.leftHandEffector.target = leftHandIkTarget;
            _fullBodyIk.solver.leftHandEffector.positionWeight = 0;
            _fullBodyIk.solver.leftHandEffector.rotationWeight = 0;
        
            _fullBodyIk.solver.leftArmChain.bendConstraint.bendGoal = leftHandIkHint;
            _fullBodyIk.solver.leftArmChain.bendConstraint.weight = 0;
            rightHandIkTarget.SetParent(null);
            leftHandIkTarget.SetParent(null);
        }
    }
}
