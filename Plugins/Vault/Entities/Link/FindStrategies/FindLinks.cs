using System;
using System.Collections.Generic;
using System.Linq;
using VaultServices.Entities.Base;
using VaultServices.Entities.Base.FindStrategies;

namespace VaultServices.Entities.Link.FindStrategies
{
	public class FindLinks : QueryNavigationProperty<Link>
	{
		public FindLinks(IExpressionParser<Link> expressionParser) : base(expressionParser)
		{
		}

		protected override Func<IBaseObject, IEnumerable<Link>> GetNavigationProperties
		{
			get
			{
				return e =>
				{
					var searchForParents = SearchFor("Parent");
					var searchForChildren = SearchFor("Child");
					if (searchForParents && !searchForChildren)
						return e.Parents;
					if (searchForChildren && !searchForParents)
						return e.Children;
					return e.Children.Concat(e.Parents);
				};
			}
		}

		bool SearchFor(string propertyName)
		{
			if (!Expression.Where.IsSet())
				return true;
			return Expression.Where.All(w => !w.PropertyName.StartsWith(propertyName) && w.PropertyName != "Description");
		}
	}
}