namespace OpenRasta.TypeSystem.Surrogates
{
    using System;

    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.TypeSystem.Surrogates;

    public abstract class AbstractStaticSurrogateBuilder : ISurrogateBuilder
    {
        public bool CanCreateFor(IMember type)
        {
            return this.CanCreateFor(type.StaticType);
        }

        public abstract bool CanCreateFor(Type type);

        public IType Create(IMember type)
        {
            return type.TypeSystem.FromClr(this.Create(type.StaticType));
        }

        public abstract Type Create(Type type);
    }
}