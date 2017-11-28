using System;
using System.Linq.Expressions;

namespace Manisero.YouShallNotPass.Utils
{
    internal static class ExpressionExtensions
    {
        public static string ToMemberName<TOwner, TMember>(this Expression<Func<TOwner, TMember>> memberGetter)
        {
            if (memberGetter.Body is MemberExpression member)
            {
                return member.Member.Name;
            }

            throw new ArgumentException("Expression is not a member access.", nameof(memberGetter));
        }
    }
}
