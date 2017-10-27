namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationRule
    {
        int Type { get; }
    }

    public interface IValidationRule<TConfig> : IValidationRule
    {
        TConfig Config { get; }
    }
}
