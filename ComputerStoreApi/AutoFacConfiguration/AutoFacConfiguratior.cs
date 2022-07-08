using _06_Infrastructure.DependencyResolvers;
using Autofac;
using Infrastructure.DataAccess;
using Infrastructure.Interfaces;
using ManagerLayer;
using ManagmentLayer.Commands;
using Services;
using System.Reflection;

namespace ComputerStoreApi.AutoFacConfiguration
{
    public class AutoFacConfiguratior : Autofac.Module

    {

        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var managmentsLocatorType = typeof(DiCommand);
            var servicesLocatorType = typeof(DIService);
            var managerLocatorType = typeof(DiManager);


            var concreteTypesToRegister = GetAllConcreteTypes(Assembly.GetAssembly(managmentsLocatorType), typeof(ICommand<>), typeof(ICommand<,>), typeof(ICommand<,,>), typeof(IManagment));
            concreteTypesToRegister.AddRange(GetAllConcreteTypes(Assembly.GetAssembly(managerLocatorType), typeof(IManager)));
            concreteTypesToRegister.AddRange(GetAllConcreteTypes(Assembly.GetAssembly(servicesLocatorType), typeof(IService)));




            builder.RegisterType(typeof(Repository)).AsImplementedInterfaces();

            concreteTypesToRegister.ForEach(t =>
            {
                builder.RegisterType(t).AsSelf();
            });
            builder.RegisterType<DependencyResolver>().AsImplementedInterfaces();


        }

        private List<Type> GetAllConcreteTypes(Assembly? assembly, params Type[] types)
        {
            if (assembly == null)
            {
                return new List<Type>();
            }

            List<Type> cocncreteTypes = new List<Type>();
            foreach (var type in types)
            {
                if (type.IsGenericType)
                {
                    cocncreteTypes.AddRange(GetConcreteGenerics(assembly, type));
                }
                else
                {
                    cocncreteTypes.AddRange(GetConcreteNoneGeneric(assembly, type));
                }
            }
            return cocncreteTypes.Where(x => !x.IsInterface && !x.IsAbstract).ToList();
        }

        private List<Type> GetConcreteGenerics(Assembly assembly, Type t)
        {
            var concreteGenerics = (from x in assembly.GetTypes()
                                    from z in x.GetInterfaces()
                                    let y = x.BaseType
                                    where
                                   (y != null && !y.IsInterface && y.IsGenericType &&
                                    (t.IsAssignableFrom(y.GetGenericTypeDefinition())) ||
                                    (z.IsGenericType &&
                                     t.IsAssignableFrom(z.GetGenericTypeDefinition())))
                                    select x).ToList();
            return concreteGenerics ?? new List<Type>();
        }


        private List<Type> GetConcreteNoneGeneric(Assembly assembly, Type t)
        {
            var concreteNoneGenerics = (from x in assembly.GetTypes()
                                        from z in x.GetInterfaces()
                                        let y = x.BaseType
                                        where
                                       (y != null && !y.IsGenericType &&
                                        (t.IsAssignableFrom(y)) ||
                                        (!z.IsGenericType &&
                                         t.IsAssignableFrom(z)))
                                        select x).ToList();
            return concreteNoneGenerics ?? new List<Type>();
        }
    }
}