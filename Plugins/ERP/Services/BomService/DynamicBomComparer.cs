using System;
using ErpServices.Services.BomService.Entities;

namespace ErpServices.Services.BomService
{
	public class DynamicBomComparer : IEquatable<Bom>
	{
		private readonly dynamic _source;

		public DynamicBomComparer(dynamic source)
		{
			_source = source;
		}

		public bool Equals(Bom bom)
		{
			return Equals(bom.ParentNumber, _source.ParentNumber);
		}
	}
}