using System;
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
		public void Query_returns_for_each_registered_webservice_an_equivalent_Service_entity()
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
		public void Query_returns_for_the_webservice_type_a_new_Service_object_where_the_basic_Props_are_setup_correctly_with_WebServiceInfoData()
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
		public void Query_returns_ServiceUrl_and_MetadataUrl_with_same_Hostname_as_in_request()
		{
			var webSvc = Container.SubstituteFor<WebService>();
			webSvc.Info = Substitute.For<IWebServiceInfo>();
			webSvc.Info.Url.Returns(new Uri("http://localhost:8080/sap/opu/odata/SomeBundleName/TestSERVICE"));
			webSvc.Manager = Substitute.For<IWebServiceManager>();
			webSvc.Manager.Services.Returns(new[] { webSvc });
			var requestUri = new Uri("http://demo.volle.dor.powerGate.online:8080/PGS/CatalogService/ServiceCollection");
			Service.GetRequestUri = () => requestUri;

			var serviceCollection = Container.Resolve<ServiceCollection>();
			var services = serviceCollection.Query(Substitute.For<IExpression<Service>>());
			
			Assert.AreEqual(1, services.Count());
			var testService = services.First();
			Assert.AreEqual("http://demo.volle.dor.powergate.online:8080/sap/opu/odata/SomeBundleName/TestSERVICE", testService.ServiceUrl);
			Assert.AreEqual("http://demo.volle.dor.powergate.online:8080/sap/opu/odata/SomeBundleName/TestSERVICE/$metadata", testService.MetadataUrl);
		}

		[Test]
		public void Query_returns_Company_of_assembly_as_Author_of_service()
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

		[Test(Description = "This tests assumes that this assembly was built today!")]
		public void Query_return_buildDate_of_WebServiceAssembly_as_UpdatedDate_of_service()
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