namespace OpenRasta.Data
{
    #region Using Directives

    using System.Collections.Generic;

    using OpenRasta.Binding;
    using OpenRasta.Contracts.Binding;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.TypeSystem;

    #endregion

    /// <summary>
    /// Represents a set of changes that can be applied to a type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChangeSet<T> where T : class
    {
        public ChangeSet(ITypeBuilder typeBuilder)
        {
            this.TypeBuilder = typeBuilder;
        }

        /// <summary>
        /// Gets the list of changes to be applied to an object.
        /// </summary>
        public IDictionary<string, IPropertyBuilder> Changes
        {
            get { return this.TypeBuilder.Changes; }
        }

        public ITypeBuilder TypeBuilder { get; private set; }

        /// <summary>
        /// Gets the binder used to build the changeset.
        /// </summary>
        /// <param name="typeSystem"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static IObjectBinder GetBinder(ITypeSystem typeSystem, IMember member)
        {
            var innerMember = typeSystem.FromClr<T>();
            return new ChangeSetBinder<T>(innerMember.Type, member.Name);
        }

        /// <summary>
        /// Applies the changes in this <see cref="ChangeSet{T}"/> to the provided instance.
        /// </summary>
        /// <param name="testObject">The instance of an object on which to apply the changes.</param>
        public void Apply(T testObject)
        {
            this.TypeBuilder.Update(testObject);
        }
    }
}