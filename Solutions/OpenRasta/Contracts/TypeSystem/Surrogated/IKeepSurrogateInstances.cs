namespace OpenRasta.Contracts.TypeSystem.Surrogated
{
    #region Using Directives

    using System.Collections.Generic;

    using OpenRasta.Contracts.TypeSystem.Surrogates;

    #endregion

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