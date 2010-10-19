namespace OpenRasta.Text
{
    #region Using Directives

    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;

    #endregion

    /// <summary>
    /// Provides partial implementation for decoding strings according to RFC2047.
    /// </summary>
    /// <remarks>
    /// This implementation is not yet conformant to rfc2047.
    /// </remarks>
    public static class Rfc2047Encoding
    {
        public static string DecodeTextToken(string textToDecode)
        {
            StringBuilder decoded = new StringBuilder();
            StringBuilder charsetBuilder = null;
            StringBuilder encodedText = null;

            for (int i = 0; i < textToDecode.Length; i++)
            {
                char ch = textToDecode[i];

                if (ch == '=' && i < textToDecode.Length - 1 && textToDecode[i + 1] == '?')
                {
                    i += 2;
                    charsetBuilder = new StringBuilder();
                    
                    while (i < textToDecode.Length && textToDecode[i] != '?')
                    {
                        charsetBuilder.Append(textToDecode[i]);
                        i++;
                    }
                    
                    i++;
                    string charset = charsetBuilder.ToString();
                    Encoding textEncoder = null;
                    
                    try
                    {
                        textEncoder = Encoding.GetEncoding(charset);
                    }
                    catch
                    {
                    }
                    
                    char encoding = textToDecode[i];
                    Func<string, Encoding, string> decoder = null;
                    
                    if ((encoding == 'Q' || encoding == 'q') && i + 1 < textToDecode.Length && textToDecode[i + 1] == '?')
                    {
                        decoder = DecodeQuotedPrintable;
                    }
                    else if ((encoding == 'B' || encoding == 'b') && i + 1 < textToDecode.Length && textToDecode[i + 1] == '?')
                    {
                        decoder = DecodeBase64;
                    }

                    if (textEncoder != null && decoder != null)
                    {
                        i += 2;
                        encodedText = new StringBuilder();
                        byte[] encodedBuffer = new byte[4];
                        
                        for (; i + 1 < textToDecode.Length && !(textToDecode[i] == '?' && textToDecode[i + 1] == '='); i++)
                        {
                            encodedText.Append(textToDecode[i]);
                        }

                        decoded.Append(decoder(encodedText.ToString(), textEncoder));
                        i += 1;
                    }
                    else
                    {
                        decoded.Append("=?").Append(charset).Append("?").Append(encoding);
                        continue;
                    }
                }
                else
                {
                    decoded.Append(ch);
                }
            }

            return decoded.ToString();
        }

        private static string DecodeQuotedPrintable(string textToDecode, Encoding textEncoder)
        {
            var decode = new MemoryStream(textToDecode.Length);

            for (int i = 0; i < textToDecode.Length; i++)
            {
                byte byteToAdd = 0;
                
                if (textToDecode[i] == '_')
                {
                    byteToAdd = (byte)' ';
                }
                else if (textToDecode[i] == '=' && i + 2 < textToDecode.Length)
                {
                    byteToAdd = byte.Parse(
                        textToDecode[i + 1] + string.Empty + textToDecode[i + 2], 
                        NumberStyles.HexNumber, 
                        CultureInfo.InvariantCulture);
                    i += 2;
                }
                else
                {
                    byteToAdd = (byte)textToDecode[i];
                }

                decode.WriteByte(byteToAdd);
            }

            return textEncoder.GetString(decode.GetBuffer(), 0, (int)decode.Length);
        }

        private static string DecodeBase64(string text, Encoding textEncoder)
        {
            var bytes = Convert.FromBase64String(text);
            
            return textEncoder.GetString(bytes);
        }
    }
}