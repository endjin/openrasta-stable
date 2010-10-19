namespace OpenRasta.Testing.Framework.MockTypes
{
    using System;
    using System.Collections.Generic;

    public class Customer
    {
        public Customer() { }
        public Customer(string firstname) { FirstName = firstname; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<string> Attributes { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
    }
}