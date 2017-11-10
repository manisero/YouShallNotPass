using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Validations
{
    [ValidatesNull]
    public class NotNullValidationRule<TValue> : IValidationRule<TValue, NotNullValidationError>
    {
    }

    public class NotNullValidationError
    {
        public static readonly NotNullValidationError Instance = new NotNullValidationError();
    }

    public class NotNullValidator<TValue> : IValidator<NotNullValidationRule<TValue>, TValue, NotNullValidationError>
    {
        public NotNullValidationError Validate(TValue value, NotNullValidationRule<TValue> rule, ValidationContext context)
        {
            return value == null
                ? NotNullValidationError.Instance
                : null;
        }
    }
}
