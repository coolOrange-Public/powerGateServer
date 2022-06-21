using System;
using System.Collections.Generic;
using VaultServices.Entities;
using VaultServices.Entities.Base;
using VaultServices.Entities.File;

namespace VaultServices
{
	public class PropertyNameSubstitution<T> where T : IBaseObject
	{
		private readonly IDictionary<Type, IDictionary<string, string>> _substitions =
			new Dictionary<Type, IDictionary<string, string>>();

		public PropertyNameSubstitution()
		{
			_substitions.Add(GetFileSubstitions());
		}

		public string ToVault(string name)
		{
			if (!_substitions.ContainsKey(typeof (T)))
				throw new NotSupportedException(string.Format("Type '{0}' is not supported!", typeof (T)));
			if (!_substitions[typeof(T)].ContainsKey(name))
				throw new ArgumentException("Argument is invalid!", name);
			return _substitions[typeof (T)][name];
		}

		private KeyValuePair<Type, IDictionary<string, string>> GetFileSubstitions()
		{
			return new KeyValuePair<Type, IDictionary<string, string>>(typeof (File),
				new Dictionary<string, string>
				{
					{"Name", "Name"},
					{"CreateUser", "CreateUserName"},
					{"CreateDate", "CreationDate"}
				});
		}
	}
}