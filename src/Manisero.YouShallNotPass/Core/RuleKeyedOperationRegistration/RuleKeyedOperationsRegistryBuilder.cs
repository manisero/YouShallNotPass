using System;

namespace Manisero.YouShallNotPass.Core.RuleKeyedOperationRegistration
{
    public interface IRuleKeyedOperationsRegistryBuilder<TOperation>
    {
        void RegisterOperation<TRule, TValue, TError>(
            TOperation operation)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        /// <param name="operationGetter">concrete operation type => operation</param>
        void RegisterGenericOperation(
            Type ruleGenericDefinition,
            Type operationOpenGenericType,
            Func<Type, TOperation> operationGetter);

        OperationsRegistry<TOperation> Build();
    }

    public class RuleKeyedOperationsRegistryBuilder<TOperation> : IRuleKeyedOperationsRegistryBuilder<TOperation>
    {
        private readonly OperationsRegistry<TOperation> _registry = new OperationsRegistry<TOperation>();

        public void RegisterOperation<TRule, TValue, TError>(
            TOperation operation)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _registry.Operations.Add(typeof(TRule), operation);
        }

        public void RegisterGenericOperation(
            Type ruleGenericDefinition,
            Type operationOpenGenericType,
            Func<Type, TOperation> operationGetter)
        {
            var registration = new OperationsRegistry<TOperation>.GenericOperationRegistration
            {
                OperationOpenGenericType = operationOpenGenericType,
                Getter = operationGetter
            };

            _registry.GenericOperations.Add(ruleGenericDefinition, registration);
        }

        public OperationsRegistry<TOperation> Build()
        {
            return _registry;
        }
    }
}
