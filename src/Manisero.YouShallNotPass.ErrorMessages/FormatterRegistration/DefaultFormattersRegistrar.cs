using System;
using System.Collections.Generic;
using System.Reflection;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.FormatterRegistration
{
    internal static partial class DefaultFormattersRegistrar
    {
        public static void Register(IValidationErrorFormattingEngineBuilder<IEnumerable<IValidationErrorMessage>> builder)
        {
            var registrationFields = typeof(DefaultFormattersRegistrar).GetFields(BindingFlags.NonPublic | BindingFlags.Static);

            foreach (var field in registrationFields)
            {
                var registration = (Action<IValidationErrorFormattingEngineBuilder<IEnumerable<IValidationErrorMessage>>>)field.GetValue(null);
                registration(builder);
            }

            builder.RegisterEmptyErrorMessage<EmailValidation.Error>(ErrorCodes.Email);
            builder.RegisterEmptyErrorMessage<IsEnumValueValidation.Error>(ErrorCodes.IsEnumValue);
            builder.RegisterEmptyErrorMessage<NotEmptyValidation.Error>(ErrorCodes.NotEmpty);
            builder.RegisterEmptyErrorMessage<NotNullValidation.Error>(ErrorCodes.NotNull);
            builder.RegisterEmptyErrorMessage<NotNullNorWhiteSpaceValidation.Error>(ErrorCodes.NotNullNorWhiteSpace);
            builder.RegisterEmptyErrorMessage<NullValidation.Error>(ErrorCodes.Null);
        }
    }
}
