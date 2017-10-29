﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Validations
{
    public class ComplexValidationRule : IValidationRule<ComplexValidationError>
    {
        /// <summary>property name -> rule</summary>
        public IDictionary<string, IValidationRule> MemberRules { get; set; }
        
        public IValidationRule OverallRule { get; set; }
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
                var propertyRule = memberRule.Value;

                // TODO: Cache property getter
                var propertyValue = value.GetType().GetProperty(propertyName).GetValue(value);
                
                var memberResult = context.Engine.Validate(propertyValue, propertyRule);

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