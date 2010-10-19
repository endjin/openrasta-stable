namespace OpenRasta.Testing.Hosting.Handlers
{
    using OpenRasta.Authentication;
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