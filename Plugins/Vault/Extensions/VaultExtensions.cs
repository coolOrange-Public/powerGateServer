using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using VaultServices.Entities.File;

namespace VaultServices.Extensions
{
	public static class VaultExtensions
	{
		public static string ToEntityType(this string entityClassId)
		{
			if (entityClassId == EntityClassIds.Files)
				return typeof (File).Name;
			if (entityClassId == EntityClassIds.Folder)
				return typeof(Folder).Name;
			if (entityClassId == EntityClassIds.Link)
				return typeof(Link).Name;
			return string.Empty;
		}
	}
}