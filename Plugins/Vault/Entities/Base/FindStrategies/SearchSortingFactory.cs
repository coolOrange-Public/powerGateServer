using Autodesk.Connectivity.WebServices;
using powerGateServer.SDK;

namespace VaultServices.Entities.Base.FindStrategies
{
	public class SearchSortingFactory
	{
		public SrchSort Create<T>(PropDef orderByProperty, IOrderByToken<T> orderbyToken)
		{
			return new SrchSort
			{
				PropDefId = orderByProperty.Id,
				SortAsc = orderbyToken.Method == OrderingMethod.OrderBy || orderbyToken.Method == OrderingMethod.ThenBy
			};
		}
	}
}