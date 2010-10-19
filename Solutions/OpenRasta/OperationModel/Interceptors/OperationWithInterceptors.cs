namespace OpenRasta.OperationModel.Interceptors
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;

    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Contracts.OperationModel.Interceptors;
    using OpenRasta.Exceptions;
    using OpenRasta.Extensions;

    #endregion

    public class OperationWithInterceptors : IOperation
    {
        private readonly IEnumerable<IOperationInterceptor> interceptors;
        private readonly IOperation wrappedOperation;

        public OperationWithInterceptors(IOperation wrappedOperation, IEnumerable<IOperationInterceptor> systemInterceptors)
        {
            this.wrappedOperation = wrappedOperation;
            this.interceptors = systemInterceptors;
        }

        public IDictionary ExtendedProperties
        {
            get { return this.wrappedOperation.ExtendedProperties; }
        }

        public IEnumerable<InputMember> Inputs
        {
            get { return this.wrappedOperation.Inputs; }
        }

        public string Name
        {
            get { return this.wrappedOperation.Name; }
        }

        public T FindAttribute<T>() where T : class
        {
            return this.wrappedOperation.FindAttribute<T>();
        }

        public IEnumerable<T> FindAttributes<T>() where T : class
        {
            return this.wrappedOperation.FindAttributes<T>();
        }

        public IEnumerable<OutputMember> Invoke()
        {
            this.ExecutePreConditions();

            Func<IEnumerable<OutputMember>> operation = () => this.wrappedOperation.Invoke();

            foreach (var executingCondition in this.interceptors)
            {
                operation = executingCondition.RewriteOperation(operation) ?? operation;
            }

            var results = operation();

            this.ExecutePostConditions(results);

            return results;
        }

        private void ExecutePostConditions(IEnumerable<OutputMember> results)
        {
            foreach (var postCondition in this.interceptors)
            {
                this.TryExecute(() => postCondition.AfterExecute(this.wrappedOperation, results), "The interceptor {0} stopped execution.".With(postCondition.GetType().Name));
            }
        }

        private void ExecutePreConditions()
        {
            foreach (var precondition in this.interceptors)
            {
                this.TryExecute(() => precondition.BeforeExecute(this.wrappedOperation), "The interceptor {0} stopped execution.".With(precondition.GetType().Name));
            }
        }

        private void TryExecute(Func<bool> interception, string exceptionMessage)
        {
            Exception exception = null;

            try
            {
                bool successful = interception();

                if (!successful)
                {
                    exception = new InterceptorException(exceptionMessage);
                }
            }
            catch (Exception e)
            {
                exception = new InterceptorException(exceptionMessage, e);
            }

            if (exception != null)
            {
                throw exception;
            }
        }
    }
}