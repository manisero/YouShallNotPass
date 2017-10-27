using System.Collections.Generic;
using Manisero.YouShallNotPass.Core;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class ComplexValidationRule
    {
        // TODO: Try to avoid object types as rules

        public IDictionary<string, object> MemberRules { get; set; }
        
        public object OverallRule { get; set; }
    }

    public class ComplexValidationError
    {
        public IDictionary<string, IValidationError> MemberValidationErrors { get; set; }

        public IValidationError OverallValidationError { get; set; }
    }

    public class ComplexValidator : IGenericValidator<ComplexValidationRule, ComplexValidationError>
    {
        public ComplexValidationError Validate<TValue>(TValue value, ComplexValidationRule rule, ValidationContext context)
        {
            var invalid = false;
            var error = new ComplexValidationError
            {
                MemberValidationErrors = new Dictionary<string, IValidationError>()
            };

            foreach (var memberRule in rule.MemberRules)
            {
                var propertyName = memberRule.Key;

                // TODO: Cache property getter
                var propertyValue = typeof(TValue).GetProperty(propertyName).GetValue(value);

                var memberError = context.Engine.Validate(propertyValue, memberRule);

                if (memberError != null)
                {
                    invalid = true;
                    error.MemberValidationErrors[propertyName] = memberError;
                }
            }

            var overallError = context.Engine.Validate(value, rule.OverallRule);

            if (overallError != null)
            {
                invalid = true;
                error.OverallValidationError = overallError;
            }
            
            return invalid
                ? error
                : null;
        }
    }
}
