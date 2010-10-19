namespace OpenRasta.Contracts.Binding
{
    using OpenRasta.Contracts.TypeSystem;

    /// <summary>
    /// Defines a component able to locate an object binder for a member.
    /// </summary>
    public interface IObjectBinderLocator
    {
        /// <summary>
        /// Gets a binder for a member.
        /// </summary>
        /// <param name="member">The member for which to find a binder.</param>
        /// <returns>An instance of an <see cref="IObjectBinder"/> defined for this member.</returns>
        IObjectBinder GetBinder(IMember member);
    }
}