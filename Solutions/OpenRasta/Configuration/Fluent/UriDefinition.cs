namespace OpenRasta.Configuration.Fluent
{
    #region Using Directives

    using System;
    using System.Globalization;

    using OpenRasta.Configuration.MetaModel;
    using OpenRasta.Contracts.Configuration.Fluent;
    using OpenRasta.Contracts.TypeSystem;

    #endregion

    public class UriDefinition : IUriDefinition
    {
        private readonly ResourceDefinition resourceDefinition;
        private readonly UriModel uriModel;

        public UriDefinition(ResourceDefinition resourceDefinition, string uri)
        {
            this.resourceDefinition = resourceDefinition;
            this.uriModel = new UriModel { Uri = uri };
            this.resourceDefinition.Registration.Uris.Add(this.uriModel);
        }

        public IResourceDefinition And
        {
            get { return this.resourceDefinition; }
        }

        public IHandlerForResourceWithUriDefinition HandledBy<T>()
        {
            return this.resourceDefinition.HandledBy<T>();
        }

        public IHandlerForResourceWithUriDefinition HandledBy(Type type)
        {
            return this.resourceDefinition.HandledBy(type);
        }

        public IHandlerForResourceWithUriDefinition HandledBy(IType type)
        {
            return this.resourceDefinition.HandledBy(type);
        }

        public IUriDefinition InLanguage(string language)
        {
            this.uriModel.Language = language == null
                                     ? CultureInfo.InvariantCulture
                                     : CultureInfo.GetCultureInfo(language);
            return this;
        }

        public IUriDefinition Named(string uriName)
        {
            this.uriModel.Name = uriName;
            return this;
        }
    }
}