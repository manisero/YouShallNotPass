using System;
using Manisero.YouShallNotPass.Core;
using Manisero.YouShallNotPass.Core.SimpleValidation;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class MinValidation<TValue> : ISimpleValidation<MinValidationConfig<TValue>>
    {
        public int Type => (int)ValidationType.Min;

        public MinValidationConfig<TValue> Config { get; set; }
    }

    public class MinValidationConfig<TValue>
    {
        public TValue MinValue { get; set; }
    }

    public class MinValidator<TValue> : ISimpleValidator<MinValidation<TValue>, TValue, MinValidationConfig<TValue>>
        where TValue : IComparable
    {
        public ISimpleValidationError Validate(TValue value, MinValidationConfig<TValue> config)
        {
            if (value.CompareTo(config.MinValue) < 0)
            {
                return new SimpleValidationError();
            }

            return null;
        }
    }
}
