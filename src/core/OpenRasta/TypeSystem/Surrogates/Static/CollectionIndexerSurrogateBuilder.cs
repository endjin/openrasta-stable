namespace OpenRasta.TypeSystem.Surrogates.Static
{
    using System;
    using System.Collections.Generic;

    using OpenRasta.TypeSystem.ReflectionBased;

    public class CollectionIndexerSurrogateBuilder : AbstractStaticSurrogateBuilder
    {
        public override bool CanCreateFor(Type type)
        {
            return type.FindInterface(typeof(ICollection<>)) != null
                && type.FindInterface(typeof(IList<>)) == null;
        }

        public override Type Create(Type type)
        {
            return typeof(CollectionIndexerSurrogate<>).MakeGenericType(
                type.FindInterface(typeof(ICollection<>)).GetGenericArguments()[0]);
        }
    }
}