using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Utils;

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
            MemberValidationErrors = new Dictionary<string, object>()
        };

        /// <summary>property name (only invalid properties) -> validation error</summary>
        public IDictionary<string, object> MemberValidationErrors { get; set; }
        
        public object OverallValidationError { get; set; }
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
                    var memberError = context.Engine.Validate(propertyValue, propertyRule);

                    if (memberError != null)
                    {
                        error.Item.MemberValidationErrors.Add(propertyName, memberError);
                    }
                }
            }

            if (rule.OverallRule != null)
            {
                var overallError = context.Engine.Validate(value, rule.OverallRule);

                if (overallError != null)
                {
                    error.Item.OverallValidationError = overallError;
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
