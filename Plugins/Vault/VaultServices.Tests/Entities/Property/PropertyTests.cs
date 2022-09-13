using Autodesk.DataManagement.Client.Framework.Vault.Currency.Properties;
using powerVault.Cmdlets.Cmdlets.Vault.Facade;
using NSubstitute;
using NUnit.Framework;

namespace VaultServices.Tests.Entities.Property
{
	[TestFixture]
	public class PropertyTests
	{
		[Test]
		public void Constructor_PassingVaultPropertx_Sets_ClassProperties()
		{
			var file = new VaultServices.Entities.File.File { Id = 1234 };
			var vaultProperty = Substitute.For<IProperty>();
			vaultProperty.Definition.Returns(new PropertyDefinition { DataType = PropertyDefinition.PropertyDataType.String });
			vaultProperty.DisplayName.Returns("File Name");
			vaultProperty.Value.Returns("Schwammkopf.iam");

			var property = new VaultServices.Entities.Property.Property(file, vaultProperty);
			Assert.AreEqual(1234, property.ParentId);
			Assert.AreEqual("File", property.ParentType);
			Assert.AreEqual("File Name", property.Name);
			Assert.AreEqual("Schwammkopf.iam", property.Value);
			Assert.AreEqual("String", property.Type);
		}
	}
}