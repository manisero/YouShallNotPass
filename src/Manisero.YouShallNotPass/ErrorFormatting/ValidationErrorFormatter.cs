﻿namespace Manisero.YouShallNotPass.ErrorFormatting
{
    public interface IValidationErrorFormatter<TFormat>
    {
    }

    public interface IValidationErrorFormatter<TError, TFormat> : IValidationErrorFormatter<TFormat>
        where TError : class
    {
        TFormat Format(
            TError error,
            ValidationErrorFormattingContext<TFormat> context);
    }

    public interface IValidationErrorFormatter<TRule, TValue, TError, TFormat> : IValidationErrorFormatter<TFormat>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        TFormat Format(
            ValidationResult<TRule, TValue, TError> validationResult,
            ValidationErrorFormattingContext<TFormat> context);
    }
}
