using System.Collections.Generic;

namespace Manisero.YouShallNotPass.Core.ComplexValidation
{
    public interface IComplexValidation : IValidation<EmptyValidationConfig>
    {
        IDictionary<string, IValidation> MemberValidations { get; }

        // TODO: Consider adding TValue type parameter (IComplexValidation<TValue>) and having IValidation<TValue> here:
        IValidation OverallValidation { get; }
    }

    public class ComplexValidation : IComplexValidation
    {
        public int Type => (int)ValidationType.Complex;

        public EmptyValidationConfig Config => EmptyValidationConfig.Default;
        
        public IDictionary<string, IValidation> MemberValidations { get; set; }

        public IValidation OverallValidation { get; set; }
    }
}
