namespace Manisero.YouShallNotPass
{
    public interface IValidationResult
    {
        IValidationRule Rule { get; }
        object Error { get; }
    }

    public interface IValidationResult<TError> : IValidationResult
        where TError : class
    {
        new TError Error { get; }
    }
    
    public class ValidationResult<TRule, TValue, TError> : IValidationResult<TError>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        public TRule Rule { get; set; }
        IValidationRule IValidationResult.Rule => Rule;

        public TError Error { get; set; }
        object IValidationResult.Error => Error;
    }

    public static class ValidationResultExtensions
    {
        public static bool HasError(this IValidationResult validationResult) => validationResult.Error != null;
    }
}
