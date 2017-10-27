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
        public static ComplexValidation Sample = new ComplexValidation
        {
            MemberValidations = new Dictionary<string, IValidation>
            {
                [nameof(SampleComplexType.Id)] = new MinValidation<int>
                {
                    Config = new MinValidationConfig<int>
                    {
                        MinValue = 1
                    }
                },
                [nameof(SampleComplexType.Email)] = new EmailValidation()
            }
        };
    }
}
