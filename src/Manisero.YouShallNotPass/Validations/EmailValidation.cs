﻿namespace Manisero.YouShallNotPass.Validations
{
    public static class EmailValidation
    {
        public class Rule : IValidationRule<string, Error>
        {
        }

        public class Error
        {
            public static readonly Error Instance = new Error();
        }

        public class Validator : IValidator<Rule, string, Error>
        {
            public Error Validate(string value, Rule rule, ValidationContext context)
            {
                return IsEmail(value)
                    ? null
                    : Error.Instance;
            }

            private bool IsEmail(string value)
            {
                // TODO: Provide some serious implementation
                return value.Split('@').Length == 2;
            }
        }
    }
}
