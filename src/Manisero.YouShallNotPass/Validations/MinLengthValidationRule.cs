namespace Manisero.YouShallNotPass.Validations
{
    public class MinLengthValidationRule : IValidationRule<string, MinLengthValidationError>
    {
        public int MinLength { get; set; }
    }

    public class MinLengthValidationError
    {
        public static readonly MinLengthValidationError Instance = new MinLengthValidationError();
    }

    public class MinLengthValidator : IValidator<MinLengthValidationRule, string, MinLengthValidationError>
    {
        public MinLengthValidationError Validate(string value, MinLengthValidationRule rule, ValidationContext context)
        {
            return value.Length < rule.MinLength
                ? MinLengthValidationError.Instance
                : null;
        }
    }
}
