using System;
using System.Linq.Expressions;

namespace Traffix.Core
{

    public static class PropertyExtensions
    {

        public static string GetPropertySymbol<T, R>(this T obj, Expression<Func<T, R>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }

    }

}
