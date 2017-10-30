using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Tests.Validations
{
    public static class ValidationsTestsHelper
    {
        public static IValidationEngine BuildEngine()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterGenericValidator(typeof(MinValidator<>));

            return builder.Build();
        }
    }
}
