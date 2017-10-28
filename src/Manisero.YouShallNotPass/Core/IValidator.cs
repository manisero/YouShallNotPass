using System.Threading.Tasks;

namespace Manisero.YouShallNotPass.Core
{
    public interface IValidator<TValue, TRule, TError>
        where TError : class
    {
        TError Validate(TValue value, TRule rule, ValidationContext context);
    }

    public interface IAsyncValidator<TValue, TRule, TError>
        where TError : class
    {
        Task<TError> ValidateAsync(TValue value, TRule rule, ValidationContext context);
    }
}
