using System;
using NUnit.Framework;
using VaultServices.Entities;
using VaultServices.Entities.Base;
using VaultServices.Entities.File;

namespace VaultServices.Tests
{
	[TestFixture]
	public class PropertyNameSubstitutionTests : ContainerBasedTests
	{
		[Test]
		public void ToVault_calling_with_invalid_type_throws_NotSupportedException()
		{
			var propertyNameSubst = Container.Resolve<PropertyNameSubstitution<IBaseObject>>();
			Assert.Throws<NotSupportedException>(() => propertyNameSubst.ToVault("Vault"));
		}

		[Test]
		public void ToVault_passing_invalid_name_throws_ArgumentNotFoundException()
		{
			var propertyNameSubst = Container.Resolve<PropertyNameSubstitution<File>>();
			Assert.Throws<ArgumentException>(() => propertyNameSubst.ToVault("Vault"));
		}

		[Test]
		[TestCase("Name", Result = "Name")]
		[TestCase("CreateUser", Result = "CreateUserName")]
		[TestCase("CreateDate", Result = "CreationDate")]
		public string File_ToVault_substiutes_the_name_with_the_vault_property_name(string name)
		{
			var propertyNameSubst = Container.Resolve<PropertyNameSubstitution<File>>();
			return propertyNameSubst.ToVault(name);
		}
	}
}