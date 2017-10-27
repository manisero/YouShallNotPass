namespace Manisero.YouShallNotPass.Core
{
    public interface IValidator<TRule, TValue, TError>
        where TError : class
    {
        TError Validate(TValue value, TRule rule, ValidationContext context);
    }

    public interface IGenericValidator<TRule, TError>
        where TError : class
    {
        TError Validate<TValue>(TValue value, TRule rule, ValidationContext context);
    }
}
