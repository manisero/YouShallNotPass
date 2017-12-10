using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Xunit;

namespace Manisero.YouShallNotPass.ErrorMessages.Tests.Errors_merging.Dictionary_entries
{
    public class Samples
    {
        static Samples()
        {
            AssertionOptions.AssertEquivalencyUsing(c => c.RespectingRuntimeTypes());
        }

        private IValidationFacade CreateValidationFacade()
        {
            var validationEngine = new ValidationEngineBuilder().Build();
            var formattingEngine = new ValidationErrorFormattingEngineBuilderFactory().Create().Build();

            return new ValidationFacade(validationEngine, formattingEngine);
        }

        [Fact]
        public void errors_regarding_same_entry_are_merged()
        {
            var validationFacade = CreateValidationFacade();

            var command = new Case.UpdateEmailsCommand
            {
                UserIdToEmail = new Dictionary<int, string> { [1] = "a" }
            };

            var error = validationFacade.Validate(command, Case.Rule);
            error.Should().NotBeNull("Validation is expected to fail.");

            error.ShouldBeEquivalentTo(new[]
            {
                new MemberValidationErrorMessage
                {
                    MemberName = nameof(Case.UpdateEmailsCommand.UserIdToEmail),
                    Errors = new IValidationErrorMessage[]
                    {
                        new EntryValidationErrorMessage
                        {
                            EntryKey = 1,
                            Errors = new IValidationErrorMessage[]
                            {
                                new MinLengthValidationErrorMessage { MinLength = 5 },
                                new ValidationErrorMessage { Code = ErrorCodes.Email }
                            }
                        }
                    }
                }
            });
        }
    }
}
