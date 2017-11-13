using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.RuleKeyedOperationRegistration;

namespace Manisero.YouShallNotPass.ErrorFormatting.Engine.FormatterRegistration
{
    public class ValidationErrorFormattersRegistry<TFormat>
    {
        public IDictionary<Type, IValidationErrorFormatter<TFormat>> ErrorOnlyFormatters { get; set; }

        public OperationsRegistry<IValidationErrorFormatter<TFormat>> FullFormattersRegistry { get; set; }
    }
}
