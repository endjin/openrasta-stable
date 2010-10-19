namespace OpenRasta.Pipeline
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.Codecs.Framework;
    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Web;

    #endregion

    /// <summary>
    /// </summary>
    /// <remarks>Need to inherit from a yet to be created SafeDictionary</remarks>
    public class PipelineData : Dictionary<object, object>
    {
        /// <summary>
        /// Gets the type of the handler selected when matching a request against the registerd resource.
        /// </summary>
        public Type HandlerType
        {
            get { return SafeGet<Type>(PipelineDataConstants.HandlerType); }
            set { base[PipelineDataConstants.HandlerType] = value; }
        }

        public IEnumerable<IOperation> Operations
        {
            get { return SafeGet<IEnumerable<IOperation>>(PipelineDataConstants.Operations); }
            set { base[PipelineDataConstants.Operations] = value; }
        }

        /// <summary>
        /// Gets the resource key associated with the requestURI. 
        /// </summary>
        public object ResourceKey
        {
            get { return SafeGet<object>(PipelineDataConstants.ResourceKey); }
            set { base[PipelineDataConstants.ResourceKey] = value; }
        }

        /// <summary>
        /// Gets the Codec associated with the response entity.
        /// </summary>
        public CodecRegistration ResponseCodec
        {
            get { return SafeGet<CodecRegistration>(PipelineDataConstants.ResponseCodec); }
            set { base[PipelineDataConstants.ResponseCodec] = value; }
        }

        public ICollection<IType> SelectedHandlers
        {
            get { return SafeGet<ICollection<IType>>(PipelineDataConstants.SelectedHandlers); }
            set { base[PipelineDataConstants.SelectedHandlers] = value; }
        }

        /// <summary>
        /// Provides access to the matched resource registration for a request URI.
        /// </summary>
        public UriRegistration SelectedResource
        {
            get { return SafeGet<UriRegistration>(PipelineDataConstants.SelectedResource); }
            set { base[PipelineDataConstants.SelectedResource] = value; }
        }

        public PipelineStage PipelineStage
        {
            get { return SafeGet<PipelineStage>(PipelineDataConstants.PipelineState); }
            set { base[PipelineDataConstants.PipelineState] = value; }
        }

        public new object this[object key]
        {
            get { return ContainsKey(key) ? base[key] : null; }
            set { base[key] = value; }
        }

        private T SafeGet<T>(string key) where T : class
        {
            object o;

            return TryGetValue(key, out o) ? o as T : null;
        }
    }
}