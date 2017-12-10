using System.Collections.Generic;
using System.Linq;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Tests.Errors_merging.Collection_items
{
    public class Case
    {
        public class ProcessEmailsCommand
        {
            public ICollection<string> Emails { get; set; }
        }

        public static IValidationRule<ProcessEmailsCommand> Rule = new ValidationRuleBuilder<ProcessEmailsCommand>()
            .All(b => b.Member(
                     x => x.Emails,
                     b1 => b1.Map(
                         x => x.AsEnumerable(),
                         b2 => b2.All(
                             b3 => b3.Collection(b4 => b4.MinLength(5)),
                             b3 => b3.Collection(b4 => b4.Email())))));
    }
}
