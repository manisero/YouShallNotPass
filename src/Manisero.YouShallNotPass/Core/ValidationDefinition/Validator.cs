using System.Threading.Tasks;
using Manisero.YouShallNotPass.Core.Engine;

namespace Manisero.YouShallNotPass.Core.ValidationDefinition
{
    public interface IValidator<TValue, TRule, TError>
        where TRule : IValidationRule<TError>
        where TError : class
    {
        TError Validate(TValue value, TRule rule, ValidationContext context);
    }

    public interface IAsyncValidator<TValue, TRule, TError>
        where TRule : IValidationRule<TError>
        where TError : class
    {
        Task<TError> ValidateAsync(TValue value, TRule rule, ValidationContext context);
    }
}
