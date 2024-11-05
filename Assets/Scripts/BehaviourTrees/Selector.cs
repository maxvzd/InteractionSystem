using System.Collections.Generic;

namespace BehaviourTrees
{
    public class Selector : Node
    {
        public Selector(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (Node child in Children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.Failure:
                        continue;
                    case NodeState.Running:
                        State = NodeState.Running;
                        return State;
                    case NodeState.Success:
                        State = NodeState.Success;
                        return State;
                    default:
                        continue;
                }
            }

            return State = NodeState.Failure;
        }
    }
}