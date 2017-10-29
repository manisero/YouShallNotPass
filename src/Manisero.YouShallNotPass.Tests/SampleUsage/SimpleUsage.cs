using FluentAssertions;
using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Tests.SampleUsage
{
    public class SimpleUsage
    {
        [Fact]
        public void valid_email___no_error()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterValidator(new EmailValidator());

            var engine = builder.Build();

            var result = engine.Validate("a@a.com", new EmailValidationRule());

            result.HasError().Should().BeFalse();
        }

        [Fact]
        public void invalid_email___error()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterValidator(new EmailValidator());

            var engine = builder.Build();

            var result = engine.Validate("a", new EmailValidationRule());

            result.HasError().Should().BeTrue();
        }
    }
}
