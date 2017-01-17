using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ErpServices.Database;

namespace ErpServices.Converters
{
	public interface IEntityDbConverter<T> : ITypeConverter<T,IDictionary<string, object>>
	{
		IEnumerable<PropertyInfo> SpecialTypeProperties { get; }
		void Remove(IDbDataRow data);
	}

	public class ReflectionEntityDbConverter<T> : IEntityDbConverter<T>
	{
		public virtual T ConvertFrom(IDictionary<string, object> data)
		{
			var entity = Activator.CreateInstance<T>();
			foreach (var prop in PrimitivePropertyInfos.ToDictionary(p => p, p => data[p.Name]))
					prop.Key.SetValue(entity, GetValue(prop), null);
				
			return entity;
		}

		public virtual IDictionary<string, object> ConvertTo(T entity)
		{
			return PrimitivePropertyInfos.ToDictionary(
				propertyInfo => propertyInfo.Name,
				propertyInfo => propertyInfo.GetValue(entity, null));
		}


		public virtual void Remove(IDbDataRow data)
		{
			data.Delete();
		}

		protected virtual IEnumerable<PropertyInfo> PrimitivePropertyInfos
		{
			get
			{
				return AllPropertyInfos.Where(p => 
					p.PropertyType.IsPrimitive || 
					p.PropertyType == typeof(string) ||
					p.PropertyType == typeof(decimal) ||
					p.PropertyType == typeof(DateTime));
			}
		}

		protected IEnumerable<PropertyInfo> AllPropertyInfos
		{
			get
			{
				return typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
			}
		}

		public IEnumerable<PropertyInfo> SpecialTypeProperties
		{
			get
			{
				return AllPropertyInfos.Except(PrimitivePropertyInfos);
			}
		}

		object GetValue(KeyValuePair<PropertyInfo, object> prop)
		{
			return Convert.ChangeType(prop.Value, prop.Key.PropertyType, CultureInfo.InvariantCulture);
		}
	}
}