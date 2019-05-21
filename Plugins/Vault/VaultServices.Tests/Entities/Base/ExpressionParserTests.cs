using NUnit.Framework;
using powerGateServer.Core.Tests;
using VaultServices.Entities.Base;

namespace VaultServices.Tests.Entities.Base
{
	public abstract class ExpressionParserTests<T> : ContainerBasedTests where T : IBaseObject
	{
		protected readonly ExpressionFactory ExpressionFactory = new ExpressionFactory();

		protected string EntityTypeArray;

		[SetUp]
		public void Initialize()
		{
			var entityType = typeof(T);
			EntityTypeArray = entityType.FullName + "[]";
		}
	}
}