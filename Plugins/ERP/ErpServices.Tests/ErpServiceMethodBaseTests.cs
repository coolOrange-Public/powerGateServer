using System;
using System.Data.Services.Common;
using ErpServices.Database;
using ErpServices.Services;
using NSubstitute;
using NUnit.Framework;

namespace ErpServices.Tests
{
	[TestFixture]
	public class ErpServiceMethodBaseTests
	{
		[Test]
		public void ClientId_returns_empty_when_client_is_not_authenticated()
		{
			var requestClientId = Substitute.For<Func<string>>();
			requestClientId.Invoke().ReturnsForAnyArgs("");
			
			var erpServiceBase = new TestsErpServiceMethodBaseImpl();
			erpServiceBase.GetCurrentClientId = requestClientId;
			var clientId = erpServiceBase.GetErpServiceMethodBaseClientId();

			Assert.AreEqual(string.Empty, clientId);
		}

		[Test]
		public void ClientId_returns_username_of_authenticated_client()
		{
			var requestClientId = Substitute.For<Func<string>>();
			requestClientId.Invoke().ReturnsForAnyArgs("hahahahahHAHAHAHA");
			
			var erpServiceBase = new TestsErpServiceMethodBaseImpl();
			erpServiceBase.GetCurrentClientId = requestClientId;
			var clientId = erpServiceBase.GetErpServiceMethodBaseClientId();

			Assert.AreEqual("hahahahahHAHAHAHA", clientId);
		}

		[DataServiceKey("Id")]
		class TestEntity
		{
			public string Name { get; set; }
			public int Id { get; set; }
		}

		class TestsErpServiceMethodBaseImpl : ErpServiceMethodBase<TestEntity>
		{
			public TestsErpServiceMethodBaseImpl() : base(Substitute.For<IEntityStores>())
			{
			}

			public string GetErpServiceMethodBaseClientId()
			{
				return ClientId;
			}
		}
	}
}