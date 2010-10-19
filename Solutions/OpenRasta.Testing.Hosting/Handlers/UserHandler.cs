namespace OpenRasta.Testing.Hosting.Handlers
{
    using OpenRasta.Security;
    using OpenRasta.Web;

    public class UserHandler
    {
        [RequiresAuthentication]
        public OperationResult Delete(int id)
        {
            return new OperationResult.OK();
        }
    }
}