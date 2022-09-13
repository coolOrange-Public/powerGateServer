using NUnit.Framework;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using VaultServices.Extensions;

namespace VaultServices.Tests
{
	[TestFixture]
	public class VaultExtensionsTests
	{
		[Test]
		[TestCase(EntityClassIds.Files, Result = "File")]
		[TestCase(EntityClassIds.Link, Result = "Link")]
		[TestCase(EntityClassIds.Folder, Result = "Folder")]
		public string ToEntityType_converts_entityClassIds_ToEntityType(string classIds)
		{
			return classIds.ToEntityType();
		}

		[Test]
		public void ToEntityType_for_unknown_types_returns_empty()
		{
			var classId = EntityClassIds.ForumMessage;
			Assert.IsEmpty(classId.ToEntityType());
		} 
	}
}