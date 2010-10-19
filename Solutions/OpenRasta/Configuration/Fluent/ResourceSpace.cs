namespace OpenRasta.Configuration.Fluent
{
    using System.ComponentModel;

    using OpenRasta.Contracts.Configuration.Fluent;

    public static class ResourceSpace
    {
        public static IHas Has
        {
            get { return new FluentTarget(); }
        }

        public static IUses Uses
        {
            get { return new FluentTarget(); }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object objA, object objB)
        {
            return object.Equals(objA, objB);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool ReferenceEquals(object objA, object objB)
        {
            return object.ReferenceEquals(objA, objB);
        }
    }
}