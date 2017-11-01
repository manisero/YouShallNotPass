namespace Manisero.YouShallNotPass.Tests.Validations
{
    public static class ValidationsTestsHelper
    {
        public static IValidationEngine BuildEngine()
        {
            var builder = new ValidationEngineBuilder();
            return builder.Build();
        }
    }
}
