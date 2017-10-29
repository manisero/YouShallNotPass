using System.Reflection;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public interface IRuleMetadataProvider
    {
        bool ValidatesNull(IValidationRule rule);
    }

    public class RuleMetadataProvider : IRuleMetadataProvider
    {
        public bool ValidatesNull(IValidationRule rule)
        {
            // TODO: Cache result

            var validatesNullAttribute = rule.GetType().GetCustomAttribute<ValidatesNullAttribute>();

            return validatesNullAttribute != null;
        }
    }
}
