﻿using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Manisero.YouShallNotPass.Tests.Core
{
    public class ValidatorsCachingThreadSafetyTests
    {
        public class Rule<TValue> : IValidationRule<TValue, object>
        {
        }

        public class Validator<TValue> : IValidator<Rule<TValue>, TValue, object>
        {
            public static int InstancesCount;

            public Validator()
            {
                // TODO: Try to avoid using delay
                Task.Delay(100).Wait();
                Interlocked.Increment(ref InstancesCount);
            }
            
            public object Validate(TValue value, Rule<TValue> rule, ValidationContext context) => null;
        }

        public static readonly Rule<int> ValidationRule = new Rule<int>();
        public const int Value = 1;

        [Fact]
        public void singleton_generic_validator_is_created_only_once()
        {
            var engine = new ValidationEngineBuilder()
                .RegisterGenericValidatorFactory(typeof(Validator<>),
                                                     type => (IValidator)Activator.CreateInstance(type))
                .Build();

            var validation1 = Task.Run(() => engine.Validate(Value, ValidationRule));
            var validation2 = Task.Run(() => engine.Validate(Value, ValidationRule));
            Task.WaitAll(validation1, validation2);
            
            Validator<int>.InstancesCount.Should().Be(1);
        }
    }
}
