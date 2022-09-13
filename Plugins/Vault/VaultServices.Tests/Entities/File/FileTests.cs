using System;
using NUnit.Framework;
using Vault = Autodesk.Connectivity.WebServices;

namespace VaultServices.Tests.Entities.File
{
	[TestFixture]
	public class FileTests : ContainerBasedTests
	{
		[Test]
		public void Constructor_PassingVaultFile_Sets_ClassProperties()
		{
			var vaultFile = new Vault.File
			{
				Id = 666,
				Name = "Spongebob.iam",
				CreateDate = new DateTime(1999, 5, 1),
				CreateUserName = "Stephen Hillenburg"
			};

			var file = new VaultServices.Entities.File.File(vaultFile);
			Assert.AreEqual(file.Id, 666);
			Assert.AreEqual(file.Name, "Spongebob.iam");
			Assert.AreEqual(file.CreateDate, new DateTime(1999, 5, 1));
			Assert.AreEqual(file.CreateUser, "Stephen Hillenburg");
			Assert.AreEqual(file.Type, "File");
		}
	}
}