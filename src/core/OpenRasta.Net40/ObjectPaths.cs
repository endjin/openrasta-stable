namespace OpenRasta
{
    using System;
    using System.Collections.Generic;
    using OpenRasta.Reflection;

    public static class ObjectPaths
    {
        [ThreadStatic]
        private static Dictionary<object, PropertyPath> objectPaths;

        private static Dictionary<object, PropertyPath> ObjectPathsCore
        {
            get
            {
                return objectPaths ?? (objectPaths = new Dictionary<object, PropertyPath>());
            }
        }

        public static void Add(object o, PropertyPath path)
        {
            ObjectPathsCore[o] = path;
        }

        public static PropertyPath Get(object o)
        {
            PropertyPath result;
            return ObjectPathsCore.TryGetValue(o, out result) ? result : null;
        }

        public static void Remove(object o)
        {
            ObjectPathsCore.Remove(o);
        }
    }
}