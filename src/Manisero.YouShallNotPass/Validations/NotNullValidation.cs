using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Validations
{
    [ValidatesNull]
    public class NotNullValidationRule<TValue> : IValidationRule<TValue, EmptyValidationError>
    {
    }

    public class NotNullValidator<TValue> : IValidator<NotNullValidationRule<TValue>, TValue, EmptyValidationError>
    {
        public EmptyValidationError Validate(TValue value, NotNullValidationRule<TValue> rule, ValidationContext context)
        {
            return value == null
                ? EmptyValidationError.Some
                : EmptyValidationError.None;
        }
    }
}
