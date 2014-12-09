using System.Data.Services.Common;

namespace UserServices.LinqToDatabase.Entities
{
	[DataServiceKey("MaterialID")]
    public class BasicData
    {
        public string BaseUomIso { get; set; }
        public string BaseUom { get; set; }
        public string MatlGroup { get; set; }
        public string MatlType { get; set; }
        public string IndSector { get; set; }
		public string MaterialID { get; set; }
    }
}
