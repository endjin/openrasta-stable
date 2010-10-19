namespace OpenRasta.Web.Markup.Rendering
{
    #region Using Directives

    using OpenRasta.Collections;
    using OpenRasta.Contracts.Web.Markup;
    using OpenRasta.Contracts.Web.Markup.Attributes;
    using OpenRasta.Contracts.Web.Markup.Rendering;

    #endregion

    public class XhtmlNodeWriter
    {
        public  virtual void WriteStartTag(IXhtmlWriter writer, IElement element)
        {
            if (!string.IsNullOrEmpty(element.TagName))
            {
                writer.BeginWriteStartElement(element.TagName);
                element.Attributes.ForEach(a => this.WriteAttribute(writer, a));

                if (element.ContentModel.Count == 0)
                {
                    writer.EndWriteStartElementFinal();
                }
                else
                {
                    writer.EndWriteStartElement();
                }
            }
        }

        protected virtual void WriteAttribute(IXhtmlWriter writer, IAttribute attribute)
        {
            if (!attribute.IsDefault || attribute.RendersOnDefaultValue)
            {
                writer.WriteAttributeString(attribute.Name.ToLowerInvariant(), attribute.SerializedValue);
            }
        }

        public virtual void WriteChildren(IXhtmlWriter writer, IElement element)
        {
            if (element.ChildNodes.Count > 0)
            {
                element.ChildNodes.ForEach(child => Write(writer, child));
            }
        }

        public virtual void WriteEndTag(IXhtmlWriter writer, IElement element)
        {
            if (element.ContentModel.Count > 0)
            {
                writer.WriteEndElement(element.TagName);
            }
        }

        public void Write(IXhtmlWriter writer, INode element)
        {
            if (element is ITextNode)
            {
                writer.WriteString(((ITextNode)element).Text);
            }
            else if (element is IAttribute)
            {
                this.WriteAttribute(writer, (IAttribute)element);
            }
            else if (element is IElement)
            {
                var el = (IElement)element;
                el.Prepare();
                
                if (!el.IsVisible)
                {
                    return;
                }

                this.WriteStartTag(writer, el);
                this.WriteChildren(writer, el);
                this.WriteEndTag(writer, el);
            }
        }
    }
}