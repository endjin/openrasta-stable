namespace OpenRasta.Testing.Hosting.TestRunner.Scenarios
{
    using System.Xml.Serialization;

    using OpenBastard;

    using OpenRasta.Testing.Hosting.Resources;
    using OpenRasta.Testing.Hosting.TestRunner.Infrastructure;

    public class manipulating_users : environment_context
    {
        private object XmlResponse;

        private User then_user
        {
            get { return (User)this.XmlResponse; }
        }

        public void can_create_a_user()
        {
            given_request_to(Uris.Users)
                .Post()
                .EntityAsMultipartFormData(
                    FormData.Text("FirstName", "Frodo"), 
                    FormData.Text("LastName", "Baggins")
                );

            when_retrieving_the_response_as_user();

            then_response_should_be_201_created();

            then_user.FirstName.ShouldBe("Frodo");
            then_user.LastName.ShouldBe("Baggins");
            then_user.Id.ShouldNotBeNull();
        }
        
        public void cannot_delete_user_with_wrong_credentials()
        {
            given_request_to(Uris.User(2))
                .Delete()
                .Credentials("username", "wrongpassword");

            when_retrieving_the_response();

            Response.StatusCode.ShouldBe(401);
        }
        
        public void can_delete_user_with_correct_credentials()
        {
            given_request_to(Uris.User(2))
                .Delete()
                .Credentials("username", "password");

            when_retrieving_the_response();

            Response.StatusCode.ShouldBe(200);
        }

        private void then_response_should_be_201_created()
        {
            Response.StatusCode.ShouldBe(201);
        }

        private void when_retrieving_the_response_as_user()
        {
            when_retrieving_the_response_as_xml<User>();
        }

        private void when_retrieving_the_response_as_xml<T>()
        {
            when_retrieving_the_response();
            var serializer = new XmlSerializer(typeof(T));
            this.XmlResponse = serializer.Deserialize(this.Response.Entity.Stream);
        }
    }
}