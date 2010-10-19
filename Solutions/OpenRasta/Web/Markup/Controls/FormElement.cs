namespace OpenRasta.Web.Markup.Controls
{
    #region Using Directives

    using System;

    using OpenRasta.Collections;
    using OpenRasta.Extensions;
    using OpenRasta.Web.Markup.Elements;
    using OpenRasta.Web.Markup.Modules;
    using OpenRasta.Web.UriDecorators;

    #endregion

    public class FormElement : GenericElement
    {
        private Uri originalAction;
        private string originalMethod;

        public FormElement(bool allowMethodOverrideSyntax) : base("form")
        {
            ContentModel.Clear();
            ContentModel.AddRange(Document.GetContentModelFor<IFormElement>());
            Attributes.AllowedAttributes = Document.GetAllowedAttributesFor<IFormElement>();
            this.IsMethodOverrideActive = allowMethodOverrideSyntax;
        }

        public override Uri Action
        { 
            get
            {
                return this.originalAction;
            }
            set
            {
                if (value == null || IsMethodHtmlFriendly(this.Method))
                {
                    this.originalAction = value;
                    this.Attributes.SetAttribute("action",value);
                }
                else
                {
                    this.originalAction = value;
                    this.Attributes.SetAttribute("action",AddHttpOverrider(value,this.Method));
                }
            }
        }

        public override string Method
        {
            get
            {
                return this.originalMethod ?? Attributes.GetAttribute("method");
            }

            set
            {
                if (!this.IsMethodOverrideActive && !IsMethodHtmlFriendly(value))
                {
                    throw new InvalidOperationException("Cannot use any other method than POST and GET unless you register the {0} uri decorator".With(typeof(HttpMethodOverrideUriDecorator).Name));
                }

                if (this.IsMethodOverrideActive && !IsMethodHtmlFriendly(value))
                {
                    Attributes.SetAttribute("method", "POST");
                    this.originalMethod = value;
                    this.Action = this.Action;
                }
                else
                {
                    Attributes.SetAttribute("method", value);
                    this.originalMethod = null;
                }
            }
        }

        private bool IsMethodOverrideActive { get; set; }

        private static Uri AddHttpOverrider(Uri uri, string httpMethod)
        {
            var builder = new UriBuilder(uri);
            builder.Path += "!" + httpMethod;
            
            return builder.Uri;
        }

        private static bool IsMethodHtmlFriendly(string method)
        {
            return method.EqualsOrdinalIgnoreCase("POST") || method.EqualsOrdinalIgnoreCase("GET");
        }
    }
}