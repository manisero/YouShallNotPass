using System;
using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Creating_validators
{
    public class Value_only
    {
        public class NotEmptyValidationRule : IValidationRule<string, EmptyValidationError>
        {
        }
        
        public static readonly NotEmptyValidationRule Rule = new NotEmptyValidationRule();

        private const string Value = "";

        // validator func

        private static readonly Func<string, bool> ValidatorFunc = x => !string.IsNullOrEmpty(x);

        [Fact]
        public void value_only_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterErrorOnlyValidatorFunc<NotEmptyValidationRule, string, EmptyValidationError>(ValidatorFunc, new EmptyValidationError());

            var engine = engineBuilder.Build();
            var result = engine.Validate(Value, Rule);

            result.HasError().Should().BeTrue();
        }
    }
}
