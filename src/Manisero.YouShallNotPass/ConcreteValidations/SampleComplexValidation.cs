using System.Collections.Generic;
using Manisero.YouShallNotPass.Core;
using Manisero.YouShallNotPass.Core.ComplexValidation;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class SampleComplexType
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }

    public class SampleComplexValidation
    {
        public static ComplexValidationRule Sample = new ComplexValidationRule
        {
            MemberValidations = new Dictionary<string, IValidationRule>
            {
                [nameof(SampleComplexType.Id)] = new MinValidationRule<int>
                {
                    Config = new MinValidationConfig<int>
                    {
                        MinValue = 1
                    }
                },
                [nameof(SampleComplexType.Email)] = new EmailValidationRule()
            }
        };
    }
}
