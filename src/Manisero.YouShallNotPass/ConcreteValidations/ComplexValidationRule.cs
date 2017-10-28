using System.Collections.Generic;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Core;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class ComplexValidationRule
    {
        // TODO: Try to avoid object types as rules

        /// <summary>property name -> rule</summary>
        public IDictionary<string, object> MemberRules { get; set; }
        
        public object OverallRule { get; set; }
    }

    public class ComplexValidationError
    {
        /// <summary>property name (only invalid properties) -> validation result</summary>
        public IDictionary<string, IValidationResult> MemberValidationErrors { get; set; }

        public IValidationResult OverallValidationError { get; set; }
    }

    public class ComplexValidator : IValidator<object, ComplexValidationRule, ComplexValidationError>,
                                    IAsyncValidator<object, ComplexValidationRule, ComplexValidationError>
    {
        // TODO: Implement async

        // TODO: Consider avoiding boxing (object value)
        public ComplexValidationError Validate(object value, ComplexValidationRule rule, ValidationContext context)
        {
            var invalid = false;
            var error = new ComplexValidationError // TODO: Avoid this up-front allocation
            {
                MemberValidationErrors = new Dictionary<string, IValidationResult>()
            };

            foreach (var memberRule in rule.MemberRules)
            {
                var propertyName = memberRule.Key;

                // TODO: Cache property getter
                var propertyValue = value.GetType().GetProperty(propertyName).GetValue(value);

                var memberResult = context.Engine.Validate(propertyValue, memberRule);

                if (memberResult.HasError())
                {
                    invalid = true;
                    error.MemberValidationErrors.Add(propertyName, memberResult);
                }
            }

            var overallResult = context.Engine.Validate(value, rule.OverallRule);

            if (overallResult.HasError())
            {
                invalid = true;
                error.OverallValidationError = overallResult;
            }
            
            return invalid
                ? error
                : null;
        }

        public Task<ComplexValidationError> ValidateAsync(object value, ComplexValidationRule rule, ValidationContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
