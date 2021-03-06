﻿namespace Manisero.YouShallNotPass
{
    public interface IValidationRule
    {
    }

    public interface IValidationRule<TValue> : IValidationRule
    {
    }

    public interface IValidationRule<TValue, TError> : IValidationRule<TValue>
        where TError : class
    {
    }

    public static class ValidationRuleInterfaceConstants
    {
        public const int TValueTypeParameterPosition = 0;
        public const int TErrorTypeParameterPosition = 1;
    }
}
