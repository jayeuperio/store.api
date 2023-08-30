using System.Reflection;

namespace Store.Api.Extensions
{
    public static class DependencyResolver
    {
        public enum DependancyLifetime
        {
            Transient,
            Singleton,
            Scoped
        }
        public static IServiceCollection RegisterAssemblies(this IServiceCollection serviceCollection, DependancyLifetime lifetime, string[] assemblies, params Type[] markerInterfaces)
        {
            for (int i = 0; i < assemblies.Length; i++)
            {
                foreach (Type item in Assembly.Load(new AssemblyName(assemblies[i])).GetTypes().Where(delegate (Type x)
                {
                    TypeInfo typeInfo = x.GetTypeInfo();
                    return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericType && typeInfo.ImplementedInterfaces.Intersect(markerInterfaces).Any();
                })
                    .ToList())
                {
                    List<Type> list = item.GetTypeInfo().ImplementedInterfaces.ToList();
                    HashSet<Type> hashSet = new HashSet<Type>(list);
                    foreach (Type item2 in list)
                    {
                        hashSet.ExceptWith(item2.GetTypeInfo().ImplementedInterfaces);
                    }

                    Type type = hashSet.FirstOrDefault();
                    if (!(type == null) && !markerInterfaces.Contains(type))
                    {
                        Register(serviceCollection, lifetime, type, item);
                    }
                }
            }

            return serviceCollection;
        }

        private static void Register(IServiceCollection serviceCollection, DependancyLifetime lifetime, Type intType, Type concType)
        {
            switch (lifetime)
            {
                case DependancyLifetime.Transient:
                    serviceCollection.AddTransient(intType, concType);
                    break;
                case DependancyLifetime.Scoped:
                    serviceCollection.AddScoped(intType, concType);
                    break;
                case DependancyLifetime.Singleton:
                    serviceCollection.AddSingleton(intType, concType);
                    break;
            }
        }
    }
}
