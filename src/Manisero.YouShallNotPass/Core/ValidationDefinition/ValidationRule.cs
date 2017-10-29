namespace Manisero.YouShallNotPass.Core.ValidationDefinition
{
    public interface IValidationRule
    {
    }

    public interface IValidationRule<TError> : IValidationRule
        where TError : class
    {
    }
}
