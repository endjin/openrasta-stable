namespace OpenRasta.Reflection
{
    using System;
    using System.Linq.Expressions;

    public abstract class PropertyPathExpressionTree
    {
        protected PropertyPathExpressionTree()
        {
        }
        
        protected PropertyPathExpressionTree(Expression property)
        {
            this.ProcessMemberAccess(property);
        }

        public Type PropertyType { get; private set; }

        public object Value { get; protected set; }
        
        public PropertyPath Path { get; private set; }
        
        public string FullPath
        {
            get
            {
                return this.Path == null ? string.Empty : this.Path.TypePrefix + "." + this.Path.TypeSuffix;
            }
        }

        protected void ProcessMemberAccess(Expression memberExpression)
        {
            var visitor = new PropertyPathVisitor();
            
            this.Path = visitor.BuildPropertyPath(memberExpression);
            this.PropertyType = visitor.PropertyType;
        }
    }
}