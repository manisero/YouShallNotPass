using System;
using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Creating_validators
{
    public class Value_only
    {
        // bool validator func

        public class NotEmptyValidationRule : IValidationRule<string, EmptyValidationError>
        {
        }

        public static readonly NotEmptyValidationRule NotEmptyRule = new NotEmptyValidationRule();

        private static readonly Func<string, bool> BoolValidatorFunc = x => !string.IsNullOrEmpty(x);

        [Fact]
        public void value_only_bool_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValueOnlyBoolValidatorFunc<NotEmptyValidationRule, string, EmptyValidationError>(BoolValidatorFunc,
                                                                                                                   new EmptyValidationError());

            var engine = engineBuilder.Build();
            var result = engine.Validate(string.Empty, NotEmptyRule);

            result.HasError().Should().BeTrue();
        }

        // validator func

        public class EvenLengthValidationRule : IValidationRule<string, EvenLengthValidationError>
        {
        }

        public class EvenLengthValidationError
        {
            public int ActualLength { get; set; }
        }

        public static readonly EvenLengthValidationRule EvenLengthRule = new EvenLengthValidationRule();

        private static readonly Func<string, EvenLengthValidationError> ValidatorFunc
            = x => x.Length % 2 != 0
                ? new EvenLengthValidationError { ActualLength = x.Length }
                : null;

        [Fact]
        public void value_only_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValueOnlyValidatorFunc<EvenLengthValidationRule, string, EvenLengthValidationError>(ValidatorFunc);

            var engine = engineBuilder.Build();
            var result = engine.Validate("a", EvenLengthRule);

            result.HasError().Should().BeTrue();
        }
    }
}
