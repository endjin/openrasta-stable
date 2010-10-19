namespace CollectionExtensions_Specification
{
    #region Using Directives

    using System;

    using NUnit.Framework;

    using OpenRasta.Collections.Specialized;
    using OpenRasta.Testing.Specifications;

    #endregion

    public class when_converting_to_nvc : context
    {
        [Test]
        public void passing_a_null_ojbect_results_in_an_error()
        {
            object target = null;
            Executing(() => target.ToNameValueCollection()).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void method_name()
        {
        }
    }
}