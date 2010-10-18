namespace OpenRasta.TypeSystem.Surrogated
{
    using System.Collections.Generic;

    using OpenRasta.TypeSystem.Surrogates;

    /// <summary>
    /// Represents an object that keeps a list of surrogate instances for members, usually for usage in member builders.
    /// </summary>
    public interface IKeepSurrogateInstances
    {
        /// <summary>
        /// Gets a dictionary associating a surrogate instance to a member, so the same one can be reused.
        /// </summary>
        IDictionary<IMember, ISurrogate> Surrogates { get; }
    }
}