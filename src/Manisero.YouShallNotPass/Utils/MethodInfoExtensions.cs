using System;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Manisero.YouShallNotPass.Utils
{
    public static class MethodInfoExtensions
    {
        public static TResult InvokeAsGeneric<TResult>(
            this MethodInfo method,
            object target,
            Type typeArg1, Type typeArg2, Type typeArg3,
            params object[] arguments)
        {
            try
            {
                return (TResult)method.MakeGenericMethod(typeArg1, typeArg2, typeArg3)
                                      .Invoke(target, arguments);
            }
            catch (TargetInvocationException exception)
            {
                ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                throw;
            }
        }
    }
}
