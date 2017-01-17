using IQToolkit.Data;
using LinqToDb.Entities;
using LinqToDb.Internal;

namespace LinqToDb
{
	public class Files : EntityProviderServiceMethod<FileContext>
	{
		public Files(DbEntityProvider entityProvider)
			: base(entityProvider)
		{
			using (var command = entityProvider.Connection.CreateCommand())
			{
				command.CommandText =
					"CREATE TABLE IF NOT EXISTS [File Contexts] (" +
						"FileID nvarchar," +
						"Name nvarchar," +
						"PRIMARY KEY (FileID)" +
					")";
				command.ExecuteNonQuery();
			}
		}
	}
}