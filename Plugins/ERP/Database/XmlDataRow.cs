using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ErpServices.Database
{
	public class XmlDataRow : IDbDataRow
	{
		protected readonly DataView DataView;
		protected readonly DataRow DataRow;
		private readonly XmlDbTable _table;
		public int PrimaryKey { get; private set; }

		public XmlDataRow(XmlDbTable table,DataRow dataRow)
		{
			_table = table;
			DataView = table.DataSet.Tables[0].DefaultView;
			DataRow = dataRow;

			PrimaryKey = (int)DataRow[_table.PrimaryKeyColumn];
		}

		public Dictionary<string, object> Data
		{
			get
			{
				var columns = DataRow.Table.Columns.Cast<DataColumn>();
				return columns.ToDictionary(c => c.ColumnName, c =>
				{
					var dataValue = DataRow[c.ColumnName];
					if (dataValue is DBNull)
						return null;
					return dataValue;
				});
			}
		}

		public void Update(string field, object value)
		{
			if (DataView.Table.Columns.Cast<DataColumn>().All(c => c.ColumnName != field))
				DataView.Table.Columns.Add(new DataColumn(field,typeof(string)));
			DataRow[field] = value == null ? null : Convert.ToString(value, CultureInfo.InvariantCulture);
		}

		public void Delete()
		{
			DataView.RowFilter = string.Format("{0}='{1}'",_table.PrimaryKeyColumn,PrimaryKey);
			DataView.Sort = _table.PrimaryKeyColumn;
			DataView.Delete(0);
			DataView.RowFilter = "";
			_table.DataRows.Remove(PrimaryKey);
		}
	}

	public class NewXmlDataRow : XmlDataRow
	{
		public NewXmlDataRow(XmlDbTable table)
			: base(table, table.DataSet.Tables[0].NewRow())
		{
			DataView.Table.Rows.Add(DataRow);
		}

	}
}