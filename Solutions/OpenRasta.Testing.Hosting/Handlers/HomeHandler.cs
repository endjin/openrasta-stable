namespace OpenRasta.Testing.Hosting.Handlers
{
    using OpenRasta.Testing.Hosting.Resources;

    public class HomeHandler
    {
        public Home Get()
        {
            return new Home();
        }
    }
}