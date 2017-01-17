using System;
using System.Collections.Generic;
using System.Linq;
using SapServices.Converters;

namespace SapServices.Database
{
	public interface IEntityStore<T> : IEnumerable<T>
	{
		IDbTable Table { get; }
		IEntityDbConverter<T> Converter { get; }
		T Find(int primaryKey);
		void Reload();
		int Insert(T entity);
		void Delete(T entity);
	}

	public class EntityStore<T> : Dictionary<T,IDbDataRow>, IEntityStore<T>
	{
		private readonly IDatabase _database;

		public IEntityDbConverter<T> Converter { get; private set; }

		public EntityStore(IDatabase database, IEntityDbConverter<T> converter)
		{
			_database = database;
			Converter = converter;
		}

		public void Reload()
		{
			Clear();
			if (Table.DataRows == null) 
				return;
			using (Transaction)
				foreach (var dataRow in Table.DataRows.Values)
					Add(LoadEntity(dataRow.PrimaryKey), dataRow);
		}

		public int Insert(T entity)
		{
			using (Transaction)
			{
				var row = Table.CreateDataRow();
				foreach (var property in Converter.ConvertTo(entity))
					row.Update(property.Key, property.Value);
				Add(entity, row);
				return row.PrimaryKey;
			}
		}

		public void Delete(T entity)
		{
			if (!ContainsKey(entity))
				return;
			using (Transaction)
			{
				Converter.Remove(this[entity]);
				Remove(entity);
			}
		}

		public IDbTable Table
		{
			get
			{
				return _database.Tables.ContainsKey(EntityType)
						? _database.Tables[EntityType]
						: _database.CreateTable(EntityType);
			}
		}

		public T Find(int primaryKey)
		{
			var dataRow = Table.DataRows[primaryKey];

			if(Values.Any(v=>v.PrimaryKey == primaryKey))
				return ((Dictionary<T, IDbDataRow>)this)
					.First(dic => dic.Value.PrimaryKey == dataRow.PrimaryKey).Key;
			return default(T);
		}

		T LoadEntity(int primaryKey)
		{
			var dataRow = Table.DataRows[primaryKey];
			return Converter.ConvertFrom(dataRow.Data);
		}

		string EntityType
		{
			get { return typeof (T).Name; }
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return Keys.GetEnumerator();
		}

		IDisposable Transaction
		{
			get { return new CommittTransaction(this); }
		}
		private class CommittTransaction : IDisposable
		{
			private readonly EntityStore<T> _entityStore;

			public CommittTransaction(EntityStore<T> entityStore)
			{
				_entityStore = entityStore;
			}

			public void Dispose()
			{
				_entityStore.Table.Commit();
			}
		}
	}
}
