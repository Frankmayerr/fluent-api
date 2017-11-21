using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Xml.Schema;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using static System.Int32;

namespace ObjectPrinting
{
	public class PrintingConfig<TOwner>
	{
		public int MaxLength = MaxValue;
		private readonly HashSet<Type> excludeTypesList = new HashSet<Type>();
		private readonly HashSet<string> excludePropList = new HashSet<string>();
		public readonly Dictionary<Type, Delegate> TypesWithSpecialSerialization
			= new Dictionary<Type, Delegate>();
		public readonly Dictionary<string, Delegate> PropertiesWithSpecialSerialization
			= new Dictionary<string, Delegate>();
		internal readonly Dictionary<Type, CultureInfo> Cultures = new Dictionary<Type, CultureInfo>();

		public string PrintToString(TOwner obj)
		{
			return PrintToString(obj, 0);
		}

		private string PrintToString(object obj, int nestingLevel)
		{
			//TODO apply configurations
			if (obj == null)
				return "null" + Environment.NewLine;

			var finalTypes = new[]
			{
				typeof(int), typeof(double), typeof(float), typeof(string),
				typeof(DateTime), typeof(TimeSpan)
			};
			if (finalTypes.Contains(obj.GetType()))
				if (obj is string str)
					return obj.ToString().Substring(0, Math.Min(MaxLength, obj.ToString().Length)) + Environment.NewLine;
			else
				return obj.ToString() + Environment.NewLine;

			var identation = new string('\t', nestingLevel + 1);
			var sb = new StringBuilder();
			var type = obj.GetType();
			sb.AppendLine(type.Name);
			foreach (var propertyInfo in type.GetProperties())
			{
				if (excludePropList.Contains(propertyInfo.Name))
					continue;
				if (excludeTypesList.Contains(propertyInfo.PropertyType))
					continue;
				sb.Append(identation + propertyInfo.Name + " = " +
						  PropertyPrinting(obj, propertyInfo, nestingLevel));
			}
			return sb.ToString();
		}

		private string PropertyPrinting(object obj, PropertyInfo propertyInfo, int nestingLevel)
		{
			if (PropertiesWithSpecialSerialization.ContainsKey(propertyInfo.Name))
				return PropertiesWithSpecialSerialization[propertyInfo.Name]
					       .DynamicInvoke(propertyInfo.GetValue(obj)) + Environment.NewLine;

			if (TypesWithSpecialSerialization.ContainsKey(propertyInfo.PropertyType))
				return TypesWithSpecialSerialization[propertyInfo.PropertyType]
					       .DynamicInvoke(propertyInfo.GetValue(obj)) + Environment.NewLine;

			if (Cultures.ContainsKey(propertyInfo.PropertyType))
			{
				return ((IFormattable)propertyInfo.GetValue(obj))
				       .ToString(null, CultureInfo.CurrentCulture) + Environment.NewLine;
			}

			return PrintToString(propertyInfo.GetValue(obj),
				nestingLevel + 1);
		}


		public PrintingConfig<TOwner> ExcludeType<TPropType>()
		{
			excludeTypesList.Add(typeof(TPropType));
			return this;
		}

		public PrintingConfig<TOwner> ExcludeProperty<TPropType>(
			Expression<Func<TOwner, TPropType>> selector)
		{
			excludePropList.Add(GetPropertyName(selector));
			return this;
		}

		private static string GetPropertyName<TPropType>(Expression<Func<TOwner, TPropType>> propExtractor)
		{
			var propInfo = ((MemberExpression)propExtractor.Body).Member as PropertyInfo;
			return propInfo.Name;

		}

		public PropertyPrintingConfig<TOwner, TPropType> Print<TPropType>()
		{
			return new PropertyPrintingConfig<TOwner, TPropType>(this);
		}

		public PropertyPrintingConfig<TOwner, TPropType> Print<TPropType>(
			Expression<Func<TOwner, TPropType>> selector)
		{
			var property = ((MemberExpression)selector.Body).Member as PropertyInfo;
			return new PropertyPrintingConfig<TOwner, TPropType>(this, property);
		}

		internal void AddNewTypeSerializing(Type type, Delegate serializer)
		{
			TypesWithSpecialSerialization[type] = serializer;
		}

		internal void AddNewPropSerializing(string property, Delegate serializer)
		{
			PropertiesWithSpecialSerialization[property] = serializer;
		}
	}
}