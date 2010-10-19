namespace OpenRasta.Contracts.Binding
{
    #region Using Directives

    using System.Collections.Generic;

    using OpenRasta.Binding;

    #endregion

    /// <summary>
    /// Represents a component able to build instances of objects from object paths and values.
    /// </summary>
    public interface IObjectBinder
    {
        /// <summary>
        /// Gets a value defining if the binder contains any values.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets the list of prefixes that the binder will ignore when parsing the keys and value pairs.
        /// </summary>
        ICollection<string> Prefixes { get; }

        /// <summary>
        /// Tries to set a property value based on a key and a list of values.
        /// </summary>
        /// <typeparam name="TValue">The type of values being used to assign to the destination property.</typeparam>
        /// <param name="key">The object path to the property to assign.</param>
        /// <param name="values">The values to be used in realizing the value for the property</param>
        /// <param name="converter">The converter responsible for converting between the source values and the destination value.</param>
        /// <returns><c>true</c> if the assignment was successful, otherwise <c>false</c>.</returns>
        bool SetProperty<TValue>(string key, IEnumerable<TValue> values, ValueConverter<TValue> converter);

        /// <summary>
        /// Tries to set an instance as the object used when assigning values.
        /// </summary>
        /// <param name="builtInstance">The instance of an object being used for binding.</param>
        /// <returns><c>true</c> if the assignment was successful, otherwise <c>false</c>.</returns>
        bool SetInstance(object builtInstance);

        /// <summary>
        /// Attempts to build an object and return a <see cref="BindingResult"/> instance containing the result of the building.
        /// </summary>
        /// <returns>An instance of the <see cref="BindingResult"/> type containing the result of the binding.</returns>
        BindingResult BuildObject();
    }
}