﻿using System;
using Manisero.YouShallNotPass.Core;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class MinValidationRule<TValue> : IValidationRule<EmptyValidationError>
        where TValue : IComparable<TValue>
    {
        public TValue MinValue { get; set; }
    }

    public class MinValidator<TValue> : IValidator<TValue, MinValidationRule<TValue>, EmptyValidationError>
        where TValue : IComparable<TValue>
    {
        public EmptyValidationError Validate(TValue value, MinValidationRule<TValue> rule, ValidationContext context)
        {
            return value.CompareTo(rule.MinValue) < 0
                ? EmptyValidationError.Some
                : EmptyValidationError.None;
        }
    }
}
