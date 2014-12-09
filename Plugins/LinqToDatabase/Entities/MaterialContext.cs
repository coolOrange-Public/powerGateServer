using System.Collections.Generic;
using System.Data.Services.Common;

namespace UserServices.LinqToDatabase.Entities
{
	[DataServiceKey("MaterialID")]
    public class MaterialContext
    {
		public string MaterialID { get; set; }
        public string ValuationType { get; set; }
        public string Plant { get; set; }
        public string ValuationArea { get; set; }
		public PlantData PlantData { get; set; }
		public ValuationData ValuationData { get; set; }
		public List<Description> Description { get; set; }
		public BasicData BasicData { get; set; }

        public MaterialContext()
        {
			Description = new List<Description>();
        }
    }

	//querry for SQLite database table
	//CREATE TABLE [Material Contexts] (
	//ValuationType nvarchar,
	//MaterialID nvarchar,
	//Plant nvarchar,
	//ValuationArea nvarchar,
	//PRIMARY KEY (ID)
	//);
}
