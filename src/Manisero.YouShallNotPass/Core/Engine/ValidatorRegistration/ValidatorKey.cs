using System;

namespace Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration
{
    public struct ValidatorKey
    {
        public Type ValueType { get; }
        public Type RuleType { get; }

        public ValidatorKey(Type valueType, Type ruleType)
        {
            ValueType = valueType;
            RuleType = ruleType;
        }

        public static ValidatorKey Create<TValue, TRule>() => new ValidatorKey(typeof(TValue), typeof(TRule));

        public bool Equals(ValidatorKey other)
        {
            return ValueType == other.ValueType && RuleType == other.RuleType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ValidatorKey && Equals((ValidatorKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ValueType.GetHashCode() * 397) ^ RuleType.GetHashCode();
            }
        }
    }
}
