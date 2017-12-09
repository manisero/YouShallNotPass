namespace Manisero.YouShallNotPass.ErrorMessages
{
    public interface IValidationErrorMessage
    {
        string Code { get; }
    }

    public class ValidationErrorMessage : IValidationErrorMessage
    {
        public string Code { get; set; }
    }
}
