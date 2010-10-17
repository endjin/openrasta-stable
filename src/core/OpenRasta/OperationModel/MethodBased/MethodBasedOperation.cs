namespace OpenRasta.OperationModel.MethodBased
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Binding;
    using OpenRasta.Collections;
    using OpenRasta.DI;
    using OpenRasta.TypeSystem;

    public class MethodBasedOperation : IOperation
    {
        private readonly IMethod method;
        private readonly IType ownerType;
        private readonly Dictionary<IParameter, IObjectBinder> parameterBinders;

        public MethodBasedOperation(IObjectBinderLocator binderLocator, IType ownerType, IMethod method)
        {
            this.method = method;
            this.ownerType = ownerType;
            this.parameterBinders = method.InputMembers.ToDictionary(x => x, x => binderLocator.GetBinder(x));
            this.Inputs = this.parameterBinders.Select(x => new InputMember(x.Key, x.Value, x.Key.IsOptional));
            this.ExtendedProperties = new NullBehaviorDictionary<object, object>();
        }

        public IDictionary ExtendedProperties { get; private set; }

        public IEnumerable<InputMember> Inputs { get; private set; }

        public string Name
        {
            get { return this.method.Name; }
        }

        public IDependencyResolver Resolver { get; set; }

        public override string ToString()
        {
            return this.method.ToString();
        }

        public T FindAttribute<T>() where T : class
        {
            return this.method.FindAttribute<T>() ?? this.ownerType.FindAttribute<T>();
        }

        public IEnumerable<T> FindAttributes<T>() where T : class
        {
            return this.ownerType.FindAttributes<T>().Concat(this.method.FindAttributes<T>());
        }

        public IEnumerable<OutputMember> Invoke()
        {
            if (!this.Inputs.AllReady())
            {
                throw new InvalidOperationException("The operation is not ready for invocation.");
            }

            var handler = this.ownerType.CreateInstance(this.Resolver);

            var bindingResults = from kv in this.parameterBinders
                                 let param = kv.Key
                                 let binder = kv.Value
                                 select binder.IsEmpty
                                            ? BindingResult.Success(param.DefaultValue)
                                            : binder.BuildObject();

            var parameters = this.GetParameters(bindingResults);

            var result = this.method.Invoke(handler, parameters.ToArray());

            // note this is only temporary until we implement out and ref support...
            if (this.method.OutputMembers.Any())
            {
                return new[]
                {
                    new OutputMember
                    {
                        Member = this.method.OutputMembers.Single(), 
                        Value = result.Single()
                    }
                };
            }

            return new OutputMember[0];
        }

        private IEnumerable<object> GetParameters(IEnumerable<BindingResult> results)
        {
            foreach (var result in results)
            {
                if (!result.Successful)
                {
                    throw new InvalidOperationException("A parameter wasn't successfully created.");
                }
                
                yield return result.Instance;
            }
        }
    }
}