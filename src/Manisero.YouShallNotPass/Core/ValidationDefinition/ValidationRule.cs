﻿namespace Manisero.YouShallNotPass.Core.ValidationDefinition
{
    public interface IValidationRule
    {
    }

    public interface IValidationRule<TValue> : IValidationRule
    {
    }

    public interface IValidationRule<TValue, TError> : IValidationRule
        where TError : class
    {
    }

    public static class ValidationRuleInterfaceConstants
    {
        public const int TErrorTypeParamterPosition = 1;
    }
}
