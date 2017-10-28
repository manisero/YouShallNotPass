using System.Collections;
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
            var builder = new ValidationEngineBuilder();

            builder.RegisterValidator<ComplexValidationRule, object, ComplexValidationError, ComplexValidator>(new ComplexValidator())
                   .RegisterValidator<CollectionValidationRule, IEnumerable, CollectionValidationError, CollectionValidator>(new CollectionValidator())
                   .RegisterValidator(typeof(MinValidator<>), type => new object()) // TODO: Return proper validator
                   .RegisterValidator<EmailValidationRule, string, EmptyValidationError, EmailValidator>(new EmailValidator());

            var engine = builder.Build();
            engine.Validate(SampleItem, Rule);
        }
    }
}
