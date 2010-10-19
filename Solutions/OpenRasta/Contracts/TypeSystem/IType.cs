namespace OpenRasta.Contracts.TypeSystem
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.Binding;

    #endregion

    /// <summary>
    /// Represents a type used to manipulate objects.
    /// </summary>
    public interface IType : IMember, IComparable<IType>
    {
        /// <summary>
        /// Tries to create an instance of this type.
        /// </summary>
        /// <typeparam name="T">The type of the values for which conversion will occur.</typeparam>
        /// <param name="values">The values to use in creating the instance</param>
        /// <param name="converter">A converter method responsible for converting between the source type <typeparamref name="T"/> and a destination type.</param>
        /// <param name="result">The fully-built object.</param>
        /// <returns><c>true</c> if the object could be created from the provided values, otherwise <c>false</c>.</returns>
        bool TryCreateInstance<T>(IEnumerable<T> values, ValueConverter<T> converter, out object result);
        
        /// <summary>
        /// Tries to create an empty instance of this type.
        /// </summary>
        /// <returns>An instance of this type.</returns>
        object CreateInstance();
        
        /// <summary>
        /// Returns a builder used to aggregate values to assign to an object of this type.
        /// </summary>
        /// <returns>A builder.</returns>
        ITypeBuilder CreateBuilder();

        /// <summary>
        /// Gets a value defining if the provided type can be assigned to a value of this type.
        /// </summary>
        /// <param name="type">The type to assign to a value of this type.</param>
        /// <returns><c>true</c> if the types are compatible, otherwise <c>false</c>.</returns>
        bool IsAssignableFrom(IType type);
    }
}