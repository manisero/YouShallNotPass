using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Validations
{
    public class ComplexValidationRule<TItem> : IValidationRule<TItem, ComplexValidationError>
    {
        /// <summary>property name -> rule</summary>
        public IDictionary<string, IValidationRule> MemberRules { get; set; }
        
        public IValidationRule<TItem> OverallRule { get; set; }
    }

    public class ComplexValidationError
    {
        public static readonly Func<ComplexValidationError> Constructor = () => new ComplexValidationError
        {
            MemberValidationErrors = new Dictionary<string, IValidationResult>()
        };

        /// <summary>property name (only invalid properties) -> validation result</summary>
        public IDictionary<string, IValidationResult> MemberValidationErrors { get; set; }
        
        public IValidationResult OverallValidationError { get; set; }
    }

    public class ComplexValidator<TItem> : IValidator<ComplexValidationRule<TItem>, TItem, ComplexValidationError>,
                                           IAsyncValidator<ComplexValidationRule<TItem>, TItem, ComplexValidationError>
    {
        public ComplexValidationError Validate(TItem value, ComplexValidationRule<TItem> rule, ValidationContext context)
        {
            var error = LightLazy.Create(ComplexValidationError.Constructor);

            if (rule.MemberRules != null)
            {
                foreach (var memberRule in rule.MemberRules)
                {
                    var propertyName = memberRule.Key;
                    var propertyRule = memberRule.Value;

                    // TODO: Cache property getter
                    var propertyValue = value.GetType().GetProperty(propertyName).GetValue(value);
                    var memberResult = context.Engine.Validate(propertyValue, propertyRule);

                    if (memberResult.HasError())
                    {
                        error.Item.MemberValidationErrors.Add(propertyName, memberResult);
                    }
                }
            }

            if (rule.OverallRule != null)
            {
                var overallResult = context.Engine.Validate(value, rule.OverallRule);

                if (overallResult.HasError())
                {
                    error.Item.OverallValidationError = overallResult;
                }
            }

            return error.ItemOrNull;
        }

        public Task<ComplexValidationError> ValidateAsync(TItem value, ComplexValidationRule<TItem> rule, ValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
