namespace OpenRasta.Binding
{
    #region Using Directives

    using OpenRasta.Contracts.Binding;
    using OpenRasta.Contracts.TypeSystem;

    #endregion

    public class KeyedValuesBinderAttribute : BinderAttribute
    {
        public override IObjectBinder GetBinder(IMember memberInfo)
        {
            return new KeyedValuesBinder(memberInfo.Type, memberInfo.Name);
        }
    }
}