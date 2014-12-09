using System;
using System.Collections.Generic;
using System.Linq;

namespace SapServices.Converters
{
	public class NumberListConverter : ITypeConverter<IEnumerable<int>, string>
	{
		private const string Delimiter = ";";

		public string ConvertTo(IEnumerable<int> from)
		{
			return string.Join(Delimiter, from);
		}

		public IEnumerable<int> ConvertFrom(string to)
		{
			return to.Split(new[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries)
				.Select(e => Convert.ToInt32((string) e));
		}
	}
}