using System;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static class DefaultValidatorsRegistrar
    {
        public static readonly Action<IValidationEngineBuilder> All = x => x.RegisterFullGenericValidator(typeof(AllValidation.Validator<>));

        public static void Register(IValidationEngineBuilder builder)
        {
            All(builder);
            builder.RegisterFullGenericValidator(typeof(AnyValidation.Validator<>));
            builder.RegisterFullGenericValidator(typeof(AtLeastNValidation.Validator<>));
            builder.RegisterFullGenericValidator(typeof(CollectionValidation.Validator<>));
            builder.RegisterFullValidator(new EmailValidation.Validator());
            builder.RegisterFullGenericValidator(typeof(MemberValidation.Validator<,>));
            builder.RegisterFullValidator(new MinLengthValidation.Validator());
            builder.RegisterFullGenericValidator(typeof(MinValidation.Validator<>));
            builder.RegisterFullValidator(new NotNullNorWhiteSpaceValidation.Validator());
            builder.RegisterFullGenericValidator(typeof(NotNullValidation.Validator<>));
        }
    }
}
