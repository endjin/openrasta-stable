namespace OpenRasta.TypeSystem.ReflectionBased
{
    #region Using Directives

    using System.Reflection;

    using OpenRasta.Contracts.TypeSystem;

    #endregion

    public class ReflectionBasedParameter : ReflectionBasedMember<IParameterBuilder>, IParameter
    {
        private readonly ReflectionBasedMethod ownerMethod;
        private readonly ParameterInfo parameterInfo;

        public ReflectionBasedParameter(ReflectionBasedMethod ownerMethod, ParameterInfo parameterInfo)
            : base(ownerMethod.TypeSystem, parameterInfo.ParameterType)
        {
            this.ownerMethod = ownerMethod;
            this.parameterInfo = parameterInfo;
        }

        public object DefaultValue
        {
            get
            {
                return this.parameterInfo.DefaultValue == Missing.Value
                           ? this.parameterInfo.ParameterType.GetDefaultValue()
                           : this.parameterInfo.DefaultValue;
            }
        }

        public bool IsOutput
        {
            get { return this.parameterInfo.IsOut; }
        }

        public bool IsOptional
        {
            get { return this.parameterInfo.IsOptional; }
        }

        public override string Name
        {
            get { return this.parameterInfo.Name; }
        }

        public IMethod Owner
        {
            get { return this.ownerMethod; }
        }
    }
}