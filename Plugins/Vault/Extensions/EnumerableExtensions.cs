using System;
using System.Collections.Generic;
using System.Linq;
using DynamicLinqExtensions;

namespace VaultServices.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<IGrouping<object, TElement>> GroupByMany<TElement>(
			this IEnumerable<TElement> elements, params string[] groupSelectors)
		{
			var selectors = new List<Func<TElement, object>>(groupSelectors.Length);
			selectors.AddRange(groupSelectors.Select(selector => DynamicExpression.ParseLambda(typeof(TElement), typeof(object), selector)).Select(l => (Func<TElement, object>)l.Compile()));
			return elements.GroupByMany(selectors.ToArray());
		}

		public static IEnumerable<IGrouping<object, TElement>> GroupByMany<TElement>(
			this IEnumerable<TElement> elements, params Func<TElement, object>[] groupSelectors)
		{
			if (!groupSelectors.Any()) return null;
			var selector = groupSelectors.First();
			return elements.GroupBy(selector);
		}
	}
}