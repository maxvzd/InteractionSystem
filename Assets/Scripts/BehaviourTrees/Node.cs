using System;
using System.Collections.Generic;

namespace BehaviourTrees
{
    public enum NodeState
    {
        Running,
        Success,
        Failure
    }
    
    public abstract class Node
    {
        protected NodeState State;
        
        public Node Parent;
        protected List<Node> Children;

        private Dictionary<string, object> _dataContext = new();

        protected Node()
        {
            Parent = null;
            Children = new List<Node>();
        }

        protected Node(List<Node> children)
        {
            Children = new List<Node>();
            foreach (Node child in children)
            {
                Attach(child);
            }
        }

        public abstract NodeState Evaluate();
        
        protected void AddEditData(string key, object value)
        {
            if (Parent is null)
            {
                object data = GetData(key);
                if (data is not null)
                {
                    RemoveData(key);
                }
                _dataContext.Add(key, value);
            }
            else
            {
                Parent.AddEditData(key, value);
            }
        }

        protected object GetData(string key)
        {
            if (Parent is null)
            {
                if (_dataContext.TryGetValue(key, out var data))
                {
                    return data;
                }
            }
            else
            {
                return Parent.GetData(key);
            }
            return null;
        }

        // protected bool? GetDataAsBool(string key)
        // {
        //     object data = GetData(key);
        //     if (data is bool b)
        //     {
        //         return b;
        //     }
        //
        //     return null;
        // }
        
        protected bool RemoveData(string key)
        {
            if (Parent is null)
            {
                if (_dataContext.TryGetValue(key, out var _))
                {
                    return _dataContext.Remove(key);
                }
            }
            else
            {
                return Parent.RemoveData(key);
            }

            return false;
        }
        
        private void Attach(Node node)
        {
            node.Parent = this;
            Children.Add(node);
        }
    }
}