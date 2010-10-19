namespace OpenRasta.OperationModel
{
    #region Using Directives

    using OpenRasta.Contracts.Binding;
    using OpenRasta.Contracts.TypeSystem;

    #endregion

    public class InputMember
    {
        public InputMember(IMember member, IObjectBinder binder, bool isOptional)
        {
            this.Member = member;
            this.Binder = binder;
            this.IsOptional = isOptional;
        }

        public IObjectBinder Binder { get; private set; }

        public bool IsOptional { get; private set; }

        public bool IsReadyForAssignment
        {
            get { return this.IsOptional || !this.Binder.IsEmpty; }
        }

        public IMember Member { get; private set; }
    }
}