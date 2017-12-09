﻿using System.Collections.Generic;
using System.Linq;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.ErrorMessages.Utils;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Formatters
{
    public class MemberValidationErrorMessage : IValidationErrorMessage
    {
        public string Code => "Member";

        public string MemberName { get; set; }

        public ICollection<IValidationErrorMessage> Errors { get; set; }
    }

    public class MemberValidationErrorFormatter<TOwner, TValue> : IValidationErrorFormatter<MemberValidation.Rule<TOwner, TValue>,
                                                                                            TOwner,
                                                                                            MemberValidation.Error,
                                                                                            IEnumerable<IValidationErrorMessage>>
    {
        public IEnumerable<IValidationErrorMessage> Format(
            ValidationResult<MemberValidation.Rule<TOwner, TValue>, TOwner, MemberValidation.Error> validationResult,
            ValidationErrorFormattingContext<IEnumerable<IValidationErrorMessage>> context)
        {
            return new MemberValidationErrorMessage
            {
                MemberName = validationResult.Rule.MemberName,
                Errors = context.Engine.Format(validationResult.Error.Violation).ToArray()
            }.ToEnumerable();
        }
    }
}
