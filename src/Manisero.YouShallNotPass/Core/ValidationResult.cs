namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationResult
    {
        IValidationRule Rule { get; }
        object Error { get; }
    }

    public class ValidationResult : IValidationResult
    {
        public IValidationRule Rule { get; set; }
        public object Error { get; set; }
    }

    public static class ValidationResultExtensions
    {
        public static bool HasError(this IValidationResult validationResult) => validationResult.Error != null;
    }
}
