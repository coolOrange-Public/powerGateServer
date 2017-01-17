using System.Data.Services.Common;

namespace LinqToDb.Entities
{
	[DataServiceKey("FileID")]
	public class FileContext
	{
		public string FileID { get; set; }
		public string Name { get; set; }
	}
}