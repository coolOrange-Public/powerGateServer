using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace ErpServices
{
	public static class TypeExtensions
	{
		public static bool IsEnumerable(this Type @this)
		{
			return typeof(IEnumerable).IsAssignableFrom(@this) || @this.IsArray;
		}

		public static Type GetElementTypeFromEnumerable(this Type @this)
		{
			if (@this.IsArray)
				return Type.GetType(@this.FullName.Replace("[]", string.Empty));
			if (typeof(IEnumerable).IsAssignableFrom(@this))
				return @this.GetGenericArguments()[0];
			throw new NotImplementedException();
		}

		public static bool IsDefaultValue(this object instance, Type instanceType)
		{
			var defaultValue = instanceType.GetDefaultValue();
			return Equals(instance, defaultValue);
		}

		public static object GetDefaultValue(this Type t)
		{
			if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
				return Activator.CreateInstance(t);
			return null;
		}

		public static object ToArray(this IEnumerable enumerable, Type t)
		{
			var toArrayMethods = typeof(Enumerable)
						   .GetMethod("ToArray", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(new[] { t });
			return toArrayMethods.Invoke(enumerable, new[] { enumerable });
		}
	}
}
