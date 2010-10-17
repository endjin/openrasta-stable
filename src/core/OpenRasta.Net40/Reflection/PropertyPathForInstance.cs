

namespace OpenRasta.Reflection
{
    using System;
    using System.Linq.Expressions;
    
    public class PropertyPathForInstance<TProperty> : PropertyPathExpressionTree
    {
        public PropertyPathForInstance(Expression<Func<TProperty>> instanceProperty)
        {
            ProcessMemberAccess(instanceProperty);
            
            try
            {
                var accessor = instanceProperty.Compile();
                base.Value = accessor();
            }
            catch (NullReferenceException)
            {
            }
        }

        public new TProperty Value
        {
            get
            {
                return (TProperty)base.Value;
            }
        }
    }
}