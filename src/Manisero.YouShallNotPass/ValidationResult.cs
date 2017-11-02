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
    
    public class ValidationResult<TError> : IValidationResult<TError>
        where TError : class
    {
        public IValidationRule Rule { get; set; }
        public TError Error { get; set; }
        object IValidationResult.Error => Error;
    }

    public static class ValidationResultExtensions
    {
        public static bool HasError(this IValidationResult validationResult) => validationResult.Error != null;
    }
}
