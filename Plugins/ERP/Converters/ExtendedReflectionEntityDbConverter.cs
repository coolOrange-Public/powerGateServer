using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ErpServices.Database;
using ErpServices.Services;

namespace ErpServices.Converters
{
	public class ExtendedReflectionEntityDbConverter<T> : ReflectionEntityDbConverter<T>
	{
		readonly NumberListConverter _numberListConverter = new NumberListConverter();
		readonly IEntityStores _entityStores;
		internal Func<string> GetCurrentClientId = AuthenticatedUser.GetUsername;

		public ExtendedReflectionEntityDbConverter(IEntityStores entityStores)
		{
			_entityStores = entityStores;
		}

		internal string ClientId
		{
			get
			{
				return IsLookupEntity() ? string.Empty : GetCurrentClientId();
			}
		}
		
		public override T ConvertFrom(IDictionary<string, object> data)
		{
			var entity = base.ConvertFrom(data);

			foreach (var specialTypeProperty in SpecialTypeProperties)
			{
				object propValue = null;
				var referenceId = data[specialTypeProperty.Name];
				if (referenceId != null)
				{
					if (specialTypeProperty.PropertyType == typeof(byte[]))
						propValue = Convert.FromBase64String((string)referenceId);
					else if (specialTypeProperty.PropertyType.IsEnumerable())
					{
						var typeElement = specialTypeProperty.PropertyType.GetElementTypeFromEnumerable();

						var entities = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new[] { typeElement }));
						foreach (var refId in _numberListConverter.ConvertFrom(referenceId.ToString()))
							entities.Add(Find(typeElement, refId));

						propValue = entities.ToArray(typeElement);
					}
					else
						propValue = Find(specialTypeProperty.PropertyType, referenceId);
				}
				specialTypeProperty.SetValue(entity, propValue, null);
			}
			return entity;
		}

		public override IDictionary<string, object> ConvertTo(T entity)
		{
			var data = base.ConvertTo(entity);

			foreach (var specialTypeProperty in SpecialTypeProperties)
			{
				var propValue = specialTypeProperty.GetValue(entity, null);
				object referenceId = null;
				if (!propValue.IsDefaultValue(specialTypeProperty.PropertyType))
				{
					if (specialTypeProperty.PropertyType == typeof(byte[]))
						referenceId = Convert.ToBase64String((byte[]) propValue);
					else if (specialTypeProperty.PropertyType.IsEnumerable())
					{
						var typeElement = specialTypeProperty.PropertyType.GetElementTypeFromEnumerable();
						referenceId = _numberListConverter.ConvertTo(((IEnumerable<object>)specialTypeProperty.GetValue(entity, null)).Select(e => Insert(typeElement, e)));
					}
					else
						referenceId = Insert(specialTypeProperty.PropertyType, propValue);
				}
				data[specialTypeProperty.Name] = referenceId;
			}
			return data;
		}

		public override void Remove(IDbDataRow data)
		{
			var entity = Find(typeof (T), data.PrimaryKey);
			foreach (var specialTypeProperty in SpecialTypeProperties)
			{
				var propValue = specialTypeProperty.GetValue(entity, null);
				if (!propValue.IsDefaultValue(specialTypeProperty.PropertyType))
				{
					if (specialTypeProperty.PropertyType == typeof (byte[]))
					{}
					else if (specialTypeProperty.PropertyType.IsEnumerable())
					{
						var typeElement = specialTypeProperty.PropertyType.GetElementTypeFromEnumerable();
						foreach (var childEntity in (IEnumerable<object>)specialTypeProperty.GetValue(entity, null))
							Delete(typeElement, childEntity);
					}else
						Delete(specialTypeProperty.PropertyType, propValue);
				}
			}
			base.Remove(data);
		}

		int Insert(Type type, object entity)
		{
			var store = GetStoreFor(type);
			var insertEntity = store.GetType().GetMethod("Insert");
			return (int) insertEntity.Invoke(store, new[] { entity });
		}
		void Delete(Type type, object entity)
		{
			var store = GetStoreFor(type);
			var deleteEntity = store.GetType().GetMethod("Delete");
			deleteEntity.Invoke(store, new[] { entity });
		}
		object Find(Type type, object referenceId)
		{
			var store = GetStoreFor(type);
			var findEntity = store.GetType().GetMethod("Find");
			return findEntity.Invoke(store, new object[] { Convert.ToInt32(referenceId) });
		}

		object GetStoreFor(Type t)
		{
			var method = _entityStores.GetType().GetMethod("ResolveFor");
			var genericMethod = method.MakeGenericMethod(new[] { t });
			return genericMethod.Invoke(_entityStores, new object[] { ClientId });
		}
		
		bool IsLookupEntity()
		{
			return typeof(T).Name.Contains("Lookup");
		}
	}
} 