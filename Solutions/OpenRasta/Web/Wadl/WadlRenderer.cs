namespace OpenRasta.Web.Configuration.Wadl
{
    // no use for WADL for the moment, will review later if its worth including.
    
    //public class WadlRenderer : XmlCodec
    //{
    //    IDisposable _closer;
    //    private const string NS_WADL = "http://research.sun.com/wadl/2006/10";
    //    public override string ContentType { get { return "application/vnd.sun.wadl+xml"; } }
    //    public override void WriteToCore(IRastaContext context)
    //    {
    //        _closer = new ElementCloser(Writer);

    //        WadlApplication application = context.Request.Entity.EntityInstance as WadlApplication;

    //        using (Application())
    //        {
    //            using (Resources(application.Resources.BasePath))
    //            {
    //                foreach (var resource in application.Resources)
    //                {
    //                    using (Resource(resource.Path))
    //                    {
    //                        foreach (var param in resource.Parameters)
    //                            Param(param.Name, param.Style.ToString().ToLowerInvariant());

    //                        Method("GET");
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    protected IDisposable Application()
    //    {
    //        Writer.WriteStartElement("application", NS_WADL);
    //        return new ElementCloser(Writer);
    //    }
    //    protected IDisposable Resources(string @base)
    //    {
    //        Writer.WriteStartElement("resources", NS_WADL);
    //        return new ElementCloser(Writer);
            

    //    }
    //    protected IDisposable Resource(string path)
    //    {
    //        Writer.WriteStartElement("resource", NS_WADL);
    //        Writer.WriteAttributeString("base", path);
    //        return new ElementCloser(Writer);
    //    }
    //    protected void Param(string name, string style)
    //    {
    //        Writer.WriteStartElement("param", NS_WADL);
    //        Writer.WriteAttributeString("name", name);
    //        Writer.WriteAttributeString("style", style);
    //        Writer.WriteEndElement();
    //    }
    //    protected void Method(string operation)
    //    {
    //        Writer.WriteStartElement("method", NS_WADL);
    //        Writer.WriteAttributeString("name", operation);
    //        Writer.WriteEndElement();
    //    }
    //    protected void Request()
    //    {

    //    }
    //    protected void End()
    //    {
    //        Writer.WriteEndElement();
    //    }
    //    private class ElementCloser : IDisposable
    //    {
    //        private XmlWriter _writer;
    //        public ElementCloser(XmlWriter writer)
    //        {
    //            _writer = writer;
    //        }
    //        public void Dispose()
    //        {
    //            _writer.WriteEndElement();
    //        }
    //    }
    //}
}