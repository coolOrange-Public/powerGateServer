using System.Linq;
using NUnit.Framework;
using powerGateServer.Core.WcfFramework.Expressions;
using VaultServices.Entities.Base;
using VaultServices.Entities.Folder;
using VaultServices.Entities.Link.FindStrategies;
using VaultServices.Tests.Entities.Base;

namespace VaultServices.Tests.Entities.Link.FindStrategies
{
    [TestFixture]
	public class LinkExpressionParserForFileTests : LinkExpressionParserTests<VaultServices.Entities.File.File>
    {
    }

    [TestFixture]
	public class LinkExpressionParserForFolderTests : LinkExpressionParserTests<Folder>
    {
    }

	public abstract class LinkExpressionParserTests<T> : ExpressionParserTests<T> where T : IBaseObject
	{
		[Test]
		public void Parsing_where_Parent_id_is_set()
		{
			var expression = ExpressionFactory.CreateCallWhere<VaultServices.Entities.Link.Link>(l => l.ParentId == 666 );
			var propertyExpression = new RequestExpression<VaultServices.Entities.Link.Link>(expression);

			var expressionParser = Container.Resolve<LinkExpressionParser>();
			var expressionForEntity = expressionParser.ParseFor<T>(propertyExpression);

			Assert.IsTrue(expressionForEntity.Where.IsSet());
			Assert.AreEqual(1,expressionForEntity.Where.Count());
			Assert.AreEqual(EntityTypeArray + ".Where(e => (e.Id == 666))", expressionForEntity.Base.ToString());
		}

		[Test]
		public void Parsing_where_Child_id_And_ChildType_is_set()
		{
			var expression = ExpressionFactory.CreateCallWhere<VaultServices.Entities.Link.Link>(l => 
				l.ChildId == 999 && l.ChildType == "File");
			var propertyExpression = new RequestExpression<VaultServices.Entities.Link.Link>(expression);

			var expressionParser = Container.Resolve<LinkExpressionParser>();
			var expressionForEntity = expressionParser.ParseFor<T>(propertyExpression);

			Assert.IsTrue(expressionForEntity.Where.IsSet());
			Assert.AreEqual(2, expressionForEntity.Where.Count());
			Assert.AreEqual(EntityTypeArray + ".Where(e => ((e.Id == 999) AndAlso (e.Type == \"File\")))", expressionForEntity.Base.ToString());
		}

		[Test]
		public void Parsing_where_Parent_andOr_childs_are_set()
		{
			var expression = ExpressionFactory.CreateCallWhere<VaultServices.Entities.Link.Link>(l => l.ParentId == 666 || l.ChildId == 999 && l.ParentId == 555 || l.ChildType == "File");
			var propertyExpression = new RequestExpression<VaultServices.Entities.Link.Link>(expression);

			var expressionParser = Container.Resolve<LinkExpressionParser>();
			var expressionForEntity = expressionParser.ParseFor<T>(propertyExpression);

			Assert.IsTrue(expressionForEntity.Where.IsSet());
			Assert.AreEqual(EntityTypeArray + ".Where(e => (((e.Id == 666) OrElse ((e.Id == 999) AndAlso (e.Id == 555))) OrElse (e.Type == \"File\")))", expressionForEntity.Base.ToString());
		}

		[Test]
		public void Parsing_order_by_Parent_id_orders_the_result_by_id()
		{
			var expression = ExpressionFactory.CreateCallThanOrderBy<VaultServices.Entities.Link.Link, long>(
				ExpressionFactory.CreateCallOrderBy<VaultServices.Entities.Link.Link, string>(l => l.ParentType),
				l => l.ParentId);
			var propertyExpression = new RequestExpression<VaultServices.Entities.Link.Link>(expression);

			var expressionParser = Container.Resolve<LinkExpressionParser>();
			var expressionForEntity = expressionParser.ParseFor<T>(propertyExpression);

			Assert.IsTrue(expressionForEntity.OrderBy.IsSet());
			Assert.AreEqual(2, expressionForEntity.OrderBy.Count());
			Assert.AreEqual(EntityTypeArray + ".OrderBy(e => e.Id)", expressionForEntity.Base.ToString());
		}
	}
}