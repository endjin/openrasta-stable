namespace OpenRasta.Web.Wadl
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Handlers;
    using OpenRasta.Contracts.Web;
    using OpenRasta.DI;

    #endregion

    public class WadlHandler
    {
        private readonly IDependencyResolver resolver;
        private readonly IUriResolver uriRepository;
        private IHandlerRepository handlerRepository;

        public WadlHandler(IDependencyResolver resolver, IUriResolver uriRepository, IHandlerRepository handlerRepository)
        {
            this.resolver = resolver;
            this.uriRepository = uriRepository;
            this.handlerRepository = handlerRepository;
        }
        
        public WadlApplication Get()
        {
            var templateProcessor = this.uriRepository as IUriTemplateParser;
            
            if (templateProcessor == null)
            {
                throw new InvalidOperationException("The system doesn't have a IUriTemplateParser, WADL generation cannot proceed.");
            }

            var app = new WadlApplication
                          {
                              Resources =
                                  {
                                      BasePath = this.resolver.Resolve<ICommunicationContext>().ApplicationBaseUri.ToString()
                                  }
                          };

            foreach (var uriMap in this.uriRepository)
            {
                var resource = new WadlResource { Path = uriMap.UriTemplate };

                var templateParameters = templateProcessor.GetTemplateParameterNamesFor(uriMap.UriTemplate);
                var queryParameters = templateProcessor.GetQueryParameterNamesFor(uriMap.UriTemplate);
                
                resource.Parameters = new System.Collections.ObjectModel.Collection<WadlResourceParameter>();
                foreach (string parameter in templateParameters)
                {
                    resource.Parameters.Add(new WadlResourceParameter { Style = WadlResourceParameterStyle.Template, Name = parameter });
                }

                foreach (string parameter in queryParameters)
                {
                    resource.Parameters.Add(new WadlResourceParameter { Style = WadlResourceParameterStyle.Query, Name = parameter });
                }

                // TODO: For each parameter, lookup the parameter type from the matched handler and include the xsd type in it
                app.Resources.Add(resource);
            }

            return app;
        }
    }
}