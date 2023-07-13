using System;
using System.Collections.Generic;
using System.Linq;
using ErpServices.Converters;
using powerGateServer.SDK.Helpers;

namespace ErpServices.Database
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

	public class EntityStore<T> : Dictionary<T, IDbDataRow>, IEntityStore<T>
	{
		readonly IDatabase _database;
		readonly string _clientId;

		public IEntityDbConverter<T> Converter { get; private set; }

		public EntityStore(IDatabase database, IEntityDbConverter<T> converter, string clientId)
		{
			_database = database;
			Converter = converter;
			_clientId = clientId;
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
			var e = LoadEntityByDataServiceKeys(entity);
			if (!ContainsKey(LoadEntityByDataServiceKeys(e)))
				return;
			using (Transaction)
			{
				Converter.Remove(this[e]);
				Remove(e);
			}
		}

		public IDbTable Table
		{
			get
			{
				var table = _database.GetTable(TableFileName);
				return table ?? _database.CreateTable(EntityType, TableFileName);
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
		
		T LoadEntityByDataServiceKeys(T entity)
		{
			var entityKeys = entity.GetDataServiceKeys();
			return Keys.FirstOrDefault(d => entityKeys.SequenceEqual(d.GetDataServiceKeys()));
		}

		string EntityType
		{
			get { return typeof(T).Name; }
		}

		string TableFileName
		{
			get { return (string.IsNullOrEmpty(_clientId) ? string.Empty : _clientId + "_") + EntityType;}
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return Keys.GetEnumerator();
		}

		IDisposable Transaction
		{
			get { return new CommitTransaction(this); }
		}
		class CommitTransaction : IDisposable
		{
			readonly EntityStore<T> _entityStore;

			public CommitTransaction(EntityStore<T> entityStore)
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
