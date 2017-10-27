using System.Collections.Generic;

namespace Manisero.YouShallNotPass.Core.ComplexValidation
{
    public interface IComplexValidationRule : IValidationRule<EmptyValidationConfig>
    {
        IDictionary<string, IValidationRule> MemberValidations { get; }

        // TODO: Consider adding TValue type parameter (IComplexValidationRule<TValue>) and having IValidationRule<TValue> here:
        IValidationRule OverallValidationRule { get; }
    }

    public class ComplexValidationRule : IComplexValidationRule
    {
        public int Type => (int)ValidationType.Complex;

        public EmptyValidationConfig Config => EmptyValidationConfig.Default;
        
        public IDictionary<string, IValidationRule> MemberValidations { get; set; }

        public IValidationRule OverallValidationRule { get; set; }
    }
}
