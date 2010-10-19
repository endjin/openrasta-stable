namespace OpenRasta.TypeSystem.Surrogated
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.TypeSystem.Surrogated;
    using OpenRasta.Contracts.TypeSystem.Surrogates;

    #endregion

    public class AlienPropertyBuilder : PropertyBuilder
    {
        private readonly IKeepSurrogateInstances parentBuilder;
        private readonly ISurrogate surrogatedTypeBuilder;

        public AlienPropertyBuilder(IMember owner, IMemberBuilder parentBuilder, IProperty alienProperty) : base(parentBuilder, alienProperty)
        {
            this.parentBuilder = (IKeepSurrogateInstances)parentBuilder;
            this.surrogatedTypeBuilder = this.parentBuilder.Surrogates[owner];
        }

        public override object Apply(object target, out object assignedValue)
        {

            if (target == null)
            {
                target = Owner.Member.Type.CreateInstance();
            }

            this.surrogatedTypeBuilder.Value = target;

            assignedValue = Value;
            
            if (Property.TrySetValue(this.surrogatedTypeBuilder, assignedValue))
            {
                return this.surrogatedTypeBuilder.Value;
            }

            throw new InvalidOperationException("An error has occurred while applying the changes.");
        }
    }
}