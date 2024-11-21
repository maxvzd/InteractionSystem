using RootMotion.FinalIK;
using UnityEngine;

namespace GunStuff
{
    public class IKHandPlacement : MonoBehaviour
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
        private GunComponentsPositionData _handPos;

        private float _weight;
        private bool _increaseWeight;
        private bool _decreaseWeight;
        private float _lerpFactor;
        
        private void Awake()
        {
            _fullBodyIk = GetComponent<FullBodyBipedIK>();
        }

        private void Update()
        {
            if (_increaseWeight)
            {
                IncreaseIkWeight(Time.deltaTime);
            }
            else if (_decreaseWeight)
            {
                DecreaseIkWeight(Time.deltaTime);
            }
        }

        public void EquipGun(GunComponentsPositionData posData)
        {
            _handPos = posData;
        
            _fullBodyIk.solver.rightHandEffector.target = rightHandIkTarget;
            _fullBodyIk.solver.rightArmChain.bendConstraint.bendGoal = rightHandIkHint;
            _fullBodyIk.solver.leftHandEffector.target = leftHandIkTarget;
            _fullBodyIk.solver.leftArmChain.bendConstraint.bendGoal = leftHandIkHint;
        
            rightHandIkTarget.SetParent(_handPos.RightHandIkTarget);
            rightHandIkTarget.localPosition = Vector3.zero;
            rightHandIkTarget.localEulerAngles = Vector3.zero;
        
            leftHandIkTarget.SetParent(_handPos.LeftHandIkTarget);
            leftHandIkTarget.localPosition = Vector3.zero;
            leftHandIkTarget.localEulerAngles = Vector3.zero;

            IncreaseIkWeight();
        }

        private void IncreaseIkWeight(float deltaTime)
        {
            _weight += deltaTime * _lerpFactor;
            
            if (_weight > 1)
            {
                _increaseWeight = false;
            }
            
            SetWeight(_weight);
        }

        private void IncreaseIkWeight()
        {
            _weight = 1;
            _increaseWeight = false;

            SetWeight(_weight);
        }

        private void SetWeight(float weight)
        {
            _fullBodyIk.solver.rightHandEffector.positionWeight = weight;
            _fullBodyIk.solver.rightHandEffector.rotationWeight = weight;
            _fullBodyIk.solver.rightArmChain.bendConstraint.weight = weight;
            _fullBodyIk.solver.leftHandEffector.positionWeight = weight;
            _fullBodyIk.solver.leftHandEffector.rotationWeight = weight;
            _fullBodyIk.solver.leftArmChain.bendConstraint.weight = weight;
        }

        private void DecreaseIkWeight(float deltaTime)
        {
            _weight -= deltaTime * _lerpFactor;
            
            if (_weight < 0)
            {
                _decreaseWeight = false;
            }
            SetWeight(_weight);
        }

        public void EnableIk(float time)
        {
            _lerpFactor = 1 / time;
            
            _increaseWeight = true;
            _decreaseWeight = false;
        }
        
        public void DisableIk(float time)
        {
            _lerpFactor = 1 / time;
            _increaseWeight = false;
            _decreaseWeight = true;
        }
        
        // public void DisableIkAndParent()
        // {
        //     SetWeight(0);
        //     
        // }

        public void TakeHandsOffGun()
        {
            rightHandIkTarget.SetParent(null);
            leftHandIkTarget.SetParent(null);
            
            _fullBodyIk.solver.rightHandEffector.target = null;
            _fullBodyIk.solver.leftHandEffector.target = null;
            
            DisableIk(0.1f);
        }
    }
}
