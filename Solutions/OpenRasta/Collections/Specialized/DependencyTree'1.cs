namespace OpenRasta.Collections.Specialized
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DependencyTree<T>
    {
        private bool isNormalized;

        public DependencyTree(T rootNode)
        {
            this.Nodes = new List<DependencyNode<T>>();
            this.RootNode = new DependencyNode<T>(rootNode);
            this.Nodes.Add(this.RootNode);
        }

        public ICollection<DependencyNode<T>> Nodes { get; private set; }

        public DependencyNode<T> RootNode { get; private set; }

        public DependencyNode<T> CreateNode(T value)
        {
            this.isNormalized = false;
            var newNode = new DependencyNode<T>(value);
            this.Nodes.Add(newNode);

            return newNode;
        }

        public IEnumerable<DependencyNode<T>> GetCallGraph()
        {
            var list = new List<DependencyNode<T>>();
            
            this.NormalizeVertices();
            this.RootNode.QueueNodes(list);

            return list;
        }

        public void NormalizeVertices()
        {
            if (!this.isNormalized)
            {
                foreach (var node in this.Nodes)
                {
                    foreach (var parent in node.ParentNodes)
                    {
                        if (!parent.ChildNodes.Contains(node))
                        {
                            parent.ChildNodes.Add(node);
                        }
                    }

                    foreach (var child in node.ChildNodes)
                    {
                        if (!child.ParentNodes.Contains(node))
                        {
                            child.ParentNodes.Add(node);
                        }
                    }
                }

                this.VerifyNoCyclicDependency();
                this.isNormalized = true;
            }
        }

        private void VerifyNoCyclicDependency()
        {
            if (this.Nodes.Any(x => x.HasRecursiveNodes()))
            {
                throw new InvalidOperationException();
            }
        }
    }
}