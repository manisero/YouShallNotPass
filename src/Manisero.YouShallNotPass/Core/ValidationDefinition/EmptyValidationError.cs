namespace Manisero.YouShallNotPass.Core.ValidationDefinition
{
    public class EmptyValidationError
    {
        public static readonly EmptyValidationError None = null;
        public static readonly EmptyValidationError Some = new EmptyValidationError();
    }
}
