namespace OpenRasta.Web
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Mime;

    #endregion

    /// <summary>
    /// Represents an internet media-type as defined by RFC 2046.
    /// </summary>
    public class MediaType : ContentType, IComparable<MediaType>, IEquatable<MediaType>
    {
        public static readonly MediaType ApplicationOctetStream = new MediaType("application/octet-stream");
        public static readonly MediaType ApplicationXWwwFormUrlencoded = new MediaType("application/x-www-form-urlencoded");
        public static readonly MediaType Html = new MediaType("text/html");
        public static readonly MediaType Json = new MediaType("application/json");
        public static readonly MediaType MultipartFormData = new MediaType("multipart/form-data");
        public static readonly MediaType TextPlain = new MediaType("text/plain");
        public static readonly MediaType Xhtml = new MediaType("application/xhtml+xml");
        public static readonly MediaType XhtmlFragment = new MediaType("application/vnd.openrasta.htmlfragment+xml");
        public static readonly MediaType Xml = new MediaType("application/xml");
        public static readonly MediaType Javascript = new MediaType("text/javascript");

        private const int MoveDown = -1;
        private const int MoveUp = 1;

        private float quality;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaType"/> class.
        /// </summary>
        /// <param name="contentType">A <see cref="T:System.String"/>, for example, "text/plain; charset=us-ascii", that contains the internet media type, subtype, and optional parameters.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="contentType"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="contentType"/> is <see cref="F:System.String.Empty"/> ("").
        /// </exception>
        /// <exception cref="T:System.FormatException">
        /// <paramref name="contentType"/> is in a form that cannot be parsed.
        /// </exception>
        public MediaType(string contentType) : base(contentType)
        {
            if (Parameters.ContainsKey("q"))
            {
                float floatResult;
                this.quality = float.TryParse(Parameters["q"], NumberStyles.Float, CultureInfo.InvariantCulture, out floatResult) ? Math.Min(1, Math.Max(0, floatResult)) : 0F;
            }
            else
            {
                this.quality = 1.0F;
            }

            int slashPos = MediaType.IndexOf('/');
            int semiColumnPos = MediaType.IndexOf(';', slashPos);

            this.TopLevelMediaType = this.MediaType.Substring(0, slashPos).Trim();
            this.Subtype = this.MediaType.Substring(slashPos + 1, (semiColumnPos != -1 ? semiColumnPos : this.MediaType.Length) - slashPos - 1).Trim();
        }

        public float Quality
        {
            get
            {
                return this.quality;
            }

            private set
            {
                this.quality = value;

                if (value != 1.0f)
                {
                    Parameters["q"] = value.ToString("0.###");
                }
                else if (Parameters.ContainsKey("q"))
                {
                    Parameters.Remove("q");
                }
            }
        }

        public string TopLevelMediaType { get; private set; }
        
        public string Subtype { get; private set; }
        
        public bool IsWildCard
        {
            get { return this.IsTopLevelWildcard && this.IsSubtypeWildcard; }
        }
        
        public bool IsTopLevelWildcard
        {
            get { return this.TopLevelMediaType == "*"; }
        }
        
        public bool IsSubtypeWildcard
        {
            get { return this.Subtype == "*"; }
        }

        public static IEnumerable<MediaType> Parse(string contentTypeList)
        {
            if (contentTypeList == null)
            {
                return new List<MediaType>();
            }

            return from mediaTypeComponent in contentTypeList.Split(',')
                   let mediatype = new MediaType(mediaTypeComponent.Trim())
                   orderby mediatype descending
                   select mediatype;
        }

        public static bool operator !=(MediaType left, MediaType right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==(MediaType left, MediaType right)
        {
            return Equals(left, right);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result * 397) ^ (this.TopLevelMediaType != null ? this.TopLevelMediaType.GetHashCode() : 0);
                result = (result * 397) ^ (this.Subtype != null ? this.Subtype.GetHashCode() : 0);
                
                foreach (string parameterName in this.Parameters.Keys)
                {
                    result = (result * 397) ^ (parameterName + this.Parameters[parameterName]).GetHashCode();
                }

                return result;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return this.Equals(obj as MediaType);
        }

        public bool Equals(MediaType other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.TopLevelMediaType.Equals(this.TopLevelMediaType) &&
                   other.Subtype.Equals(this.Subtype) &&
                   this.ParametersAreEqual(other);
        }

        public int CompareTo(MediaType other)
        {
            if (other == null)
            {
                return MoveUp;
            }

            if (this.Equals(other))
            {
                return 0;
            }

            // first, always move down */*
            if (this.IsWildCard)
            {
                if (other.IsWildCard)
                {
                    return 0;
                }

                return MoveDown;
            }

            // then sort by quality
            if (this.Quality != other.Quality)
            {
                return this.Quality > other.Quality ? MoveUp : MoveDown;
            }

            // then, if the quality is the same, always move application/xml at the end
            if (MediaType == "application/xml")
            {
                return MoveDown;
            }

            if (other.MediaType == "application/xml")
            {
                return MoveUp;
            }

            if (this.TopLevelMediaType != other.TopLevelMediaType)
            {
                if (this.IsTopLevelWildcard)
                {
                    return MoveDown;
                }

                if (other.IsTopLevelWildcard)
                {
                    return MoveUp;
                }

                return this.TopLevelMediaType.CompareTo(other.TopLevelMediaType);
            }

            if (this.Subtype != other.Subtype)
            {
                if (this.IsSubtypeWildcard)
                {
                    return MoveDown;
                }

                if (other.IsSubtypeWildcard)
                {
                    return MoveUp;
                }

                return this.Subtype.CompareTo(other.Subtype);
            }

            return 0;
        }

        public MediaType WithQuality(float quality)
        {
            var newMediaType = new MediaType(ToString()) { Quality = quality };

            return newMediaType;
        }

        public MediaType WithoutQuality()
        {
            var newMediatype = new MediaType(ToString()) { Quality = 1.0f };

            return newMediatype;
        }

        public bool Matches(MediaType typeToMatch)
        {
            return (typeToMatch.IsTopLevelWildcard || this.IsTopLevelWildcard
                    || this.TopLevelMediaType == typeToMatch.TopLevelMediaType)
                   && (typeToMatch.IsSubtypeWildcard || this.IsSubtypeWildcard || this.Subtype == typeToMatch.Subtype);
        }

        private bool ParametersAreEqual(MediaType other)
        {
            if (other.Parameters.Count != this.Parameters.Count)
            {
                return false;
            }

            foreach (string parameter in other.Parameters.Keys)
            {
                if (!this.Parameters.ContainsKey(parameter) || this.Parameters[parameter] != other.Parameters[parameter])
                {
                    return false;
                }
            }

            return true;
        }

        public class MediaTypeEqualityComparer : IEqualityComparer<MediaType>
        {
            public bool Equals(MediaType x, MediaType y)
            {
                return x.Matches(y);
            }

            public int GetHashCode(MediaType obj)
            {
                return obj.TopLevelMediaType.GetHashCode() ^ obj.Subtype.GetHashCode();
            }
        }
    }
}