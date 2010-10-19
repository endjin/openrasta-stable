namespace OpenRasta.Contracts.Web
{
    using System.Collections.Generic;

    public interface IUriTemplateParser
    {
        IEnumerable<string> GetQueryParameterNamesFor(string uriTemplate);

        IEnumerable<string> GetTemplateParameterNamesFor(string uriTemplate);
    }
}