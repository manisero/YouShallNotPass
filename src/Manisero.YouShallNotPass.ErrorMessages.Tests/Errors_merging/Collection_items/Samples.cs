using FluentAssertions;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Xunit;

namespace Manisero.YouShallNotPass.ErrorMessages.Tests.Errors_merging.Collection_items
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
        public void errors_regarding_same_item_are_merged()
        {
            var validationFacade = CreateValidationFacade();

            var command = new Case.ProcessEmailsCommand
            {
                Emails = new[] { "a" }
            };

            var error = validationFacade.Validate(command, Case.Rule);
            error.Should().NotBeNull("Validation is expected to fail.");

            error.ShouldBeEquivalentTo(new[]
            {
                new MemberValidationErrorMessage
                {
                    MemberName = nameof(Case.ProcessEmailsCommand.Emails),
                    Errors = new IValidationErrorMessage[]
                    {
                        new ItemValidationErrorMessage
                        {
                            ItemIndex = 0,
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

        [Fact]
        public void merged_error_does_not_contain_rules_violated_by_other_items()
        {
            var validationFacade = CreateValidationFacade();

            var command = new Case.ProcessEmailsCommand
            {
                Emails = new[] { "aaaaa", "a@a" }
            };

            var error = validationFacade.Validate(command, Case.Rule);
            error.Should().NotBeNull("Validation is expected to fail.");

            error.ShouldBeEquivalentTo(new[]
            {
                new MemberValidationErrorMessage
                {
                    MemberName = nameof(Case.ProcessEmailsCommand.Emails),
                    Errors = new IValidationErrorMessage[]
                    {
                        new ItemValidationErrorMessage
                        {
                            ItemIndex = 0,
                            Errors = new IValidationErrorMessage[]
                            {
                                new ValidationErrorMessage { Code = ErrorCodes.Email }
                            }
                        },
                        new ItemValidationErrorMessage
                        {
                            ItemIndex = 1,
                            Errors = new IValidationErrorMessage[]
                            {
                                new MinLengthValidationErrorMessage { MinLength = 5 }
                            }
                        }
                    }
                }
            });
        }
    }
}
