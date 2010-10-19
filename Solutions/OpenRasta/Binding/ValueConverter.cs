namespace OpenRasta.Binding
{
    using System;

    /// <summary>
    /// Defines a conversion method used to convert entities between multiple types.
    /// </summary>
    /// <typeparam name="T">The type of the entity to convert.</typeparam>
    /// <param name="entity">The entity instance to convert.</param>
    /// <param name="entityType">The target Type</param>
    /// <returns>The result of the conversion.</returns>
    public delegate BindingResult ValueConverter<T>(T entity, Type entityType);
}