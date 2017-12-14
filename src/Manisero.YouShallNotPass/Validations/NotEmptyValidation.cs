using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Validations
{
    public static class NotEmptyValidation
    {
        public class Rule<TItem> : IValidationRule<IEnumerable<TItem>, Error>
        {
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
                    return e.MoveNext()
                        ? null
                        : Error.Instance;
                }
            }
        }

        public static Rule<TItem> NotEmpty<TItem>(
            this ValidationRuleBuilder<IEnumerable<TItem>> builder)
            => new Rule<TItem>();
    }
}

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        private static readonly Action<IValidationEngineBuilder> NotEmpty
            = x => x.RegisterGenericValidator(typeof(NotEmptyValidation.Validator<>));
    }
}
