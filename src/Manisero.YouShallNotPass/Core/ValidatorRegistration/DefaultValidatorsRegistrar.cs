using System;
using System.Reflection;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        public static void Register(IValidationEngineBuilder builder)
        {
            var registrationFields = typeof(DefaultValidatorsRegistrar).GetFields(BindingFlags.NonPublic | BindingFlags.Static);

            foreach (var field in registrationFields)
            {
                var registration = (Action<IValidationEngineBuilder>)field.GetValue(null);
                registration(builder);
            }

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
