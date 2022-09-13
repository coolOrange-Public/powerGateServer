using System.Linq;
using Autofac;
using NSubstitute;
using NUnit.Framework;
using powerGateServer.SDK;

namespace VaultServices.Tests
{
	[TestFixture]
	public class ContainerBasedServiceTests
	{
		[Test]
		public void Constructor_adds_all_registered_services_to_webService()
		{
			var testService = new TestService();
			CollectionAssert.AreEquivalent(new[] { typeof(string),typeof(int),typeof(double) },
				testService.Methods.Select(m=>m.EntityType));
		}


		private class TestService : ContainerBasedService
		{
			public TestService()
			{
				ContainerBuilder.RegisterInstance(Substitute.For<ServiceMethod<string>>()).As<IServiceMethod>();
				ContainerBuilder.RegisterInstance(Substitute.For<ServiceMethod<int>>()).As<IServiceMethod>();
				ContainerBuilder.RegisterInstance(Substitute.For<ServiceMethod<double>>()).As<IServiceMethod>();

				AddServiceMethods();
			}
		}
	}
}