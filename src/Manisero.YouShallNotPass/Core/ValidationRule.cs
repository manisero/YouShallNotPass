namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationRule
    {
    }

    public interface IValidationRule<TError> : IValidationRule
        where TError : class
    {
    }
}
