namespace OpenRasta.Web.Wadl
{
    using System.Collections.ObjectModel;

    public class WadlApplication
    {
        public WadlApplication()
        {
            this.Resources = new WadlResourceCollection();
        }

        public WadlResourceCollection Resources { get; set; }
    }
    
    public class WadlResourceCollection : Collection<WadlResource>
    {
        public string BasePath { get; set; }   
    }

    public class WadlResource
    {
        public WadlResource()
        {
            this.Parameters = new Collection<WadlResourceParameter>();
        }

        public string Path { get; set; }

        public string QueryType { get; set; }

        public Collection<WadlResourceParameter> Parameters { get; set; }
    }
    
    public class WadlResourceParameter
    {
        public WadlResourceParameterStyle Style { get; set; }

        public string Name { get; set; }
    }

    public enum WadlResourceParameterStyle
    {
        Template,
        Matrix,
        Query
    }
}