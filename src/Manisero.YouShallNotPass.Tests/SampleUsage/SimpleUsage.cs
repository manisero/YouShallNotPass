using FluentAssertions;
using Manisero.YouShallNotPass.ConcreteValidations;
using Manisero.YouShallNotPass.Core;
using Xunit;

namespace Manisero.YouShallNotPass.Tests.SampleUsage
{
    public class SimpleUsage
    {
        [Fact]
        public void run()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterValidator(new EmailValidator());

            var engine = builder.Build();

            var result = engine.Validate("a@a.com", new EmailValidationRule());

            result.HasError().Should().BeFalse();
        }
    }
}
