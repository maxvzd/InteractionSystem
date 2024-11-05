using UnityEngine;

namespace BehaviourTrees
{
    public abstract class BehaviourTree : MonoBehaviour
    {
        private Node _root;

        public bool isAiEnabled = true;

        private float _timeElapsed;
        private const float TICK = 0.5f;


        protected virtual void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root is null || !isAiEnabled) return;

            _timeElapsed += Time.deltaTime;

            if (_timeElapsed > TICK)
            {
                _timeElapsed = 0f;
                _root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
    }
}