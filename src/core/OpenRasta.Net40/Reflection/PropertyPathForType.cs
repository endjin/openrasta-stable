namespace OpenRasta.Reflection
{
    using System;
    using System.Linq.Expressions;
    
    public class PropertyPathForType<TType, TProperty> : PropertyPathExpressionTree
    {
        public PropertyPathForType(Expression<Func<TType, TProperty>> property) : base(property)
        {
        }
    }
}