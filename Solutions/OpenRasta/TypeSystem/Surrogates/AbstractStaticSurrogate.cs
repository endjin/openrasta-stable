namespace OpenRasta.TypeSystem.Surrogates
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.TypeSystem.Surrogates;

    #endregion

    public abstract class AbstractStaticSurrogate<TSurrogated> : ISurrogateBuilder, ISurrogate
    {
        object ISurrogate.Value
        {
            get { return this.Value; }
            set { this.Value = (TSurrogated)value; }
        }

        protected virtual TSurrogated Value { get; set; }

        bool ISurrogateBuilder.CanCreateFor(IMember member)
        {
            return this.SupportedTypes().Any(x => member.Type.IsAssignableTo(x));
        }

        IType ISurrogateBuilder.Create(IMember type)
        {
            return type.TypeSystem.FromClr(GetType());
        }

        protected virtual IEnumerable<Type> SupportedTypes()
        {
            yield return typeof(TSurrogated);
        }
    }
}