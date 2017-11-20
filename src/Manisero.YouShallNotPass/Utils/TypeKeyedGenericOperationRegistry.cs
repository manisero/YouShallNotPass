using System;
using System.Collections.Generic;

namespace Manisero.YouShallNotPass.Utils
{
    internal class TypeKeyedGenericOperationRegistry<TOperation>
        where TOperation : class
    {
        private struct Registration
        {
            public Type OperationOpenGenericType { get; set; }
            public Func<Type, TOperation> OperationGetter { get; set; }
        }

        /// <summary>key generic definition -> registration</summary>
        private readonly IDictionary<Type, Registration> _registrations = new Dictionary<Type, Registration>();

        public void Register(
            Type keyGenericDefinition,
            Type operationOpenGenericType,
            Func<Type, TOperation> operationGetter)
        {
            var registration = new Registration
            {
                OperationOpenGenericType = operationOpenGenericType,
                OperationGetter = operationGetter
            };

            _registrations.Add(keyGenericDefinition, registration);
        }

        public TOperation TryResolve(Type key)
        {
            if (!key.IsGenericType)
            {
                return null;
            }

            var keyGenericDefinition = key.GetGenericTypeDefinition();
            var registration = _registrations.GetValueOrNull(keyGenericDefinition);

            if (registration == null)
            {
                return null;
            }

            // Assumptions:
            // - if operation has n generic type parameters, then key has at least n generic parameters
            // - key's first n generic type parameters are the same as operation's generic type parameters

            var operationOpenGenericType = registration.Value.OperationOpenGenericType;
            var operationTypeParameters = operationOpenGenericType.GetGenericArguments();

            if (key.GenericTypeArguments.Length < operationTypeParameters.Length)
            {
                // TODO: Consider throwing exception
                return null;
            }

            var operationTypeArguments = key.GenericTypeArguments.GetRange(0, operationTypeParameters.Length);
            var operationType = operationOpenGenericType.MakeGenericType(operationTypeArguments);

            return registration.Value.OperationGetter(operationType);
        }
    }
}
