namespace OpenRasta.Contracts.OperationModel
{
    using System.Collections;
    using System.Collections.Generic;

    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.OperationModel;

    public interface IOperation : IAttributeProvider
    {
        IEnumerable<InputMember> Inputs { get; }

        IDictionary ExtendedProperties { get; }

        string Name { get; }

        IEnumerable<OutputMember> Invoke();
    }
}