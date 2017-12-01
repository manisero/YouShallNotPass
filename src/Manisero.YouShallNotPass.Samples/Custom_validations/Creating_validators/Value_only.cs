using System;
using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Creating_validators
{
    public class Value_only
    {
        // bool validator func

        public static class NotEmptyValidation
        {
            public class Rule : IValidationRule<string, EmptyValidationError>
            {
            }

            public static readonly Func<string, bool> Validator = x => !string.IsNullOrEmpty(x);
        }

        public static readonly NotEmptyValidation.Rule NotEmptyRule = new NotEmptyValidation.Rule();
        
        [Fact]
        public void value_only_bool_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValueOnlyBoolValidatorFunc<NotEmptyValidation.Rule, string, EmptyValidationError>(NotEmptyValidation.Validator,
                                                                                                                   new EmptyValidationError());

            var engine = engineBuilder.Build();
            var result = engine.Validate(string.Empty, NotEmptyRule);

            result.HasError().Should().BeTrue();
        }

        // validator func

        public static class EvenLengthValidation
        {
            public class Rule : IValidationRule<string, Error>
            {
            }

            public class Error
            {
                public int ActualLength { get; set; }
            }

            public static readonly Func<string, Error> Validator
                = x => x.Length % 2 != 0
                    ? new Error { ActualLength = x.Length }
                    : null;
        }

        public static readonly EvenLengthValidation.Rule EvenLengthRule = new EvenLengthValidation.Rule();
        
        [Fact]
        public void value_only_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValueOnlyValidatorFunc<EvenLengthValidation.Rule, string, EvenLengthValidation.Error>(EvenLengthValidation.Validator);

            var engine = engineBuilder.Build();
            var result = engine.Validate("a", EvenLengthRule);

            result.HasError().Should().BeTrue();
        }
    }
}
