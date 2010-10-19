namespace OpenRasta.OperationModel.MethodBased
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Contracts.OperationModel.MethodBased;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.TypeSystem;

    #endregion

    public class TypeExclusionMethodFilter<T> : IMethodFilter
    {
        public TypeExclusionMethodFilter()
        {
            this.TypeSystem = TypeSystems.Default;
        }

        public ITypeSystem TypeSystem { get; set; }

        public IEnumerable<IMethod> Filter(IEnumerable<IMethod> methods)
        {
            var type = this.TypeSystem.FromClr<T>();

            return from method in methods
                   where method.Owner.Type.CompareTo(type) != 0
                   select method;
        }
    }
}