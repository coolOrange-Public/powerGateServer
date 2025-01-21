using System.Collections.Generic;
using System.Linq;

namespace SapServices.Tests
{
	public class TestData
	{
		public static string GetDefaultXmlContent()
		{
			return GetDefaultXmlContent(Enumerable.Empty<string>());
		}

		public static string GetDefaultXmlContent(IEnumerable<string> entities)
		{
			return string.Format("<TestTable>" +
				   "<xs:schema id=\"TestTable\" xmlns=\"\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">" +
				   " <xs:element name=\"TestTable\" msdata:IsDataSet=\"true\" msdata:UseCurrentLocale=\"true\">" +
				   "   <xs:complexType>" +
				   "     <xs:choice minOccurs=\"0\" maxOccurs=\"unbounded\">" +
				   "       <xs:element name=\"Test\">" +
				   "         <xs:complexType>" +
				   "           <xs:sequence>" +
				   "             <xs:element name=\"EntityID\" msdata:AutoIncrement=\"true\" type=\"xs:int\" minOccurs=\"0\" />" +
				   "           </xs:sequence>" +
				   "         </xs:complexType>" +
				   "       </xs:element>" +
				   "     </xs:choice>" +
				   "   </xs:complexType>" +
				   " </xs:element>" +
				   "</xs:schema>" +
				   "{0}"+
				   "</TestTable>",
				   string.Join("",entities.Select(e=>"<Test>"+e+"</Test>"))
				   );
		}
	}
}
