using System.Collections.Generic;

namespace BehaviourTrees
{
    public class Sequence : Node
    {
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (Node child in Children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.Failure:
                        State = NodeState.Failure;
                        return State;
                    case NodeState.Running:
                        return State = NodeState.Running;
                    case NodeState.Success:
                        continue;
                    default:
                        State = NodeState.Success;
                        return State;
                }
            }

            return State = NodeState.Success;
        }
    }
}