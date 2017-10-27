namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationError
    {
        object Rule { get; }
        object Value { get; }
        object Error { get; }
    }

    public class ValidationError : IValidationError
    {
        public object Rule { get; set; }
        public object Value { get; set; }
        public object Error { get; set; }
    }
}
