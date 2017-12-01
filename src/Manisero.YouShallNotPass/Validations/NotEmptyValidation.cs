using System.Collections.Generic;

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
