using System.Collections.Generic;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Tests.Errors_merging.Dictionary_entries
{
    public class Case
    {
        public class UpdateEmailsCommand
        {
            public IDictionary<int, string> UserIdToEmail { get; set; }
        }

        public static IValidationRule<UpdateEmailsCommand> Rule = new ValidationRuleBuilder<UpdateEmailsCommand>()
            .All(b => b.Member(
                     x => x.UserIdToEmail,
                     b1 => b1.All(
                         b2 => b2.Dictionary(b3 => b3.MinLength(5)),
                         b2 => b2.Dictionary(b3 => b3.Email()))));
    }
}
