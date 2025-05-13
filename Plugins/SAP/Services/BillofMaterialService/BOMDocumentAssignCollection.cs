using SapServices.Database;
using SapServices.Services.BillofMaterialService.Entities;

namespace SapServices.Services.BillofMaterialService
{
    public class BOMDocumentAssignCollection : BomNavigationPropertyCollectionEntitySet<BillOfMaterialDocumentAssign>
    {
        public BOMDocumentAssignCollection(IEntityStores entityStores) : base(entityStores)
        {
        }

        public override string Name
        {
            get { return "BOMDocumentAssignCollection"; }
        }

	    public override void Create(BillOfMaterialDocumentAssign entity)
	    {
			Throw(entity, "A {0} with key: [{1}] cannot be created!");
	    }

	    public override void Delete(BillOfMaterialDocumentAssign entity)
	    {
			Throw(entity, "A {0} with key: [{1}] cannot be deleted!");
	    }

	    public override void Update(BillOfMaterialDocumentAssign entity)
	    {
			Throw(entity, "A {0} with key: [{1}] cannot be updated!");
	    }
    }
}