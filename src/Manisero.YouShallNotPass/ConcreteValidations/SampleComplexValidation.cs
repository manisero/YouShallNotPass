using System.Collections.Generic;

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
            MemberValidations = new Dictionary<string, object>
            {
                [nameof(SampleComplexType.Id)] = new MinValidationRule<int>
                {
                    MinValue = 1
                },
                [nameof(SampleComplexType.Email)] = new EmailValidationRule()
            }
        };
    }
}
