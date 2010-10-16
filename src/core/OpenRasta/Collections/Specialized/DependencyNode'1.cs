namespace OpenRasta.Collections.Specialized
{
    using System.Collections.Generic;
    using System.Linq;

    public class DependencyNode<T>
    {
        public DependencyNode(T value)
        {
            this.Value = value;
            this.ParentNodes = new List<DependencyNode<T>>();
            this.ChildNodes = new List<DependencyNode<T>>();
        }

        public ICollection<DependencyNode<T>> ChildNodes { get; private set; }

        public int OutgoingWeight
        {
            get
            {
                if (this.HasRecursiveNodes())
                {
                    return -1;
                }

                return this.ChildNodes.Aggregate(this.ChildNodes.Count, (count, node) => count + node.OutgoingWeight);
            }
        }

        public ICollection<DependencyNode<T>> ParentNodes { get; private set; }

        public T Value { get; set; }

        protected bool Visited { get; set; }

        public bool HasRecursiveNodes()
        {
            return this.HasRecursiveNodes(new Stack<DependencyNode<T>>());
        }

        public void QueueNodes(ICollection<DependencyNode<T>> nodes)
        {
            this.Visited = true;

            foreach (var parentNode in this.ParentNodes.OrderBy(x => x.OutgoingWeight))
            {
                if (parentNode.Visited)
                {
                    continue;
                }

                parentNode.QueueNodes(nodes);
            }

            nodes.Add(this);
            
            foreach (var childNode in this.ChildNodes.OrderBy(x => x.OutgoingWeight))
            {
                if (childNode.Visited)
                {
                    continue;
                }

                childNode.QueueNodes(nodes);
            }
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        private bool HasRecursiveNodes(Stack<DependencyNode<T>> recursionDefender)
        {
            if (recursionDefender.Contains(this))
            {
                throw new RecursionException();
            }

            recursionDefender.Push(this);
            
            try
            {
                foreach (var child in this.ChildNodes)
                {
                    if (child.HasRecursiveNodes(recursionDefender))
                    {
                        return true;
                    }
                }
            }
            finally
            {
                recursionDefender.Pop();
            }

            return false;
        }
    }
}