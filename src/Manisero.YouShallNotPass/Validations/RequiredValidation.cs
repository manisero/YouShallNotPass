using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Validations
{
    [ValidatesNull]
    public class RequiredValidationRule<TValue> : IValidationRule<TValue, EmptyValidationError>
    {
    }

    public class RequiredValidator<TValue> : IValidator<RequiredValidationRule<TValue>, TValue, EmptyValidationError>
    {
        public EmptyValidationError Validate(TValue value, RequiredValidationRule<TValue> rule, ValidationContext context)
        {
            return value == null
                ? EmptyValidationError.Some
                : EmptyValidationError.None;
        }
    }
}
