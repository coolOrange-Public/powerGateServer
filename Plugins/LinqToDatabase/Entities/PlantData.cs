using System;
using System.Data.Services.Common;

namespace UserServices.LinqToDatabase.Entities
{
	[DataServiceKey("MaterialID", "PlantID")]
    public class PlantData
    {
        public DateTime Pvallidfrom { get; set; }
        public string PurStatus { get; set; }
        public string PurGroup { get; set; }
        public string MaterialID { get; set; }
        public string PlantID { get; set; }
        public string Availcheck { get; set; }
    }

//	CREATE TABLE [Plant Datas] (
//Pvallidfrom Date,
//MaterialID nvarchar,
//PlantID nvarchar,
//PurStatus nvarchar,
//PurGroup nvarchar,
//Availcheck nvarchar,
//PRIMARY KEY (MaterialID,PlantID)
//);
}
