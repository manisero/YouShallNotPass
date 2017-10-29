using System;

namespace Manisero.YouShallNotPass.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetGenericInterfaceDefinitionImplementation(this Type type, Type interfaceDefinition)
        {
            if (type.IsInterface && type.ImplementsGenericDefinition(interfaceDefinition))
            {
                return type;
            }

            var interfaces = type.GetInterfaces();

            foreach (var @interface in interfaces)
            {
                if (@interface.ImplementsGenericDefinition(interfaceDefinition))
                {
                    return @interface;
                }
            }

            return null;
        }

        public static bool ImplementsGenericDefinition(this Type type, Type definition)
        {
            return type.IsConstructedGenericType && type.GetGenericTypeDefinition() == definition;
        }
    }
}
