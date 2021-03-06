﻿using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.ErrorFormatting.Core.FormatterRegistration
{
    internal class ValidationErrorFormattersRegistry<TFormat>
    {
        /// <summary>error type -> formatter</summary>
        public IDictionary<Type, IValidationErrorFormatter<TFormat>> ErrorOnlyFormatters { get; set; }
            = new Dictionary<Type, IValidationErrorFormatter<TFormat>>();

        /// <summary>error type -> generic formatter</summary>
        public TypeKeyedGenericOperationRegistry<IValidationErrorFormatter<TFormat>> ErrorOnlyGenericFormatters { get; set; }
            = new TypeKeyedGenericOperationRegistry<IValidationErrorFormatter<TFormat>>();

        /// <summary>rule type -> formatter</summary>
        public IDictionary<Type, IValidationErrorFormatter<TFormat>> FullFormatters { get; set; }
            = new Dictionary<Type, IValidationErrorFormatter<TFormat>>();

        /// <summary>rule type -> generic formatter</summary>
        public TypeKeyedGenericOperationRegistry<IValidationErrorFormatter<TFormat>> FullGenericFormatters { get; set; }
            = new TypeKeyedGenericOperationRegistry<IValidationErrorFormatter<TFormat>>();
    }
}
