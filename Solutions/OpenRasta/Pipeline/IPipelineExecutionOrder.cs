namespace OpenRasta.Pipeline
{
    using System;

    public interface IPipelineExecutionOrder
    {
        IPipelineExecutionOrderAnd Before(Type contributorType);

        IPipelineExecutionOrderAnd After(Type contributorType);
    }
}