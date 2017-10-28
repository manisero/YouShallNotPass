using System.Collections.Generic;
using Manisero.YouShallNotPass.ConcreteValidations;
using Manisero.YouShallNotPass.Core;

namespace Manisero.YouShallNotPass
{
    public class SampleType
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public ICollection<int> ChildIds { get; set; }
    }

    public class Sample
    {
        public static ComplexValidationRule Rule = new ComplexValidationRule
        {
            MemberRules = new Dictionary<string, object>
            {
                [nameof(SampleType.Id)] = new MinValidationRule<int>
                {
                    MinValue = 1
                },
                [nameof(SampleType.Email)] = new EmailValidationRule(),
                [nameof(SampleType.ChildIds)] = new CollectionValidationRule
                {
                    ItemRule = new MinValidationRule<int>
                    {
                        MinValue = 1
                    }
                }
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

            engine.RegisterValidator(new ComplexValidator());
            engine.RegisterValidator(new CollectionValidator());
            engine.RegisterValidator(typeof(MinValidator<>), type => new object()); // TODO: Return proper validator
            engine.RegisterValidator(new EmailValidator());
            
            engine.Validate(SampleItem, Rule);
        }
    }
}
