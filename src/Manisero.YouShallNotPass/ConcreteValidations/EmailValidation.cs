﻿using Manisero.YouShallNotPass.ValidationErrors;
using Manisero.YouShallNotPass.Validations;
using Manisero.YouShallNotPass.Validators;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class EmailValidation : ISimpleValidation<EmptyValidationConfig>
    {
        public int Type => (int)ValidationType.Email;

        public EmptyValidationConfig Config => throw new System.NotImplementedException();
    }

    public class EmailValidator : ISimpleValidator<EmailValidation, string, EmptyValidationConfig>
    {
        public ISimpleValidationError Validate(string value, EmptyValidationConfig config)
        {
            if (!IsEmail(value))
            {
                return new SimpleValidationError();
            }

            return null;
        }

        private bool IsEmail(string value)
        {
            throw new System.NotImplementedException();
        }
    }
}
