namespace OpenRasta.TypeSystem.Surrogated
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.TypeSystem.Surrogated;
    using OpenRasta.Contracts.TypeSystem.Surrogates;

    #endregion

    public class TypeWithSurrogatesBuilder : TypeBuilder, IKeepSurrogateInstances
    {
        public TypeWithSurrogatesBuilder(IType typeWithSurrogates, IEnumerable<IType> alienTypes)
            : base(typeWithSurrogates)
        {
            this.Surrogates = alienTypes.ToDictionary(x => (IMember)x, x => (ISurrogate)x.CreateInstance());
        }

        public IDictionary<IMember, ISurrogate> Surrogates { get; private set; }
    }
}