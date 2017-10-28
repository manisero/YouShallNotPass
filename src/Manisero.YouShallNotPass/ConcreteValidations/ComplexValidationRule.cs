﻿using System.Collections.Generic;
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

    public class ComplexValidator : IGenericValidator<ComplexValidationRule, ComplexValidationError>
    {
        public ComplexValidationError Validate<TValue>(TValue value, ComplexValidationRule rule, ValidationContext context)
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
                var propertyValue = typeof(TValue).GetProperty(propertyName).GetValue(value);

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
    }
}
