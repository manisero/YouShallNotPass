using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Creating_validators
{
    public class Value_only
    {
        // value only bool validator

        public static class NotEmptyValidation
        {
            public class Rule : IValidationRule<string, EmptyValidationError>
            {
            }

            public class Validator : ValueOnlyBoolValidator<Rule, string, EmptyValidationError>
            {
                public Validator() : base(EmptyValidationError.Some)
                {
                }

                protected override bool Validate(string value)
                    => !string.IsNullOrEmpty(value);
            }
        }

        public static readonly NotEmptyValidation.Rule NotEmptyRule = new NotEmptyValidation.Rule();
        
        [Fact]
        public void value_only_bool_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValidator(new NotEmptyValidation.Validator());

            var engine = engineBuilder.Build();
            var result = engine.Validate(string.Empty, NotEmptyRule);

            result.HasError().Should().BeTrue();
        }

        // value only validator

        public static class EvenLengthValidation
        {
            public class Rule : IValidationRule<string, Error>
            {
            }

            public class Error
            {
                public int ActualLength { get; set; }
            }

            public class Validator : ValueOnlyValidator<Rule, string, Error>
            {
                protected override Error Validate(string value)
                    => value.Length % 2 != 0
                        ? new Error { ActualLength = value.Length }
                        : null;
            }
        }

        public static readonly EvenLengthValidation.Rule EvenLengthRule = new EvenLengthValidation.Rule();
        
        [Fact]
        public void value_only_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValidator(new EvenLengthValidation.Validator());

            var engine = engineBuilder.Build();
            var result = engine.Validate("a", EvenLengthRule);

            result.HasError().Should().BeTrue();
        }
    }
}
