using System.Collections.Generic;
using AIDetection;
using Constants;
using UnityEngine;

namespace BehaviourTrees.Zombie
{
    public class CanZombieSeeAnyTargets : Node
    {
        private readonly float _sightRadius;
        private readonly float _halfFov;
        private readonly Transform _eyesTransform;
        private readonly Transform _currentTransform;
        private readonly Animator _animator;

        public CanZombieSeeAnyTargets(float sightRadius, float fov, Transform eyesTransform, Transform currentTransform, Animator animator)
        {
            _sightRadius = sightRadius;
            _halfFov = fov / 2;
            _eyesTransform = eyesTransform;
            _currentTransform = currentTransform;
            _animator = animator;
        }

        public override NodeState Evaluate()
        {
            IList<Detectee> detectees = SightManager.Detectees;

            for (int i = 0; i < detectees.Count; i++)
            {
                Detectee currentDetectee = detectees[i];
                if (!VisionDetector.Detect(currentDetectee, _eyesTransform, _sightRadius, _halfFov, _currentTransform)) continue;

                AddEditData(BehaviourTreeData.TargetData, currentDetectee);
                AddEditData(BehaviourTreeData.TargetPositionData, currentDetectee.transform.position);

                object alertData = GetData(BehaviourTreeData.AlertData);
                if (alertData is false or null)
                {
                    _animator.SetTrigger(AnimatorConstants.AlertTrigger);
                    AddEditData(BehaviourTreeData.AlertData, true);
                }
                return State = NodeState.Success;
            }
            return State = NodeState.Failure;
        }
    }
}