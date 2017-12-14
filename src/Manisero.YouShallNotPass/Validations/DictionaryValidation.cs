using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Utils;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Validations
{
    public static class DictionaryValidation
    {
        public class Rule<TKey, TValue> : IValidationRule<IDictionary<TKey, TValue>, Error<TKey>>
        {
            public IValidationRule<TValue> ValueRule { get; set; }
        }

        public class Error<TKey>
        {
            public static Error<TKey> Create()
                => new Error<TKey> { Violations = new Dictionary<TKey, IValidationResult>() };

            /// <summary>entry key (only invalid entries) -> validation result</summary>
            public IDictionary<TKey, IValidationResult> Violations { get; set; }
        }

        public class Validator<TKey, TValue> : IValidator<Rule<TKey, TValue>, IDictionary<TKey, TValue>, Error<TKey>>,
                                               IAsyncValidator<Rule<TKey, TValue>, IDictionary<TKey, TValue>, Error<TKey>>
        {
            public Error<TKey> Validate(
                IDictionary<TKey, TValue> value,
                Rule<TKey, TValue> rule,
                ValidationContext context)
            {
                var error = LightLazy.Create(Error<TKey>.Create);

                foreach (var entry in value)
                {
                    var entryResult = context.Engine.Validate(entry.Value, rule.ValueRule);

                    if (entryResult.HasError())
                    {
                        error.Item.Violations.Add(entry.Key, entryResult);
                    }
                }

                return error.ItemOrNull;
            }

            public Task<Error<TKey>> ValidateAsync(
                IDictionary<TKey, TValue> value,
                Rule<TKey, TValue> rule,
                ValidationContext context)
            {
                throw new NotImplementedException();
            }
        }

        public static Rule<TKey, TValue> Dictionary<TKey, TValue>(
            this ValidationRuleBuilder<IDictionary<TKey, TValue>> builder,
            Func<ValidationRuleBuilder<TValue>, IValidationRule<TValue>> valueRule)
            => new Rule<TKey, TValue>
            {
                ValueRule = valueRule(ValidationRuleBuilder<TValue>.Instance)
            };
    }
}

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        private static readonly Action<IValidationEngineBuilder> Dictionary
            = x => x.RegisterGenericValidator(typeof(DictionaryValidation.Validator<,>));
    }
}
