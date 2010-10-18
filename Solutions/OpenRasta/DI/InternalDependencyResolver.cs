#region License
/* Authors:
 *      Sebastien Lambla (seb@serialseb.com)
 * Copyright:
 *      (C) 2007-2009 Caffeine IT & naughtyProd Ltd (http://www.caffeine-it.com)
 * License:
 *      This file is distributed under the terms of the MIT License found at the end of this file.
 */
#endregion

namespace OpenRasta.DI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.DI.Internal;
    using OpenRasta.Diagnostics;
    using OpenRasta.Pipeline;

    public abstract class DependencyLifetimeManager
    {
        protected DependencyLifetimeManager(InternalDependencyResolver resolver)
        {
            this.Resolver = resolver;
        }

        protected InternalDependencyResolver Resolver { get; private set; }

        public virtual bool IsRegistrationAvailable(DependencyRegistration registration)
        {
            return true;
        }

        public virtual object Resolve(ResolveContext context, DependencyRegistration registration)
        {
            return context.Builder.CreateObject(registration);
        }

        public virtual void VerifyRegistration(DependencyRegistration registration)
        {
        }
    }

    public class InternalDependencyResolver : DependencyResolverCore, IDependencyResolver
    {
        readonly Dictionary<DependencyLifetime, DependencyLifetimeManager> lifetimeManagers;
        ILogger log = new TraceSourceLogger();

        public InternalDependencyResolver()
        {
            this.Registrations = new DependencyRegistrationCollection();
            this.lifetimeManagers = new Dictionary<DependencyLifetime, DependencyLifetimeManager>
            {
                { DependencyLifetime.Transient, new TransientLifetimeManager(this) }, 
                { DependencyLifetime.Singleton, new SingletonLifetimeManager(this) }, 
                { DependencyLifetime.PerRequest, new PerRequestLifetimeManager(this) }
            };
        }

        public ILogger Log
        {
            get { return this.log; }
            set { this.log = value; }
        }

        public DependencyRegistrationCollection Registrations { get; private set; }

        protected override void AddDependencyCore(Type serviceType, Type concreteType, DependencyLifetime lifetime)
        {
            this.Registrations.Add(new DependencyRegistration(serviceType, concreteType, this.lifetimeManagers[lifetime]));
        }

        protected override void AddDependencyCore(Type concreteType, DependencyLifetime lifetime)
        {
            this.AddDependencyCore(concreteType, concreteType, lifetime);
        }

        protected override void AddDependencyInstanceCore(Type serviceType, object instance, DependencyLifetime lifetime)
        {
            var instanceType = instance.GetType();

            var registration = new DependencyRegistration(serviceType, instanceType, this.lifetimeManagers[lifetime], instance);

            this.Registrations.Add(registration);
        }

        protected override IEnumerable<TService> ResolveAllCore<TService>()
        {
            return from dependency in this.Registrations[typeof(TService)]
                   where dependency.LifetimeManager.IsRegistrationAvailable(dependency)
                   select (TService)this.Resolve(dependency);
        }

        protected override object ResolveCore(Type serviceType)
        {
            if (!this.Registrations.HasRegistrationForService(serviceType))
            {
                throw new DependencyResolutionException("No type registered for {0}".With(serviceType.Name));
            }

            return this.Resolve(this.Registrations.GetRegistrationForService(serviceType));
        }

        public void HandleIncomingRequestProcessed()
        {
            var store = (IContextStore)this.Resolve(typeof(IContextStore));
            store.Destruct();
        }

        public bool HasDependency(Type serviceType)
        {
            if (serviceType == null)
            {
                return false;
            }

            return this.Registrations.HasRegistrationForService(serviceType);
        }

        public bool HasDependencyImplementation(Type serviceType, Type concreteType)
        {
            return this.Registrations.HasRegistrationForService(serviceType) && this.Registrations[serviceType].Count(r => r.ConcreteType == concreteType) >= 1;
        }

        object Resolve(DependencyRegistration dependency)
        {
            var context = new ResolveContext(this.Registrations, this.Log);
            
            return context.Resolve(dependency);
        }
    }
}

#region Full license
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion