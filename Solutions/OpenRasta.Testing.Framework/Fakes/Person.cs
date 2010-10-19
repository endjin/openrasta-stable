namespace OpenRasta.Testing.Framework.Fakes
{
    using System;

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Username { get; set; }
        public Address Address { get; set; }
    }
}