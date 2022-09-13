using AutofacContrib.NSubstitute;
using NUnit.Framework;

namespace VaultServices.Tests
{
	public class ContainerBasedTests
	{
		internal AutoSubstitute Container;

		[SetUp]
		public virtual void SetUpContainer()
		{
			Container = new AutoSubstitute();
		}
	}
}
