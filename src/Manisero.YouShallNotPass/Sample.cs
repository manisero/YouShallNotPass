using System.Collections.Generic;
using Manisero.YouShallNotPass.ConcreteValidations;
using Manisero.YouShallNotPass.Core;

namespace Manisero.YouShallNotPass
{
    public class SampleType
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }

    public class Sample
    {
        public static ComplexValidationRule Rule = new ComplexValidationRule
        {
            MemberRules = new Dictionary<string, object>
            {
                [nameof(SampleType.Id)] = new MinValidationRule
                {
                    MinValue = 1
                },
                [nameof(SampleType.Email)] = new EmailValidationRule()
            }
        };

        public static SampleType SampleItem = new SampleType
        {
            Id = 1,
            Email = "a@a.com"
        };

        public void Run()
        {
            var engine = new ValidationEngine();

            engine.RegisterGenericRule(() => new ComplexValidator());
            engine.RegisterGenericRule(() => new MinValidator());
            engine.RegisterRule(() => new EmailValidator());
            
            engine.Validate(SampleItem, Rule);
        }
    }
}
