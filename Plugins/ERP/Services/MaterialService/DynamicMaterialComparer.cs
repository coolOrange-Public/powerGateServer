using System;
using ErpServices.Services.MaterialService.Entities;

namespace ErpServices.Services.MaterialService
{
	public class DynamicMaterialComparer : IEquatable<Material>
	{
		private readonly dynamic _source;

		public DynamicMaterialComparer(dynamic source)
		{
			_source = source;
		}

		public bool Equals(Material context)
		{
			return Equals(context.Number, _source.Number);
		}
	}
}