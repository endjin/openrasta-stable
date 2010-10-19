namespace OpenRasta.Web
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;

    using OpenRasta.Extensions;
    using OpenRasta.Text;

    #endregion

    public class ContentDispositionHeader : IEquatable<ContentDispositionHeader>
    {
        public ContentDispositionHeader(string header)
        {
            var fragments = header.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (fragments.Length == 0)
            {
                throw new FormatException("The header value {0} is invalid for Content-Disposition.".With(header));
            }

            this.Disposition = fragments[0].Trim();

            for (int i = 1; i < fragments.Length; i++)
            {
                var parameter = this.ParseParameter(fragments[i]);
                
                if (string.Compare(parameter.Key, "filename", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.FileName = parameter.Value;
                }
                else if (string.Compare(parameter.Key, "name", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.Name = Rfc2047Encoding.DecodeTextToken(parameter.Value);
                }
            }
        }

        public string FileName { get; set; }

        public string Name { get; set; }

        public string Disposition { get; set; }

        public override string ToString()
        {
            var header = new StringBuilder();
            header.Append(this.Disposition);
            
            if (this.Name != null)
            {
                header.Append("; name=\"").Append(this.Name).Append("\"");
            }
            
            if (this.FileName != null)
            {
                header.Append("; filename=\"").Append(this.FileName).Append("\"");
            }

            return header.ToString();
        }

        public bool Equals(ContentDispositionHeader other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Disposition == other.Disposition && this.Name == other.Name && this.FileName == other.FileName;
        }

        public override int GetHashCode()
        {
            return (this.Disposition ?? string.Empty).GetHashCode() ^ (this.Name ?? string.Empty).GetHashCode() ^ (this.FileName ?? string.Empty).GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is ContentDispositionHeader) || obj == null)
            {
                return false;
            }

            return this.Equals((ContentDispositionHeader)obj);
        }

        private KeyValuePair<string, string> ParseParameter(string fragment)
        {
            var equalIndex = fragment.IndexOf('=');
            
            if (equalIndex == -1)
            {
                throw new FormatException();
            }
            
            var key = fragment.Substring(0, equalIndex).Trim();
            var beginningValue = fragment.IndexOf('"', equalIndex + 1);
            
            if (beginningValue == -1)
            {
                throw new FormatException();
            }

            var endValue = fragment.IndexOf('"', beginningValue + 1);

            if (endValue == -1)
            {
                throw new FormatException();
            }

            return new KeyValuePair<string, string>(key, fragment.Substring(beginningValue + 1, endValue - beginningValue - 1));
        }
    }
}