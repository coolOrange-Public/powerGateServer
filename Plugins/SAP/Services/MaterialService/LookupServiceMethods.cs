using System.Collections.Generic;
using System.Linq;
using powerGateServer.SDK;
using SapServices.Database;
using SapServices.Services.MaterialService.Entities;

namespace SapServices.Services.MaterialService
{
	public class LanguForLanguIsoCollection : LookupCollection<LanguForLanguIsoLookup>
	{
		public LanguForLanguIsoCollection(IEntityStores entityStores) 
			: base(entityStores)
		{
		}

		public override string Name
		{
			get { return "LanguForLanguIsoCollection"; }
		}
	}

	public class MaterialByPlantLookupCollection : LookupCollection<MaterialByPlantLookup>
	{
		public MaterialByPlantLookupCollection(IEntityStores entityStores) 
			: base(entityStores)
		{
		}

		IEnumerable<MaterialContext> MaterialStore
		{
			get
			{
				return EntityStores.ResolveFor<MaterialContext>();
			}
		} 

		public override IEnumerable<MaterialByPlantLookup> Query(IExpression<MaterialByPlantLookup> expression)
		{
			var lookups = new List<MaterialByPlantLookup>();
			foreach (var materialContext in MaterialStore)
				lookups.AddRange(CreateLookupsFor(materialContext));
			return lookups;
		}

		public override void Create(MaterialByPlantLookup entity)
		{
			Throw(entity, "A {0} with key: [{1}] cannot be created! Create a valid MaterialContext instead!");
		}

		public override void Delete(MaterialByPlantLookup entity)
		{
			Throw(entity, "A {0} with key: [{1}] cannot be deleted! Delete the MaterialContext instead!");
		}

		public override void Update(MaterialByPlantLookup entity)
		{
			Throw(entity, "A {0} with key: [{1}] cannot be updated! Update the MaterialContext instead!");
		}

		IEnumerable<MaterialByPlantLookup> CreateLookupsFor(MaterialContext materialContext)
		{
			return materialContext.Description
				.Select(description => new MaterialByPlantLookup
				{
					Description = description.MatlDesc, 
					Plant = materialContext.Plant, 
					Langu = description.Langu, 
					Material = materialContext.Material
				});
		}
	}
}
