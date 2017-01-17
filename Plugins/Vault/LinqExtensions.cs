using System;
using System.Collections.Generic;
using System.Linq;

namespace UserServices
{
	public static class LinqExtensions
	{
		public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> elements, Func<T, TKey> keySelector)
		{
			return elements.GroupBy(keySelector).Select(g => g.First());
		}
	}
}
