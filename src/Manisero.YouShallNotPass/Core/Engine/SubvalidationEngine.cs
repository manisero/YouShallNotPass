﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration;
using Manisero.YouShallNotPass.Extensions;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public class SubvalidationEngine : ISubvalidationEngine
    {
        private static readonly Lazy<MethodInfo> ValidateInternalGenericMethod = new Lazy<MethodInfo>(
            () => typeof(SubvalidationEngine).GetMethod(nameof(ValidateInternalGeneric),
                                                        BindingFlags.Instance | BindingFlags.NonPublic));

        private readonly IValidationRuleMetadataProvider _validationRuleMetadataProvider;
        private readonly IValidatorResolver _validatorResolver;
        private readonly ValidationContext _context;

        public SubvalidationEngine(
            IValidationRuleMetadataProvider validationRuleMetadataProvider,
            IValidatorResolver validatorResolver,
            IDictionary<string, object> data)
        {
            _validationRuleMetadataProvider = validationRuleMetadataProvider;
            _validatorResolver = validatorResolver;

            _context = new ValidationContext
            {
                Engine = this,
                Data = data
            };
        }

        public IValidationResult Validate(object value, IValidationRule rule)
        {
            // TODO: Make this as fast as possible (build lambda and cache it under ruleType key)

            var ruleType = rule.GetType();
            var iRuleImplementation = ruleType.GetGenericInterfaceDefinitionImplementation(typeof(IValidationRule<,>));
            var ruleGenericArguments = iRuleImplementation.GetGenericArguments();

            var valueType = ruleGenericArguments[ValidationRuleInterfaceConstants.TValueTypeParameterPosition];
            var errorType = ruleGenericArguments[ValidationRuleInterfaceConstants.TErrorTypeParameterPosition];

            return ValidateInternal(ruleType, valueType, errorType, value, rule);
        }

        public Task<IValidationResult> ValidateAsync(object value, IValidationRule rule)
        {
            throw new NotImplementedException();
        }

        public IValidationResult Validate<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>
        {
            // TODO: Make this as fast as possible (build lambda and cache it under ruleType key)

            var ruleType = rule.GetType();
            var iRuleImplementation = ruleType.GetGenericInterfaceDefinitionImplementation(typeof(IValidationRule<,>));
            var ruleGenericArguments = iRuleImplementation.GetGenericArguments();

            var valueType = ruleGenericArguments[ValidationRuleInterfaceConstants.TValueTypeParameterPosition];
            var errorType = ruleGenericArguments[ValidationRuleInterfaceConstants.TErrorTypeParameterPosition];

            return ValidateInternal(ruleType, valueType, errorType, value, rule);
        }

        public Task<IValidationResult> ValidateAsync<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>
        {
            throw new NotImplementedException();
        }

        public IValidationResult<TError> Validate<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            return ValidateInternalGeneric<TRule, TValue, TError>(value, rule);
        }

        public Task<IValidationResult<TError>> ValidateAsync<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            throw new NotImplementedException();
        }

        private IValidationResult ValidateInternal(
            Type ruleType, Type valueType, Type errorType,
            object value, IValidationRule rule)
        {
            try
            {
                var result = ValidateInternalGenericMethod.Value
                                                          .MakeGenericMethod(ruleType, valueType, errorType)
                                                          .Invoke(this,
                                                                  new object[] { value, rule });

                return (IValidationResult)result;
            }
            catch (TargetInvocationException exception)
            {
                ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                throw;
            }
        }

        private IValidationResult<TError> ValidateInternalGeneric<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var validatesNull = _validationRuleMetadataProvider.ValidatesNull(typeof(TRule));

            if (!validatesNull && value == null)
            {
                return new ValidationResult<TError>
                {
                    Rule = rule
                };
            }

            var validator = _validatorResolver.TryResolve<TRule, TValue, TError>();

            if (validator == null)
            {
                throw new InvalidOperationException($"Unable to find validator validating value '{typeof(TValue)}' against rule '{typeof(TRule)}'.");
            }

            var error = validator.Validate(value, rule, _context);

            return new ValidationResult<TError>
            {
                Rule = rule,
                Error = error
            };
        }
    }
}
