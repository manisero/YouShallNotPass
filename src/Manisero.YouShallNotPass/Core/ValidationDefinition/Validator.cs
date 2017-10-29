using System.Threading.Tasks;
using Manisero.YouShallNotPass.Core.Engine;

namespace Manisero.YouShallNotPass.Core.ValidationDefinition
{
    public interface IValidator
    {
    }

    public interface IValidator<TRule, TValue, TError> : IValidator
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        TError Validate(TValue value, TRule rule, ValidationContext context);
    }

    public interface IAsyncValidator<TRule, TValue, TError> : IValidator
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        Task<TError> ValidateAsync(TValue value, TRule rule, ValidationContext context);
    }

    public static class ValidatorInterfaceConstants
    {
        public const int TRuleTypeParameterPosition = 0;
    }
}
