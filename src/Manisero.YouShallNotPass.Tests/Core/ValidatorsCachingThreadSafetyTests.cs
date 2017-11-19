using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Manisero.YouShallNotPass.Tests.Core
{
    public class ValidatorsCachingThreadSafetyTests
    {
        public class Rule<TValue> : IValidationRule<TValue, Error>
        {
        }

        public class Error
        {
            public Guid ValidatorId { get; set; }
        }

        public class Validator<TValue> : IValidator<Rule<TValue>, TValue, Error>
        {
            private readonly Guid _id = Guid.NewGuid();

            public Error Validate(TValue value, Rule<TValue> rule, ValidationContext context)
                => new Error { ValidatorId = _id };
        }

        public static readonly Rule<int> ValidationRule = new Rule<int>();
        public const int Value = 1;

        [Fact]
        public void test()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterFullGenericValidatorFactory(typeof(Validator<>),
                                                              type => (IValidator)Activator.CreateInstance(type));

            var engine = engineBuilder.Build();

            var validation1 = Task.Run(() => engine.Validate<Rule<int>, int, Error>(Value, ValidationRule));
            var validation2 = Task.Run(() => engine.Validate<Rule<int>, int, Error>(Value, ValidationRule));
            Task.WaitAll(validation1, validation2);

            var result1 = validation1.Result;
            var result2 = validation2.Result;

            result2.Error.ValidatorId.Should().Be(result1.Error.ValidatorId);
        }
    }
}
