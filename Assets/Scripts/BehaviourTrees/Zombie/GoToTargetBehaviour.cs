using AIDetection;
using Constants;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTrees.Zombie
{
    public class GoToTargetBehaviour : Node
    {
        private readonly NavMeshAgent _navmeshAgent;
        private readonly Transform _currentTransform;
        private readonly float _halfFov;
        private readonly float _sightRadius;
        private readonly Transform _eyesTransform;

        public GoToTargetBehaviour(NavMeshAgent navmeshAgent, Transform currentTransform, float fov, float sightRadius, Transform eyesTransform)
        {
            _navmeshAgent = navmeshAgent;
            _currentTransform = currentTransform;
            _halfFov = fov / 2f;
            _sightRadius = sightRadius;
            _eyesTransform = eyesTransform;
        }

        public override NodeState Evaluate()
        {
            object dataPos = GetData(BehaviourTreeData.TargetPositionData);
            object dataTarget = GetData(BehaviourTreeData.TargetData);

            if (dataPos is not Vector3 pos || dataTarget is not Detectee target) return NodeState.Failure;

            bool canSeeTarget = VisionDetector.Detect(target, _eyesTransform, _sightRadius, _halfFov, _currentTransform);
            if(canSeeTarget)
            {
                pos = target.transform.position;
                AddEditData(BehaviourTreeData.TargetPositionData, pos);
            }
            _navmeshAgent.SetDestination(pos);
            
            if (!_navmeshAgent.pathPending && _navmeshAgent.remainingDistance <= _navmeshAgent.stoppingDistance)
            {
                RemoveData(BehaviourTreeData.TargetPositionData);
                RemoveData(BehaviourTreeData.TargetData);
                
                return canSeeTarget ? State = NodeState.Success : State = NodeState.Failure;
            }
            return State = NodeState.Running;
        }
    }
}