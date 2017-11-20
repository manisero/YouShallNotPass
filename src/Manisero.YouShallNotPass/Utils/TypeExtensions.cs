using System;

namespace Manisero.YouShallNotPass.Utils
{
    internal static class TypeExtensions
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
                    // TODO: type can implement the same interface multiple times
                    return @interface;
                }
            }

            return null;
        }

        public static bool ImplementsGenericDefinition(this Type type, Type definition)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == definition;
        }

        public static bool ImplementsGenericInterfaceDefinition(this Type type, Type interfaceDefinition)
        {
            return type.GetGenericInterfaceDefinitionImplementation(interfaceDefinition) != null;
        }

        public static Type GetGenericInterfaceTypeArgument(this Type type, Type genericInterfaceDefinition, int typeArgumentPosition)
        {
            var interfaceImplementation = type.GetGenericInterfaceDefinitionImplementation(genericInterfaceDefinition);

            return interfaceImplementation?.GenericTypeArguments[typeArgumentPosition];
        }
    }
}
