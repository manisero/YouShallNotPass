using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Xunit;

namespace Manisero.YouShallNotPass.Tests.ErrorFormatting
{
    public class FormattersCachingThreadSafetyTests
    {
        public class Rule<TValue> : IValidationRule<TValue, Error<TValue>>
        {
        }

        public class Error<TValue>
        {
        }
        
        public class Formatter<TValue> : IValidationErrorFormatter<Error<TValue>, object>
        {
            public static int InstancesCount;

            public Formatter()
            {
                // TODO: Try to avoid using delay
                Task.Delay(100).Wait();
                Interlocked.Increment(ref InstancesCount);
            }

            public object Format(Error<TValue> error, ValidationErrorFormattingContext<object> context) => null;
        }

        public static readonly ValidationResult<Rule<int>, int, Error<int>> ValidationResult
            = new ValidationResult<Rule<int>, int, Error<int>>
            {
                Rule = new Rule<int>(),
                Value = 1,
                Error = new Error<int>()
            };

        [Fact]
        public void singleton_generic_formatter_is_created_only_once()
        {
            var engine = new ValidationErrorFormattingEngineBuilder<object>()
                .RegisterErrorOnlyGenericFormatterFactory(typeof(Formatter<>),
                                                          type => (IValidationErrorFormatter<object>)Activator.CreateInstance(type))
                .Build();
            
            var formatting1 = Task.Run(() => engine.Format(ValidationResult));
            var formatting2 = Task.Run(() => engine.Format(ValidationResult));
            Task.WaitAll(formatting1, formatting2);

            Formatter<int>.InstancesCount.Should().Be(1);
        }
    }
}
