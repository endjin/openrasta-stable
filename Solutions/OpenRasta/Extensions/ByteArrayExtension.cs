namespace OpenRasta.Extensions
{
    public static class ByteArrayExtension
    {
        public static MatchResult Match(this byte[] source, byte[] marker)
        {
            return Match(source, 0L, marker, 0L, source.LongLength);
        }

        public static MatchResult Match(this byte[] source, long sourceIndex, byte[] marker, long count)
        {
            return Match(source, sourceIndex, marker, 0L, count);
        }

        public static MatchResult Match(this byte[] source, long sourceIndex, byte[] marker, long markerIndex, long count)
        {
            long endOfArray = sourceIndex + count > source.Length ? source.Length : sourceIndex + count;
            
            for (long sourceCurrentIndex = sourceIndex; sourceCurrentIndex < endOfArray; sourceCurrentIndex++)
            {
                long markerCurrentIndex = markerIndex;
                
                for (; markerCurrentIndex < marker.Length; markerCurrentIndex++)
                {
                    if (sourceCurrentIndex + markerCurrentIndex >= endOfArray ||
                        source[sourceCurrentIndex + markerCurrentIndex] != marker[markerCurrentIndex])
                    {
                        break;
                    }
                }

                if (markerCurrentIndex == marker.Length)
                {
                    return new MatchResult { State = MatchState.Found, Index = sourceCurrentIndex };
                }
                
                if (sourceCurrentIndex + markerCurrentIndex == endOfArray)
                {
                    return new MatchResult { State = MatchState.Truncated, Index = sourceCurrentIndex };
                }
            }

            return new MatchResult { State = MatchState.NotFound, Index = -1 };
        }
    }
}