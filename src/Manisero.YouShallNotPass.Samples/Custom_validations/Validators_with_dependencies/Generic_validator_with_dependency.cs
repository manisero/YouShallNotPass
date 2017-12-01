using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Validators_with_dependencies
{
    public class Generic_validator_with_dependency
    {
        // AllowedValuesRepository

        public interface IAllowedValuesRepository
        {
            bool IsAllowed<TValue>(TValue value);
        }

        public class AllowedValuesRepository : IAllowedValuesRepository
        {
            public bool IsAllowed<TValue>(TValue value) => false;
        }

        // IsAllowedValidation validation

        public static class IsAllowedValidation
        {
            public class Rule<TValue> : IValidationRule<TValue, EmptyValidationError>
            {
            }

            public class Validator<TValue> : IValidator<Rule<TValue>, TValue, EmptyValidationError>
            {
                private readonly IAllowedValuesRepository _allowedValuesRepository;

                public Validator(IAllowedValuesRepository allowedValuesRepository)
                {
                    _allowedValuesRepository = allowedValuesRepository;
                }

                public EmptyValidationError Validate(TValue value, Rule<TValue> rule, ValidationContext context)
                {
                    var isAllowed = _allowedValuesRepository.IsAllowed(value);

                    return isAllowed
                        ? EmptyValidationError.None
                        : EmptyValidationError.Some;
                }
            }
        }

        [Fact]
        public void sample()
        {
            var builder = new ValidationEngineBuilder();

            // You should replace below lambda with your DI Container usage
            builder.RegisterFullGenericValidatorFactory(typeof(IsAllowedValidation.Validator<>),
                                                        type => (IValidator)type.GetConstructor(new[] { typeof(IAllowedValuesRepository) })
                                                                                .Invoke(new object[] { new AllowedValuesRepository() }));

            var engine = builder.Build();

            var rule = new IsAllowedValidation.Rule<int>();

            var result = engine.Validate(1, rule);

            result.HasError().Should().BeTrue();
        }
    }
}
