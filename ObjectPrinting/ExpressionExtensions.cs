using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ObjectPrinting
{
	public static class ExpressionExtensions
	{
		public static string GetPropertyName<TOwner, TPropType>(this Expression<Func<TOwner, TPropType>> propExtractor)
		{
			var propInfo = ((MemberExpression)propExtractor.Body).Member as PropertyInfo;
			return propInfo.Name;
		}
	}
}