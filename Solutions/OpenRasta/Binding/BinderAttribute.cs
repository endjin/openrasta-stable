namespace OpenRasta.Binding
{
    using System;

    using OpenRasta.Contracts.Binding;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.DI;
    using OpenRasta.TypeSystem;

    /// <summary>
    /// Defines a binder locator for a statically typed member
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Property | AttributeTargets.Struct | AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = true)]
    public class BinderAttribute : Attribute, IObjectBinderLocator
    {
        public Type Type { get; set; }

        public virtual IObjectBinder GetBinder(IMember parameterInfo)
        {
            return (IObjectBinder)DependencyManager.GetService(Type);
        }
    }
}