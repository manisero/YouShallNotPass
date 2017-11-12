using System;

namespace Manisero.YouShallNotPass.Core.RuleKeyedOperationRegistration
{
    public interface IRuleKeyedOperationsRegistryBuilder<TOperation>
    {
        void RegisterOperation<TRule, TValue, TError>(
            TOperation operation)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        void RegisterOperationFactory<TRule, TValue, TError>(
            Func<TOperation> operationFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        /// <param name="operationFactory">concrete operation type => operation</param>
        void RegisterGenericOperationFactory(
            Type ruleGenericDefinition,
            Type operationOpenGenericType,
            Func<Type, TOperation> operationFactory);

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
            _registry.OperationInstances.Add(typeof(TRule), operation);
        }

        public void RegisterOperationFactory<TRule, TValue, TError>(
            Func<TOperation> operationFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _registry.OperationFactories.Add(typeof(TRule), operationFactory);
        }

        public void RegisterGenericOperationFactory(
            Type ruleGenericDefinition,
            Type operationOpenGenericType,
            Func<Type, TOperation> operationFactory)
        {
            var registration = new OperationsRegistry<TOperation>.GenericOperationRegistration
            {
                OperationOpenGenericType = operationOpenGenericType,
                Factory = operationFactory
            };

            _registry.GenericOperationFactories.Add(ruleGenericDefinition, registration);
        }

        public OperationsRegistry<TOperation> Build()
        {
            return _registry;
        }
    }
}
