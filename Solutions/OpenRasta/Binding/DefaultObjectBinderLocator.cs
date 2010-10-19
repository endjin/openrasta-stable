namespace OpenRasta.Binding
{
    #region Using Directives

    using System;
    using System.Linq;

    using OpenRasta.Contracts.Binding;
    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Diagnostics;
    using OpenRasta.TypeSystem;

    #endregion

    public class DefaultObjectBinderLocator : IObjectBinderLocator
    {
        public DefaultObjectBinderLocator()
        {
            this.Logger = NullLogger.Instance;
            this.TypeSystem = TypeSystems.Default;
        }

        public ILogger Logger { get; set; }

        public ITypeSystem TypeSystem { get; set; }

        public IObjectBinder GetBinder(IMember member)
        {
            var abstractObjectBinderAttribute = member.FindAttribute<BinderAttribute>() ?? member.Type.FindAttribute<BinderAttribute>();

            if (abstractObjectBinderAttribute != null)
            {
                return abstractObjectBinderAttribute.GetBinder(member);
            }

            IMethod binderMethod = member.GetMethod("GetBinder");
            
            if (binderMethod != null)
            {
                try
                {
                    return binderMethod.Invoke(null, this.TypeSystem, member).OfType<IObjectBinder>().Single();
                }
                catch (Exception e)
                {
                    this.LogGetBinderMethodCouldntBeRun(e);
                }
            }

            return new KeyedValuesBinder(member.Type, member.Name);
        }

        private void LogGetBinderMethodCouldntBeRun(Exception exception)
        {
            this.Logger.WriteWarning("Method GetBinder couldn't be processed. See the exception for more details.");
            this.Logger.WriteException(exception);
        }
    }
}