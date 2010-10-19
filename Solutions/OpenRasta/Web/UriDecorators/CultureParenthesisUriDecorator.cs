namespace OpenRasta.Web.UriDecorators
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Web;
    using OpenRasta.Contracts.Web.UriDecorators;

    #endregion

    public class CultureParenthesisUriDecorator : IUriDecorator
    {
        private ICommunicationContext context;
        
        public CultureParenthesisUriDecorator(ICommunicationContext context)
        {
            this.context = context;
        }
        
        public bool Parse(Uri uri, out Uri processedUri)
        {
            processedUri = uri;
            
            // TODO:Provide an implementation, maybe? 
            return false;
        }

        public void Apply()
        {
        }
    }
}