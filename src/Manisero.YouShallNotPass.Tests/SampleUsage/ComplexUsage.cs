using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Tests.SampleUsage
{
    public class ComplexUsage
    {
        public static ComplexValidationRule Rule = new ComplexValidationRule
        {
            MemberRules = new Dictionary<string, IValidationRule>
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

        [Fact]
        public void run()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterValidator(() => new ComplexValidator())
                   .RegisterValidator(new CollectionValidator())
                   .RegisterValidator(typeof(MinValidator<>), type => new object()) // TODO: Return proper validator
                   .RegisterValidator(new EmailValidator());

            var engine = builder.Build();
            engine.Validate(SampleItem, Rule);

            // TODO: Assert
        }
    }
}
