namespace OpenRasta.Contracts.OperationModel
{
    using System.Collections.Generic;

    public interface IOperationProcessor
    {
        IEnumerable<IOperation> Process(IEnumerable<IOperation> operations);        
    }
}