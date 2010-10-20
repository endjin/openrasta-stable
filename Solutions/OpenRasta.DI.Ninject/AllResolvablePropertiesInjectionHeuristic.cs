namespace OpenRasta.DI.Ninject
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Reflection;

    using global::Ninject;
    using global::Ninject.Components;
    using global::Ninject.Parameters;
    using global::Ninject.Selection.Heuristics;

    #endregion

    /// <summary>
    /// Determines whether members should be injected during activation by checking
    /// if they provide a public setter and have an existing binding.
    /// </summary>
    public class AllResolvablePropertiesInjectionHeuristic : NinjectComponent, IInjectionHeuristic
    {
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllResolvablePropertiesInjectionHeuristic"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public AllResolvablePropertiesInjectionHeuristic(IKernel kernel)
        {
            this.kernel = kernel;
        }

        /// <summary>
        /// Returns a value indicating whether the specified member should be injected.
        /// </summary>
        /// <param name="member">The member in question.</param>
        /// <returns>
        ///     <see langword="true"/> if the member should be injected; otherwise <see langword="false"/>.
        /// </returns>
        public bool ShouldInject(MemberInfo member)
        {
            if (member.MemberType != MemberTypes.Property)
            {
                return false;
            }

            var propertyInfo = member as PropertyInfo;
            if (propertyInfo == null)
            {
                return false;
            }

            if (propertyInfo.GetSetMethod() == null)
            {
                return false;
            }

            // If the types are the same, or if the property is an interface or abstract class
            // that the declaring type implements (which would cause a cyclic resolution)
            if ((propertyInfo.PropertyType == propertyInfo.DeclaringType)
                || propertyInfo.DeclaringType.IsAssignableFrom(propertyInfo.PropertyType))
            {
                return false;
            }

            var request = this.kernel.CreateRequest(propertyInfo.PropertyType, null, EmptyParameters, true);
            
            return this.kernel.CanResolve(request);
        }

        private static readonly IEnumerable<IParameter> EmptyParameters = new IParameter[] { };
    }
}