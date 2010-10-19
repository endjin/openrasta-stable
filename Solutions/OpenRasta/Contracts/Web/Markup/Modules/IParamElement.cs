namespace OpenRasta.Web.Markup.Modules
{
    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Web.Markup.Attributes;

    /// <summary>
    /// Represents the &lt;param&gt; element
    /// </summary>
    public interface IParamElement : IElement,
                                     IIDAttribute,
                                     INameAttribute,
                                     ITypeAttribute,
                                     IValueAttribute
    {
        [ParamValueTypeAttribute]
        ParamValueType ValueType { get; set; }
    }
}