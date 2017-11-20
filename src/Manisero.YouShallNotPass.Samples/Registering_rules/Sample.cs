using System.Collections.Generic;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Registering_rules
{
    public class Sample
    {
        public class CreateUserCommand
        {
            public int UserId { get; set; }
        }

        public static readonly ComplexValidationRule<CreateUserCommand> Rule = new ComplexValidationRule<CreateUserCommand>
        {
            MemberRules = new Dictionary<string, IValidationRule>
            {
                [nameof(CreateUserCommand.UserId)] = new MinValidationRule<int> { MinValue = 1 }
            }
        };

        [Fact]
        public void sample()
        {
            var engineBuilder = new ValidationEngineBuilder();
            var engine = engineBuilder.Build();

            //engine.RegisterRule(Rule);
        }
    }
}
