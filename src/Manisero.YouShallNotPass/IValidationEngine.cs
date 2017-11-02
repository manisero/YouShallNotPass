using System.Threading.Tasks;
using Manisero.YouShallNotPass.Core.Engine;

namespace Manisero.YouShallNotPass
{
    public interface IValidationEngine
    {
        IValidationResult Validate(object value, IValidationRule rule);

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<IValidationResult> ValidateAsync(object value, IValidationRule rule);
    }
}
