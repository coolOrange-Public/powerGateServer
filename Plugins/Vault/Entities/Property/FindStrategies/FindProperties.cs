using System;
using System.Collections.Generic;
using VaultServices.Entities.Base;
using VaultServices.Entities.Base.FindStrategies;

namespace VaultServices.Entities.Property.FindStrategies
{
	public class FindProperties : QueryNavigationProperty<Property>
    {
		public FindProperties(IExpressionParser<Property> expressionParser)
			: base(expressionParser)
        {
        }

		protected override Func<IBaseObject, IEnumerable<Property>> GetNavigationProperties
		{
			get { return e => e.Properties; }
		}
    }
}