using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.ErrorFormatting.Core.FormatterRegistration
{
    public class ValidationErrorFormattersRegistry<TFormat>
    {
        public IDictionary<Type, IValidationErrorFormatter<TFormat>> ErrorOnlyFormatters { get; set; }
            = new Dictionary<Type, IValidationErrorFormatter<TFormat>>();

        public TypeKeyedGenericOperationRegistry<IValidationErrorFormatter<TFormat>> ErrorOnlyGenericFormatters { get; set; }
            = new TypeKeyedGenericOperationRegistry<IValidationErrorFormatter<TFormat>>();

        public IDictionary<Type, IValidationErrorFormatter<TFormat>> FullFormatters { get; set; }
            = new Dictionary<Type, IValidationErrorFormatter<TFormat>>();

        public TypeKeyedGenericOperationRegistry<IValidationErrorFormatter<TFormat>> FullGenericFormatters { get; set; }
            = new TypeKeyedGenericOperationRegistry<IValidationErrorFormatter<TFormat>>();
    }
}
