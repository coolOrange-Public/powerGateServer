using AutofacContrib.NSubstitute;
using NUnit.Framework;

namespace SapServices.Tests
{
	public class ContainerBaseTest
	{
		internal AutoSubstitute Container;

		[SetUp]
		public virtual void SetUpContainer()
		{
			Container =  new AutoSubstitute();
		}
	}
}
