namespace OpenRasta.Web.Markup.Extensions
{
    #region Using Directives

    using System;

    using OpenRasta.Web.Markup.Modules;

    #endregion

    public static class EditModuleExtensions
    {
        public static T DateTime<T>(this T element, DateTime dateTimes) where T : IEditElement
        {
            element.DateTime = dateTimes;

            return element;
        }
    }
}