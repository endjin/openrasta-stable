namespace OpenRasta.TypeSystem.Surrogated
{
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.TypeSystem.Surrogates;

    public class PropertyWithSurrogatesBuilder : PropertyBuilder, IKeepSurrogateInstances
    {
        public PropertyWithSurrogatesBuilder(IProperty property, IMemberBuilder parent, IEnumerable<IType> alienTypes)
            : base(parent, property)
        {
            this.Surrogates = alienTypes.ToDictionary(x => (IMember)x, x => (ISurrogate)x.CreateInstance());
        }

        public IDictionary<IMember, ISurrogate> Surrogates { get; private set; }
    }
}