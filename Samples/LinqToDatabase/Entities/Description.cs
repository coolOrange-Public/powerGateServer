using System.Data.Services.Common;

namespace UserServices.LinqToDatabase.Entities
{
	[DataServiceKey("MaterialID", "Langu")]
    [DataServiceEntity]
    public class Description
    {
		public string MaterialID { get; set; }
        public string MatlDesc { get; set; }
        public string LanguIso { get; set; }
        public string Langu { get; set; }
    }

}
