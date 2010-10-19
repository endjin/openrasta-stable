namespace OpenRasta.Testing.Hosting
{
    public static class Uris
    {
        public const string Files = "/files";
        public const string FilesComplexType = "/files/complexType";
        public const string FilesIfile = "/files/iFile";
        public const string Home = "/";
        public const string Users = "/users";
        public const string USER = "/users/{id}";

        public static string User(int id)
        {
            return "/users/" + id;
        }
    }
}