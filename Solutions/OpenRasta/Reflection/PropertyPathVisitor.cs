namespace OpenRasta.Reflection
{
    #region Using Directives

    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using OpenRasta.TypeSystem.ReflectionBased;

    #endregion

    public class PropertyPathVisitor : ExpressionVisitor
    {
        public PropertyPathVisitor()
        {
            this.PropertyPathBuilder = new StringBuilder();
        }

        public Type PropertyType { get; private set; }

        protected StringBuilder PropertyPathBuilder { get; set; }

        protected string RootType { get; set; }

        public PropertyPath BuildPropertyPath(Expression expression)
        {
            Visit(expression);
            
            if (this.RootType != null || this.PropertyPathBuilder.Length != 0)
            {
                return new PropertyPath
                {
                    TypePrefix = this.RootType,
                    TypeSuffix = this.PropertyPathBuilder.ToString()
                };
            }

            return null;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            this.Visit(m.Expression);
            this.PropertyType = m.Type;
            
            try
            {
                var rootInstance = Expression.Lambda(typeof(Func<object>), m.Expression).Compile().DynamicInvoke();

                if (rootInstance != null)
                {
                    {
                        var pi = m.Member as PropertyInfo;
                        object resultingInstance = null;

                        if (pi != null)
                        {
                            resultingInstance = pi.GetValue(rootInstance, null);
                        }
                       
                        var fi = m.Member as FieldInfo;
                        
                        if (fi != null)
                        {
                            resultingInstance = fi.GetValue(rootInstance);
                        }
                        
                        if (resultingInstance != null)
                        {
                            var propertyPath = ObjectPaths.Get(resultingInstance);
                            
                            if (propertyPath != null)
                            {
                                this.RootType = propertyPath.TypePrefix;
                                this.AppendPropertyPath(propertyPath.TypeSuffix);
                                
                                return m;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            if (this.RootType == null)
            {
                this.RootType = this.ExtractType(m.Member).GetTypeString();
            }
            else
            {
                var name = m.Member.Name;
                this.AppendPropertyPath(name);
            }
            
            return m;
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (this.RootType == null)
            {
                this.RootType = p.Type.GetTypeString();
            }
            return base.VisitParameter(p);
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            base.VisitMethodCall(m);

            if (m.Method.Name == "get_Item" && m.Arguments.Count == 1 && m.Arguments[0].NodeType == ExpressionType.Constant)
            {
                object argumentValue = ((ConstantExpression)m.Arguments[0]).Value;
                this.PropertyPathBuilder.Append(":").Append(argumentValue.ConvertToString());
            }

            return m;
        }

        private void AppendPropertyPath(string name)
        {
            if (this.PropertyPathBuilder.Length > 0)
            {
                this.PropertyPathBuilder.Append(".");
            }

            this.PropertyPathBuilder.Append(name);
        }

        private Type ExtractType(MemberInfo info)
        {
            var pi = info as PropertyInfo;
            
            if (pi != null)
            {
                return pi.PropertyType;
            }

            var fi = info as FieldInfo;

            if (fi != null)
            {
                return fi.FieldType;
            }

            return null;
        }
    }
}