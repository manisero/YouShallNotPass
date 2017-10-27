namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationResult
    {
        object Rule { get; }
        object Value { get; }
        object Error { get; }
    }

    public class ValidationResult : IValidationResult
    {
        public object Rule { get; set; }
        public object Value { get; set; }
        public object Error { get; set; }
    }

    public static class ValidationResultExtensions
    {
        public static bool HasError(this IValidationResult validationResult) => validationResult.Error != null;
    }
}
