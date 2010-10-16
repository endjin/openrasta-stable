namespace OpenRasta.Diagnostics
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class LogCategoryAttribute : Attribute
    {
        public LogCategoryAttribute(string categoryName)
        {
            this.CategoryName = categoryName;
        }

        public string CategoryName { get; private set; }
    }
}