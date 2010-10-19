namespace OpenRasta.Testing.Hosting.Handlers
{
    using System.Collections.Generic;

    using OpenRasta.Contracts.Web;
    using OpenRasta.Testing.Hosting.Resources;
    using OpenRasta.Web;

    public class UserListHandler
    {
        private static List<User> userRepository = new List<User>();

        public OperationResult Post(User userToAdd)
        {
            this.AddUser(userToAdd);

            return new OperationResult.Created
            {
                RedirectLocation = userToAdd.CreateUri(), 
                ResponseResource = userToAdd
            };
        }

        public OperationResult Put(List<User> users)
        {
            userRepository = users;

            return new OperationResult.OK
            {
                ResponseResource = users
            };
        }

        private void AddUser(User userToAdd)
        {
            userRepository.Add(userToAdd);
            userToAdd.Id = userRepository.Count;
        }
    }
}