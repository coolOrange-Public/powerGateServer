﻿using System;
using System.Linq;
using CatalogService.Entities;
using NSubstitute;
using NUnit.Framework;
using powerGateServer.SDK;

namespace CatalogService.Tests
{
	[TestFixture]
	public class ServiceCollectionTests : ContainerBaseTest
	{
		[Test]
		public void Query_returnsForeachRegisteredWebservice_an_equivalent_ServiceEntity()
		{
			var webSvc = Container.SubstituteFor<WebService>();
			webSvc.Info = Substitute.For<IWebServiceInfo>();
			webSvc.Info.Url.Returns(
				new Uri("http://localhost:8080/sap/opu/odata/SomeBundleName/TestSERVICE"));
			var webSvc2 = Substitute.For<WebService>();
			webSvc2.Info = Substitute.For<IWebServiceInfo>();
			webSvc2.Info.Url.Returns(
				new Uri("http://localhost:8080/sap/opu/odata/SomeBundleName/TestSERVICE"));
			var webSvc3 = Substitute.For<WebService>();
			webSvc3.Info = Substitute.For<IWebServiceInfo>();
			webSvc3.Info.Url.Returns(
				new Uri("http://localhost:8080/sap/opu/odata/SomeBundleName/TestSERVICE"));
			webSvc.Manager = Substitute.For<IWebServiceManager>();
			webSvc.Manager.Services.Returns(new[]
			{
				webSvc, webSvc2, webSvc3
			});
			var serviceCollection = Container.Resolve<ServiceCollection>();

			var services = serviceCollection.Query(Substitute.For<IExpression<Service>>());

			Assert.AreEqual(3,services.Count());
		}

		[Test]
		public void Query_ReturnsForTheAwebserviceType_a_new_Service_object_where_the_basicPropsAreSetupCorretlyWithWebServiceInfoData()
		{
			var webSvc = Container.SubstituteFor<WebService>();
			webSvc.Info = Substitute.For<IWebServiceInfo>();
			webSvc.Info.ServiceName.Returns("TestSERVICE");
			webSvc.Info.Path.Returns("SomeBundleName");
			webSvc.Info.ServiceName.Returns("TestSERVICE");
			webSvc.Info.Url.Returns(
				new Uri("http://localhost:8080/sap/opu/odata/SomeBundleName/TestSERVICE"));
			webSvc.Manager = Substitute.For<IWebServiceManager>();
			webSvc.Manager.Services.Returns(new[] { webSvc });
			var serviceCollection = Container.Resolve<ServiceCollection>();
			var services = serviceCollection.Query(Substitute.For<IExpression<Service>>());
			
			Assert.AreEqual(1, services.Count());
			var testService = services.First();
			Assert.AreEqual("TestSERVICE", testService.Title);
			Assert.AreEqual("TestSERVICE", testService.Description);
			Assert.AreEqual("/SomeBundleName/TestSERVICE_0001", testService.ID);
			Assert.AreEqual("http://localhost:8080/sap/opu/odata/SomeBundleName/TestSERVICE/$metadata", testService.MetadataUrl);
			Assert.AreEqual("/SomeBundleName/TestSERVICE", testService.TechnicalServiceName);
			Assert.AreEqual("http://localhost:8080/sap/opu/odata/SomeBundleName/TestSERVICE", testService.ServiceUrl);
		}

		[Test]
		public void Query_theAuthorOfTheService_isSetToTheCompanyOfTheAssembly()
		{
			var webSvc = new TestWebService
			{
				Manager = Substitute.For<IWebServiceManager>()
			};
			Container.Provide<WebService>(webSvc);
			webSvc.Manager.Services.Returns(new[] { webSvc });
			var serviceCollection = Container.Resolve<ServiceCollection>();
			var testService = serviceCollection.Query(Substitute.For<IExpression<Service>>()).FirstOrDefault();

			Assert.AreEqual("coolOrange", testService.Author);
		}

		[Test(Description = "This tests assumes that this assembly was builded today!")]
		public void Query_theUpdatedDateIsTheBuildDateOfTheWebServiceAssembly()
		{
			var webSvc = new TestWebService
			{
				Manager = Substitute.For<IWebServiceManager>()
			};
			Container.Provide<WebService>(webSvc);
			webSvc.Manager.Services.Returns(new[] { webSvc });
			var serviceCollection = Container.Resolve<ServiceCollection>();
			var testService = serviceCollection.Query(Substitute.For<IExpression<Service>>()).FirstOrDefault();

			Assert.AreEqual(DateTime.Now.ToShortDateString(),testService.UpdatedDate.ToShortDateString());
		}

		public class TestWebService : WebService
		{
			public TestWebService()
			{
				Info = Substitute.For<IWebServiceInfo>();
				Info.Url.Returns(new Uri("http://localhost:8080/sap/opu/odata/SomeBundleName/TestSERVICE"));
			}
		}
	}
}