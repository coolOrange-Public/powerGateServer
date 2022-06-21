using NSubstitute;
using NUnit.Framework;
using VaultServices.Entities.Link;
using VDF = Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using Vault = Autodesk.Connectivity.WebServices;

namespace VaultServices.Tests.Entities.Link
{
	[TestFixture]
	public class LinkTests 
	{
		[Test]
		public void Shortcut_passing_a_vault_link_creates_the_shortcut_properly()
		{
			var entityClass = new VDF.EntityClass(VDF.EntityClassIds.Folder, string.Empty, string.Empty, false, false, false, false, false, false, false, false, false, false);
			var parent = Substitute.For<VDF.IEntity>();
			parent.EntityClass.Returns(entityClass);
			parent.EntityIterationId.Returns(666);
			var vaultLink = new VDF.Link(parent, 1, "Number", 88, VDF.EntityClassIds.Files);
			var shortcut = new Shortcut(vaultLink);
			Assert.AreEqual("Shortcut", shortcut.Description);
			Assert.AreEqual(88, shortcut.ChildId);
			Assert.AreEqual("File", shortcut.ChildType);
			Assert.AreEqual(666, shortcut.ParentId);
			Assert.AreEqual("Folder", shortcut.ParentType);
		}

		[Test]
		public void Dependency_passing_a_File_as_child_creates_the_dependency_properly()
		{
			var file = new VaultServices.Entities.File.File{Id = 88};
			var vaultFile = new Vault.File {Id = 999};
			var dependency = new Dependency(file, vaultFile);
			Assert.AreEqual("Dependency", dependency.Description);
			Assert.AreEqual(88, dependency.ParentId);
			Assert.AreEqual("File", dependency.ParentType);
			Assert.AreEqual(999,dependency.ChildId);
			Assert.AreEqual("File", dependency.ChildType);
		}
	}
}