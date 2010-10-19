namespace OpenRasta.OperationModel
{
    #region Using Directives

    using OpenRasta.Contracts.Binding;
    using OpenRasta.Contracts.TypeSystem;

    #endregion

    public class OperationMember
    {
        public OperationMember(IMember member, IObjectBinder binder, bool optional)
        {
            this.Member = member;
            this.Binder = binder;
            this.IsOptional = optional;
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