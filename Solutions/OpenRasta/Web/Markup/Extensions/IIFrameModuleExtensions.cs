namespace OpenRasta.Web.Markup
{
    using OpenRasta.Web.Markup.Modules;

    public static class IIFrameModuleExtensions
    {
        public static T FrameBorder<T>(this T element, bool frameBorder) where T : IIFrameElement
        {
            element.FrameBorder = frameBorder;
            
            return element;
        }

        public static T MarginWidth<T>(this T element, int marginWidth) where T : IIFrameElement
        {
            element.MarginWidth = marginWidth;
            
            return element;
        }
        
        public static T MarginHeight<T>(this T element, int marginHeight) where T : IIFrameElement
        {
            element.MarginHeight = marginHeight;
            
            return element;
        }
        
        public static T Scrolling<T>(this T element, Scrolling scrolling) where T : IIFrameElement
        {
            element.Scrolling = scrolling;
            
            return element;
        }
    }
}