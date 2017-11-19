using System;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Core.RuleKeyedOperationRegistration
{
    public interface IRuleKeyedOperationResolver<TOperation>
        where TOperation : class
    {
        TOperation TryResolve<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }

    public class RuleKeyedOperationResolver<TOperation> : IRuleKeyedOperationResolver<TOperation>
        where TOperation : class
    {
        private readonly OperationsRegistry<TOperation> _operationsRegistry;

        public RuleKeyedOperationResolver(OperationsRegistry<TOperation> operationsRegistry)
        {
            _operationsRegistry = operationsRegistry;
        }

        public TOperation TryResolve<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var ruleType = typeof(TRule);

            var operation = _operationsRegistry.Operations.GetValueOrDefault(ruleType);

            if (operation != null)
            {
                return operation;
            }
            
            var genericOperation = TryGetGenericOperation(ruleType);

            if (genericOperation != null)
            {
                // TODO: Make it thread-safe
                _operationsRegistry.Operations.Add(ruleType, genericOperation);
                return genericOperation;
            }

            return null;
        }
        
        private TOperation TryGetGenericOperation(Type ruleType)
        {
            // TODO: Make this as fast as possible (e.g. cache factory / operationType) (but don't cache operation returned by factory)

            if (!ruleType.IsGenericType)
            {
                return null;
            }
            
            var ruleGenericDefinition = ruleType.GetGenericTypeDefinition();

            var registration = _operationsRegistry.GenericOperations.GetValueOrNull(ruleGenericDefinition);

            if (registration == null)
            {
                return null;
            }

            // Assumptions:
            // - if operation has n generic type parameters, then rule has at least n generic parameters
            // - rule's first n generic type parameters are the same as operation's generic type parameters

            var operationOpenGenericType = registration.Value.OperationOpenGenericType;
            var operationTypeParameters = operationOpenGenericType.GetGenericArguments();

            if (ruleType.GenericTypeArguments.Length < operationTypeParameters.Length)
            {
                // TODO: Consider throwing exception
                return null;
            }

            var operationTypeArguments = ruleType.GenericTypeArguments.GetRange(0, operationTypeParameters.Length);
            var operationType = operationOpenGenericType.MakeGenericType(operationTypeArguments);

            return registration.Value.Getter(operationType);
        }
    }
}
