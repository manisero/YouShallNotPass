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

        // bool validator func

        private static readonly Func<string, bool> BoolValidatorFunc = x => !string.IsNullOrEmpty(x);

        [Fact]
        public void value_only_bool_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValueOnlyBoolValidatorFunc<NotEmptyValidationRule, string, EmptyValidationError>(BoolValidatorFunc,
                                                                                                                   new EmptyValidationError());

            var engine = engineBuilder.Build();
            var result = engine.Validate(Value, Rule);

            result.HasError().Should().BeTrue();
        }

        // validator func

        private static readonly Func<string, EmptyValidationError> ValidatorFunc
            = x => string.IsNullOrEmpty(x)
                ? EmptyValidationError.Some
                : EmptyValidationError.None;

        [Fact]
        public void value_only_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValueOnlyValidatorFunc<NotEmptyValidationRule, string, EmptyValidationError>(ValidatorFunc);

            var engine = engineBuilder.Build();
            var result = engine.Validate(Value, Rule);

            result.HasError().Should().BeTrue();
        }
    }
}
