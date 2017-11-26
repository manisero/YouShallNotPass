namespace Manisero.YouShallNotPass.ErrorMessages
{
    public interface IValidationErrorMessage<TCode>
    {
        TCode Code { get; }
    }
}
