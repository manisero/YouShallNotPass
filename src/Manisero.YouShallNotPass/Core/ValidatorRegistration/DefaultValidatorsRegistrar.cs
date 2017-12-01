using System;
using System.Reflection;

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        public static void Register(IValidationEngineBuilder builder)
        {
            var registrationFields = typeof(DefaultValidatorsRegistrar).GetFields(BindingFlags.NonPublic | BindingFlags.Static);

            foreach (var field in registrationFields)
            {
                var registration = (Action<IValidationEngineBuilder>)field.GetValue(null);
                registration(builder);
            }
        }
    }
}
