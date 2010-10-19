namespace OpenRasta.Web.Markup.Extensions
{
    #region Using Directives

    using System;

    using OpenRasta.Web.Markup.Modules;

    #endregion

    public static class FormModuleExtensions
    {
        public static T AcceptCharset<T>(this T element, string charset) where T : IFormElement
        {
            element.AcceptCharset.Add(charset);
            
            return element;
        }

        public static T Action<T>(this T element, string actionUri) where T : IFormElement
        {
            element.Action = new Uri(actionUri);
            
            return element;
        }

        public static T Action<T>(this T element, Uri actionUri) where T : IFormElement
        {
            element.Action = actionUri;
            
            return element;
        }

        public static T Method<T>(this T element, string httpMethod) where T : IFormElement
        {
            element.Method = httpMethod;
        
            return element;
        }

        public static T EncType<T>(this T element, string mediaType) where T : IFormElement
        {
            element.EncType = new MediaType(mediaType);
            
            return element;
        }

        public static T EncType<T>(this T element, MediaType mediaType) where T : IFormElement
        {
            element.EncType = mediaType;
            
            return element;
        }

        public static T InputType<T>(this T element, InputType inputType) where T : IInputElement
        {
            element.Type = inputType;
            
            return element;
        }

        public static T MaxLength<T>(this T element, int maxLength) where T : IInputTextElement
        {
            element.MaxLength = maxLength;
            
            return element;
        }

        public static T Checked<T>(this T element) where T : IInputCheckedElement
        {
            element.Checked = true;
            
            return element;
        }

        public static T Multiple<T>(this T element) where T : ISelectElement
        {
            element.Multiple = true;
            
            return element;
        }

        public static T Selected<T>(this T element) where T : IOptionElement
        {
            element.Selected = true;
            
            return element;
        }

        public static T Cols<T>(this T element, int columns) where T : ITextAreaElement
        {
            element.Cols = columns;
            
            return element;
        }

        public static T Rows<T>(this T element, int rows) where T : ITextAreaElement
        {
            element.Rows = rows;

            return element;
        }

        public static T InputType<T>(this T element, ButtonType type) where T : IButtonElement
        {
            element.Type = type;
            
            return element;
        }

        public static T Submit<T>(this T element) where T : IButtonElement
        {
            return element.InputType(ButtonType.Submit);
        }
        
        public static T Reset<T>(this T element) where T : IButtonElement
        {
            return element.InputType(ButtonType.Reset);
        }

        public static T For<T>(this T element, string id) where T : ILabelElement
        {
            element.For = id;
            
            return element;
        }
    }
}