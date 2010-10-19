using OpenRasta.Binding;
using OpenRasta.TypeSystem;

namespace OpenRasta.OperationModel
{
    using OpenRasta.Contracts.Binding;
    using OpenRasta.Contracts.TypeSystem;

    public class OperationMember
    {
        public OperationMember(IMember member, IObjectBinder binder, bool isOptional)
        {
            Member = member;
            Binder = binder;
            IsOptional = isOptional;
        }

        public IObjectBinder Binder { get; private set; }
        public bool IsOptional { get; private set; }

        public bool IsReadyForAssignment
        {
            get { return IsOptional || !Binder.IsEmpty; }
        }

        public IMember Member { get; private set; }
    }
}