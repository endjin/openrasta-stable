namespace OpenRasta.OperationModel.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;

    using OpenRasta.Binding;
    using OpenRasta.Collections;
    using OpenRasta.Diagnostics;
    using OpenRasta.Pipeline;
    using OpenRasta.TypeSystem.ReflectionBased;
    using OpenRasta.Web;

    public class UriParametersFilter : IOperationFilter
    {
        private readonly PipelineData pipelineData;

        public UriParametersFilter(ICommunicationContext context, IErrorCollector collector)
        {
            this.Logger = NullLogger.Instance;
            this.Errors = collector;
            this.pipelineData = context.PipelineData;
        }

        private IErrorCollector Errors { get; set; }
        
        private ILogger Logger { get; set; }

        public IEnumerable<IOperation> Process(IEnumerable<IOperation> operations)
        {
            int acceptedMethods = 0;

            foreach (var operation in operations)
            {
                if (IsEmpty(this.pipelineData.SelectedResource.UriTemplateParameters))
                {
                    this.LogAcceptNoUriParameters(operation);
                    acceptedMethods++;
                    
                    yield return operation;
                    
                    continue;
                }

                foreach (var uriParameterMatches in this.pipelineData.SelectedResource.UriTemplateParameters)
                {
                    var uriParametersCopy = new NameValueCollection(uriParameterMatches);

                    var matchedParameters = from member in operation.Inputs
                                            from matchedParameterName in uriParametersCopy.AllKeys
                                            where this.TrySetPropertyAndRemoveUsedKey(member, matchedParameterName, uriParametersCopy, ConvertFromString)
                                            select matchedParameterName;

                    if (matchedParameters.Count() == uriParameterMatches.Count)
                    {
                        this.LogOperationAccepted(uriParameterMatches, operation);
                        acceptedMethods++;

                        yield return operation;
                    }
                }
            }

            this.LogAcceptedCount(acceptedMethods);

            if (acceptedMethods <= 0)
            {
                this.Errors.AddServerError(
                    CreateErrorNoOperationFound(this.pipelineData.SelectedResource.UriTemplateParameters));
            }
        }

        private static BindingResult ConvertFromString(string strings, Type entityType)
        {
            try
            {
                return BindingResult.Success(entityType.CreateInstanceFrom(strings));
            }
            catch (Exception e)
            {
                if (e.InnerException is FormatException)
                {
                    return BindingResult.Failure();
                }

                throw;
            }
        }

        private static ErrorFrom<UriParametersFilter> CreateErrorNoOperationFound(IEnumerable<NameValueCollection> uriTemplateParameters)
        {
            return new ErrorFrom<UriParametersFilter>
            {
                Title = "No method matched the uri parameters", 
                Message = string.Format(
                    "None of the operations had members that could be matches against the uri parameters:\r\n{0}", 
                    FormatUriParameterMatches(uriTemplateParameters))
            };
        }

        private static string FormatUriParameterMatches(IEnumerable<NameValueCollection> uriParameterMatches)
        {
            var builder = new StringBuilder();
            
            foreach (var nvc in uriParameterMatches)
            {
                builder.AppendLine(nvc.ToHtmlFormEncoding());
            }

            return builder.ToString();
        }

        private static bool IsEmpty(IList<NameValueCollection> parameters)
        {
            return parameters.Count == 0 || (parameters.Count == 1 && parameters[0].Count == 0);
        }

        private void LogAcceptedCount(int operationCount)
        {
            this.Logger.WriteInfo("Found {0} potential operations to resolve.", operationCount);
        }

        private void LogAcceptNoUriParameters(IOperation operation)
        {
            this.Logger.WriteDebug("Empty parameter list, selected method {0}.", operation);
        }

        private void LogOperationAccepted(NameValueCollection uriParameterMatches, IOperation operation)
        {
            this.Logger.WriteDebug(
                "Accepted operation {0} with {1} matched parameters for uri parameter list {2}.", 
                operation.Name, 
                operation.Inputs.CountReady(), 
                uriParameterMatches.ToHtmlFormEncoding());
        }

        private bool TrySetPropertyAndRemoveUsedKey(InputMember member, string uriParameterName, NameValueCollection uriParameters, ValueConverter<string> converter)
        {
            if (member.Binder.SetProperty(uriParameterName, uriParameters.GetValues(uriParameterName), converter))
            {
                uriParameters.Remove(uriParameterName);

                return true;
            }

            return false;
        }
    }
}