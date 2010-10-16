// port from mono code. See http://anonsvn.mono-project.com/viewvc/trunk/mcs/class/System.Web/System.Web/HttpUtility.cs?view=markup

namespace OpenRasta.Codecs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using OpenRasta.Binding;
    using OpenRasta.Collections;
    using OpenRasta.Pipeline;
    using OpenRasta.TypeSystem;
    using OpenRasta.TypeSystem.ReflectionBased;
    using OpenRasta.Web;

    public abstract class AbstractApplicationXWwwFormUrlencodedCodec
    {
        private const string FormdataCache = "__ApplicationXWwwUrlformEncodedCodec_FORMDATA_CACHED";

        private readonly PipelineData pipelineData;

        private IObjectBinderLocator binderLocator;

        protected AbstractApplicationXWwwFormUrlencodedCodec(ICommunicationContext context, IObjectBinderLocator locator)
        {
            this.pipelineData = context.PipelineData;
            this.BinderLocator = locator;
        }

        public IObjectBinderLocator BinderLocator
        {
            get
            {
                return this.binderLocator;
            }
            set
            {
                this.binderLocator = value;
            }
        }

        public IDictionary<IHttpEntity, Dictionary<string, string[]>> Cache
        {
            get
            {
                return (this.pipelineData[FormdataCache]
                        ?? (this.pipelineData[FormdataCache] = new NullBehaviorDictionary<IHttpEntity, Dictionary<string, string[]>>())) as
                       IDictionary<IHttpEntity, Dictionary<string, string[]>>;
            }
        }

        public object Configuration { get; set; }

        public static string UrlDecode(string s, Encoding e)
        {
            if (null == s)
            {
                return null;
            }

            if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
            {
                return s;
            }

            if (e == null)
            {
                e = Encoding.UTF8;
            }

            var output = new StringBuilder();
            long len = s.Length;
            var bytes = new MemoryStream();

            for (int i = 0; i < len; i++)
            {
                if (s[i] == '%' && i + 2 < len && s[i + 1] != '%')
                {
                    int xchar;
                    if (s[i + 1] == 'u' && i + 5 < len)
                    {
                        if (bytes.Length > 0)
                        {
                            output.Append((char[])GetChars(bytes, e));
                            bytes.SetLength(0);
                        }

                        xchar = GetChar(s, i + 2, 4);
                        if (xchar != -1)
                        {
                            output.Append((char)xchar);
                            i += 5;
                        }
                        else
                        {
                            output.Append('%');
                        }
                    }
                    else if ((xchar = GetChar(s, i + 1, 2)) != -1)
                    {
                        bytes.WriteByte((byte)xchar);
                        i += 2;
                    }
                    else
                    {
                        output.Append('%');
                    }

                    continue;
                }

                if (bytes.Length > 0)
                {
                    output.Append((char[])GetChars(bytes, e));
                    bytes.SetLength(0);
                }

                if (s[i] == '+')
                {
                    output.Append(' ');
                }
                else
                {
                    output.Append(s[i]);
                }
            }

            if (bytes.Length > 0)
            {
                output.Append((char[])GetChars(bytes, e));
            }

            bytes = null;
            return output.ToString();
        }

        public IEnumerable<KeyedValues<string>> ReadKeyValues(IHttpEntity entity)
        {
            foreach (string keyName in this.FormData(entity).Keys.ToArray())
            {
                var key = new KeyedValues<string>(keyName, this.FormData(entity)[keyName], ConvertValuesByString);
                
                yield return key;

                if (key.WasUsed)
                {
                    this.FormData(entity).Remove(keyName);
                }
            }
        }

        protected static BindingResult ConvertValuesByString(string strings, Type entityType)
        {
            try
            {
                return BindingResult.Success(entityType.CreateInstanceFrom(strings));
            }
            catch (NotSupportedException)
            {
                return BindingResult.Failure();
            }
        }

        protected static bool IsRawDictionary(IType destinationType)
        {
            return destinationType.IsAssignableTo<IDictionary<string, string[]>>();
        }

        protected Dictionary<string, string[]> FormData(IHttpEntity source)
        {
            if (this.Cache[source] == null)
            {
                // TODO: detect proper encoding based on the charset and content sniffing
                var reader = new StreamReader(source.Stream, Encoding.UTF8);
                this.Cache[source] = this.ReadKeyValueData(Encoding.UTF8, reader);
            }

            return this.Cache[source];
        }

        private static void AddKeyValue(Encoding e, Dictionary<string, string[]> dic, StringBuilder key, StringBuilder value)
        {
            string keyString = UrlDecode(key.ToString(), e);
            string valueString = UrlDecode(value.ToString(), e);
            string[] oldValue;
            string[] newValue;
            if (!dic.TryGetValue(keyString, out oldValue))
            {
                newValue = new[] { valueString };
            }
            else
            {
                newValue = new string[oldValue.Length + 1];
                oldValue.CopyTo(newValue, 0);
                newValue[newValue.Length - 1] = valueString;
            }
            dic[keyString] = newValue;
            key.Length = 0;
            value.Length = 0;
        }

        private static int GetChar(byte[] bytes, int offset, int length)
        {
            int value = 0;
            int end = length + offset;

            for (int i = offset; i < end; i++)
            {
                int current = GetInt(bytes[i]);

                if (current == -1)
                {
                    return -1;
                }

                value = (value << 4) + current;
            }

            return value;
        }

        private static int GetChar(string str, int offset, int length)
        {
            int val = 0;
            int end = length + offset;

            for (int i = offset; i < end; i++)
            {
                char c = str[i];
                
                if (c > 127)
                {
                    return -1;
                }

                int current = GetInt((byte)c);
                
                if (current == -1)
                {
                    return -1;
                }

                val = (val << 4) + current;
            }

            return val;
        }

        private static char[] GetChars(MemoryStream b, Encoding e)
        {
            return e.GetChars(b.GetBuffer(), 0, (int)b.Length);
        }

        private static int GetInt(byte b)
        {
            char c = (char)b;

            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }

            if (c >= 'a' && c <= 'f')
            {
                return c - 'a' + 10;
            }

            if (c >= 'A' && c <= 'F')
            {
                return c - 'A' + 10;
            }

            return -1;
        }

        private Dictionary<string, string[]> ReadKeyValueData(Encoding contentEncoding, StreamReader reader)
        {
            var keyValues = new Dictionary<string, string[]>();
            var key = new StringBuilder();
            var value = new StringBuilder();

            int c;
            while ((c = reader.Read()) != -1)
            {
                if (c == '=')
                {
                    value.Length = 0;

                    while ((c = reader.Read()) != -1)
                    {
                        if (c == '&')
                        {
                            AddKeyValue(contentEncoding, keyValues, key, value);
                            break;
                        }
                        
                        value.Append((char)c);
                    }

                    if (c == -1)
                    {
                        AddKeyValue(contentEncoding, keyValues, key, value);
                        break;
                    }
                }
                else if (c == '&')
                {
                    AddKeyValue(contentEncoding, keyValues, key, value);
                }
                else
                {
                    key.Append((char)c);
                }
            }

            return keyValues;
        }
    }
}