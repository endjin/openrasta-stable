namespace OpenRasta.DI.Internal
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Exceptions;
    using OpenRasta.Extensions;

    #endregion

    public class ObjectBuilder
    {
        public ObjectBuilder(ResolveContext context, ILogger log)
        {
            ResolveContext = context;
            this.Log = log;
        }

        protected ILogger Log { get; set; }

        protected ResolveContext ResolveContext { get; set; }

        public object CreateObject(DependencyRegistration registration)
        {
            StringBuilder unresolvedDependenciesMessage = null;
            foreach (var constructor in registration.Constructors)
            {
                var unresolvedDependencies = constructor.Value.Aggregate(
                    new List<ParameterInfo>(),
                    (unresolved, pi) =>
                        {
                            if (!ResolveContext.Registrations.HasRegistrationForService(pi.ParameterType))
                            {
                                unresolved.Add(pi);
                            }

                            return unresolved;
                        });
                if (unresolvedDependencies.Count > 0)
                {
                    this.LogUnresolvedConstructor(unresolvedDependencies, ref unresolvedDependenciesMessage);
                    continue;
                }

                var dependents = from pi in constructor.Value
                                 select ResolveContext.Resolve(pi.ParameterType);

                return this.AssignProperties(constructor.Key.Invoke(dependents.ToArray()));
            }

            throw new DependencyResolutionException(
                "Could not resolve type {0} because its dependencies couldn't be fullfilled\r\n{1}".With(registration.ConcreteType.Name, unresolvedDependenciesMessage));
        }

        private object AssignProperties(object instanceObject)
        {
            foreach (var property in from pi in instanceObject.GetType().GetProperties()
                                     where pi.CanWrite && pi.GetIndexParameters().Length == 0
                                     let reg = ResolveContext.Registrations.GetRegistrationForService(pi.PropertyType)
                                     where reg != null && ResolveContext.CanResolve(reg)
                                     select new { pi, reg })
            {
                property.pi.SetValue(instanceObject, ResolveContext.Resolve(property.reg), null);
            }

            return instanceObject;
        }

        private void LogUnresolvedConstructor(IEnumerable<ParameterInfo> unresolvedDependencies, ref StringBuilder unresolvedDependenciesMessage)
        {
            unresolvedDependenciesMessage = unresolvedDependenciesMessage ?? new StringBuilder();
            string message = unresolvedDependencies.Aggregate(string.Empty, (str, pi) => str + pi.ParameterType);
            this.Log.WriteDebug("Ignoring constructor, following dependencies didn't have a registration:" + message);
            unresolvedDependenciesMessage.Append("Constructor: ").AppendLine(message);
        }
    }
}