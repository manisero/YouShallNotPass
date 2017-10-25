namespace Manisero.YouShallNotPass
{
    public interface IValidation<TConfig>
    {
        int Type { get; }

        TConfig Config { get; }
    }
}
