namespace OpenRasta.OperationModel
{
    using OpenRasta.Contracts.TypeSystem;

    public class OutputMember
    {
        public IMember Member { get; set; }

        public object Value { get; set; }
    }
}