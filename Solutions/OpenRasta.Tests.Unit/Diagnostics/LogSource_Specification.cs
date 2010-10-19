namespace OpenRasta.Diagnostics
{
    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Testing.Specifications;

    using NUnit.Framework;

    public class when_reading_log_source_data
    {
        [Test]
        public void the_category_is_read_from_an_attribute()
        {
            LogSource<MockLogSource>.Category.ShouldBe(MockLogSource.CATEGORY_NAME);
        }

        [Test]
        public void a_log_source_without_category_is_named_after_the_type()
        {
            LogSource<LogSourceNoCategory>.Category.ShouldBe("LogSourceNoCategory");
         }

        public class LogSourceNoCategory : ILogSource
        {
        }

        [LogCategory(CATEGORY_NAME)]
        private class MockLogSource : ILogSource
        {
            public const string CATEGORY_NAME = "Mock log source";
        }
    }
}
