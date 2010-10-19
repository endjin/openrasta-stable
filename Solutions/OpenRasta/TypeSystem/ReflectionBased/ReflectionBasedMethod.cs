namespace OpenRasta.TypeSystem.ReflectionBased
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Extensions;

    public class ReflectionBasedMethod : IMethod
    {
        private readonly MethodInfo methodInfo;
        private readonly object syncRoot = new object();

        internal ReflectionBasedMethod(IMember ownerType, MethodInfo methodInfo)
        {
            this.methodInfo = methodInfo;
            this.Owner = ownerType;
            this.TypeSystem = TypeSystems.Default;
            this.EnsureInputMembersExist();
            this.EnsureOutputMembersExist();
        }

        public IEnumerable<IParameter> InputMembers { get; private set; }

        public string Name
        {
            get { return this.methodInfo.Name; }
        }

        public IEnumerable<IMember> OutputMembers { get; private set; }

        public IMember Owner { get; set; }

        public ITypeSystem TypeSystem { get; set; }

        public override string ToString()
        {
            return "{0}::{1}({2})".With(this.Owner.TypeName, this.methodInfo.Name, string.Join(", ", this.methodInfo.GetParameters().Select(x => "{0} {1}".With(x.ParameterType.Name, x.Name)).ToArray()));
        }

        public T FindAttribute<T>() where T : class
        {
            return FindAttributes<T>().FirstOrDefault();
        }

        public IEnumerable<T> FindAttributes<T>() where T : class
        {
            return this.methodInfo.GetCustomAttributes(typeof(T), true).Cast<T>();
        }

        public IEnumerable<object> Invoke(object target, params object[] members)
        {
            return new[] { this.methodInfo.Invoke(target, members) };
        }

        private void EnsureInputMembersExist()
        {
            if (this.InputMembers == null)
            {
                this.InputMembers = this.methodInfo.GetParameters()
                    .Where(x => !x.IsOut)
                    .Select(x => (IParameter)new ReflectionBasedParameter(this, x)).ToList().AsReadOnly();
            }
        }

        private void EnsureOutputMembersExist()
        {
            if (this.OutputMembers == null)
            {
                var outputParameters = new List<IMember>();
                outputParameters.Add(this.TypeSystem.FromClr(this.methodInfo.ReturnType));
                
                foreach (var outOrRefParameter in this.InputMembers.Where(x => x.IsOutput))
                {
                    outputParameters.Add(outOrRefParameter);
                }
                
                this.OutputMembers = outputParameters.AsReadOnly();
            }
        }
    }
}