namespace OpenRasta.DI.Ninject
{
    using System;
    using System.Reflection;

    using global::Ninject;
    using global::Ninject.Parameters;

    /// <summary>
    /// Extensions to enable more readable code.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Determines whether the specified service type is bindable.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="kernel">The kernel.</param>
        /// <returns>
        /// 	<see langword="true"/> if the specified service type is bindable; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsBindable(this Type serviceType, IKernel kernel)
        {
            var request = kernel.CreateRequest(serviceType, null, new IParameter[] { }, false);
            return kernel.CanResolve(request);
        }

        /// <summary>
        /// Determines whether the specified member has a particular attribute.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <param name="type">The attribute type to check for.</param>
        /// <returns>
        /// 	<see langword="true"/> if the specified member has attribute; otherwise, <see langword="true"/>.
        /// </returns>
        public static bool HasAttribute(this ICustomAttributeProvider member, Type type)
        {
            return member.IsDefined(type, true);
        }

        /// <summary>
        /// Gets a unique key for a given type.
        /// </summary>
        /// <returns>Returns the AssemblyQualifiedName.</returns>
        public static string GetKey(this Type type)
        {
            return type.AssemblyQualifiedName;
        }
    }
}