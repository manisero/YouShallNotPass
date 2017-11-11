using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
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

        public class IsAllowedValidationRule<TValue> : IValidationRule<TValue, CustomMessageValidationError>
        {
        }

        public class Validator<TValue> : IValidator<IsAllowedValidationRule<TValue>, TValue, CustomMessageValidationError>
        {
            private readonly IAllowedValuesRepository _allowedValuesRepository;

            public Validator(IAllowedValuesRepository allowedValuesRepository)
            {
                _allowedValuesRepository = allowedValuesRepository;
            }

            public CustomMessageValidationError Validate(TValue value, IsAllowedValidationRule<TValue> rule, ValidationContext context)
            {
                var isAllowed = _allowedValuesRepository.IsAllowed(value);

                return isAllowed
                    ? null
                    : new CustomMessageValidationError();
            }
        }

        [Fact]
        public void sample()
        {
            var builder = new ValidationEngineBuilder();

            // You should replace below lambda with your DI Container usage
            builder.RegisterGenericValidatorFactory(typeof(Validator<>),
                                                    type => (IValidator)type.GetConstructor(new[] { typeof(IAllowedValuesRepository) })
                                                                            .Invoke(new object[] { new AllowedValuesRepository() }));

            var engine = builder.Build();

            var rule = new IsAllowedValidationRule<int>();

            var error = engine.Validate(1, rule);

            error.Should().NotBeNull();
        }
    }
}
