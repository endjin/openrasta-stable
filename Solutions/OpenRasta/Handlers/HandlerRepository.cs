namespace OpenRasta.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Collections;
    using OpenRasta.Contracts.Handlers;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.TypeSystem;

    public class HandlerRepository : IHandlerRepository
    {
        private readonly IDictionary<object, HashSet<IType>> resourceHandlers = new NullBehaviorDictionary<object, HashSet<IType>>();

        public IEnumerable<IType> GetHandlerTypes()
        {
            return this.resourceHandlers.SelectMany(x => x.Value).Distinct();
        }

        public void AddResourceHandler(object resourceKey, IType handlerType)
        {
            if (resourceKey == null)
            {
                throw new ArgumentNullException("resourceKey");
            }

            if (resourceKey is Type)
            {
                throw new ArgumentException("Cannot register a type as the key. Use an IType instead.", "resourceKey");
            }

            var list = this.GetOrCreate(resourceKey);

            if (list.Contains(handlerType))
            {
                throw new ArgumentException("The provided handler is already registered.", "handlerType");
            }

            list.Add(handlerType);
        }

        public void Clear()
        {
            this.resourceHandlers.Clear();
        }

        public IEnumerable<IType> GetHandlerTypesFor(object resourceKey)
        {
            if (resourceKey is Type)
            {
                throw new ArgumentException("Type keys are not allowed. Use an IType instead.");
            }

            return this.GetOrCreate(resourceKey);
        }


        HashSet<IType> GetOrCreate(object resourceKey)
        {
            HashSet<IType> handlerTypes;
            if (!this.resourceHandlers.TryGetValue(resourceKey, out handlerTypes))
            {
                this.resourceHandlers.Add(resourceKey, handlerTypes = new HashSet<IType>());
            }
            return handlerTypes;
        }
    }
}