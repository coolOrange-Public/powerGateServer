using System.Linq;
using powerVault.Cmdlets.Cmdlets.Vault;
using powerVault.Cmdlets.Cmdlets.Vault.Facade;
using coolOrange.VaultServices.Vault;
using NSubstitute;
using NUnit.Framework;
using VaultServices.Entities.File;
using VaultServices.Entities.Link;
using Vault = Autodesk.Connectivity.WebServices;
using VDF = Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;


namespace VaultServices.Tests.Entities.File
{
	[TestFixture]
	public class FileAssociationServiceTests : ContainerBasedTests
	{
		[Test]
		public void GetChildren_uses_the_assocService_to_retrieve_the_file_attachmens_and_creates_an_AttachmentLink()
		{
			var assoc = Substitute.For<IAssociation<Vault.File, Vault.File>>();
			assoc.Children.Returns(new[] { new Vault.File { Name = "SpongeBob.iam", Id = 9000 } });
			var assocService = Container.SubstituteFor<IAssociationService>();
			assocService.GetFileAssociations(Arg.Is<Vault.File>(f => f.Id == 88), ReadAssocsTyp.Attachments).Returns(assoc);
			var file = new VaultServices.Entities.File.File { Id = 88 };
			var fileAssocService = Container.Resolve<FileAssociationService>();

			var link = fileAssocService.GetChildren(file).First();
			Assert.AreEqual(88, link.ParentId);
			Assert.AreEqual("File", link.ParentType);
			Assert.AreEqual(9000, link.ChildId);
			Assert.AreEqual("File", link.ChildType);
			Assert.IsInstanceOf<Attachment>(link);
		}

		[Test]
		public void GetChildren_uses_the_assocService_to_retrieve_the_file_children_and_creates_a_DependencyLink()
		{
			var assoc = Substitute.For<IAssociation<Vault.File, Vault.File>>();
			assoc.Children.Returns(new[] { new Vault.File { Name = "Patrick Star.iam", Id = 99 } });
			var assocService = Container.SubstituteFor<IAssociationService>();
			assocService.GetFileAssociations(Arg.Is<Vault.File>(f => f.Id == 88), ReadAssocsTyp.Dependencies).Returns(assoc);
			var file = new VaultServices.Entities.File.File { Id = 88 };
			var fileAssocService = Container.Resolve<FileAssociationService>();

			var link = fileAssocService.GetChildren(file).First();
			Assert.AreEqual(88, link.ParentId);
			Assert.AreEqual("File", link.ParentType);
			Assert.AreEqual(99, link.ChildId);
			Assert.AreEqual("File", link.ChildType);
			Assert.IsInstanceOf<Dependency>(link);
		}

		[Test]
		public void GetChildren_uses_the_linkManager_to_retrieve_the_shorctus_and_creates_a_ShortcutLink()
		{
			var testEntity = CreateTestVDFLink();
			var vaultService = Container.SubstituteFor<IVaultServices>();
			vaultService.GetLinkedChildren(Arg.Any<VDF.IEntity>(), null).Returns(new[] { testEntity });
			var file = new VaultServices.Entities.File.File { Id = 88 };
			var fileAssocService = Container.Resolve<FileAssociationService>();

			var link = fileAssocService.GetChildren(file).First();
			Assert.AreEqual(88, link.ParentId);
			Assert.AreEqual("File", link.ParentType);
			Assert.AreEqual(12345, link.ChildId);
			Assert.AreEqual("File", link.ChildType);
			Assert.IsInstanceOf<Shortcut>(link);
		}

		[Test, Description("This test is probably going to be replaced or changed, since powerVault should be extended for folders")]
		public void GetParents_uses_the_DocumentService_to_retrieve_the_parent_Folder_and_creates_a_DependencyLink()
		{
			var vaultServices = Container.SubstituteFor<IVaultServices>();
			vaultServices.GetFolderById(123).Returns(new Vault.Folder { Id = 123, Name = "I`m a monkey" });
			vaultServices.GetFileById(666).Returns(new Vault.File { Id = 666 , FolderId = 123});
			var file = new VaultServices.Entities.File.File { Id = 666 };
			var fileAssocService = Container.Resolve<FileAssociationService>();

			var link = fileAssocService.GetParents(file).First();
			Assert.AreEqual(123, link.ParentId);
			Assert.AreEqual("Folder", link.ParentType);
			Assert.AreEqual(666, link.ChildId);
			Assert.AreEqual("File", link.ChildType);
			Assert.IsInstanceOf<Dependency>(link);
		}

		private VDF.IEntity CreateTestVDFLink()
		{
			var entityClass = new VDF.EntityClass(VDF.EntityClassIds.Files, string.Empty, string.Empty, false, false, false,
				false, false,
				false, false, false, false, false);
			var parent = Substitute.For<VDF.IEntity>();
			parent.EntityClass.Returns(entityClass);
			parent.EntityIterationId.Returns(88);

			var linkInfo = new VDF.Link(parent, 1, "Number", 12345, VDF.EntityClassIds.Files);
			var entity = Substitute.For<VDF.IEntity>();
			entity.EntityIterationId.Returns(12345);
			entity.LinkInfo.Returns(linkInfo);
			return entity;
		}
	}
}