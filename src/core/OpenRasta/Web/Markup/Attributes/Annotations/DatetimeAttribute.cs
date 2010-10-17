namespace OpenRasta.Web.Markup.Attributes.Annotations
{
    using System;

    public class DatetimeAttribute : PrimaryTypeAttributeCore
    {
        public DatetimeAttribute() : base(Factory<DateTime?>)
        {
        }

        public DatetimeAttribute(string attribName) : base(attribName, Factory<DateTime?>)
        {
        }
    }
}