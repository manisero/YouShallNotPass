using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Validations
{
    public static class ComplexValidation
    {
        public class Rule<TItem> : IValidationRule<TItem, Error>
        {
            public IValidationRule<TItem> OverallRule { get; set; }

            /// <summary>property name -> rule</summary>
            public IDictionary<string, IValidationRule> MemberRules { get; set; }
        }

        public class Error
        {
            public static readonly Func<Error> Constructor = () => new Error
            {
                MemberViolations = new Dictionary<string, IValidationResult>()
            };

            public IValidationResult OverallViolation { get; set; }

            /// <summary>property name (only invalid properties) -> validation result</summary>
            public IDictionary<string, IValidationResult> MemberViolations { get; set; }
        }

        public class Validator<TItem> : IValidator<Rule<TItem>, TItem, Error>,
                                        IAsyncValidator<Rule<TItem>, TItem, Error>
        {
            public Error Validate(TItem value, Rule<TItem> rule, ValidationContext context)
            {
                var error = LightLazy.Create(Error.Constructor);

                if (rule.OverallRule != null)
                {
                    var overallResult = context.Engine.Validate(value, rule.OverallRule);

                    if (overallResult.HasError())
                    {
                        error.Item.OverallViolation = overallResult;
                    }
                }

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
                            error.Item.MemberViolations.Add(propertyName, memberResult);
                        }
                    }
                }

                return error.ItemOrNull;
            }

            public Task<Error> ValidateAsync(TItem value, Rule<TItem> rule, ValidationContext context)
            {
                throw new NotImplementedException();
            }
        }
    }
}
