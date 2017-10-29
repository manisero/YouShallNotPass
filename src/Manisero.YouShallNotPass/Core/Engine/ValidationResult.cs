using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Core.Engine
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
