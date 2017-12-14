using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Validations
{
    public static class StartsWithValidation
    {
        public class Rule<TItem> : IValidationRule<IEnumerable<TItem>, Error>
        {
            public TItem Value { get; set; }
        }

        public class Error
        {
            public static readonly Error Instance = new Error();
        }

        public class Validator<TItem> : IValidator<Rule<TItem>, IEnumerable<TItem>, Error>
        {
            public Error Validate(IEnumerable<TItem> value, Rule<TItem> rule, ValidationContext context)
            {
                using (var e = value.GetEnumerator())
                {
                    if (e.MoveNext() && !Equals(e.Current, rule.Value))
                    {
                        return Error.Instance;
                    }
                }

                return null;
            }
        }

        public static Rule<TItem> StartsWith<TItem>(
            this ValidationRuleBuilder<IEnumerable<TItem>> builder,
            TItem value)
            => new Rule<TItem>
            {
                Value = value
            };
    }
}

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        private static readonly Action<IValidationEngineBuilder> StartsWith
            = x => x.RegisterGenericValidator(typeof(StartsWithValidation.Validator<>));
    }
}
