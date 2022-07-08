using Autofac;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_Infrastructure.DependencyResolvers
{
    public class DependencyResolver : IResolver
    {
        private readonly ILifetimeScope scope;

        public DependencyResolver(ILifetimeScope scope)
        {
            this.scope = scope;
        }
        public T Resolve<T>()
        {
            return scope.Resolve<T>();
        }
    }
}
