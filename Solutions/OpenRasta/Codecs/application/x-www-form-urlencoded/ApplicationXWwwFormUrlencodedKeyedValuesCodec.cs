// port from mono code. See http://anonsvn.mono-project.com/viewvc/trunk/mcs/class/System.Web/System.Web/HttpUtility.cs?view=markup
namespace OpenRasta.Codecs
{
    #region Using Directives

    using OpenRasta.Codecs.Attributes;
    using OpenRasta.Contracts.Binding;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.Web;

    #endregion

    [MediaType("application/x-www-form-urlencoded;q=0.5")]
    [SupportedType(typeof(object))]
    public class ApplicationXWwwFormUrlencodedKeyedValuesCodec : AbstractApplicationXWwwFormUrlencodedCodec, IKeyedValuesMediaTypeReader<string>
    {
        public ApplicationXWwwFormUrlencodedKeyedValuesCodec(ICommunicationContext context, IObjectBinderLocator locator) : base(context, locator)
        {
        }
    }
}