using System.Collections.Generic;

namespace UserServices.Database
{
	public interface IDatabase
	{
		Dictionary<string, IDbTable> Tables { get; } 

		IDbTable CreateTable(string name);
		void Commit();
	}

	public interface IDbTable
	{
		string Name { get; }
		string PrimaryKeyColumn { get; }
		Dictionary<int,IDbDataRow>  DataRows { get; }

		IDbDataRow CreateDataRow();

		void Commit();
	}

	public interface IDbDataRow
	{
		int PrimaryKey { get; }

		Dictionary<string,object> Data { get; }

		void Update(string field, object value);
		void Delete();
	}
}
