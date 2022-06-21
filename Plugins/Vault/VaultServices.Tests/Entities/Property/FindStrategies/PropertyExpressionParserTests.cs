using System;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using powerGateServer.Core.WcfFramework.Expressions;
using VaultServices.Entities.Base;
using VaultServices.Entities.Folder;
using VaultServices.Entities.Property.FindStrategies;
using VaultServices.Tests.Entities.Base;

namespace VaultServices.Tests.Entities.Property.FindStrategies
{
    [TestFixture]
    public class PropertyExpressionParserForFileTests : PropertyExpressionParserTests<VaultServices.Entities.File.File>
    {
    }

    [TestFixture]
    public class PropertyExpressionParserForFolderTests : PropertyExpressionParserTests<Folder>
    {
    }

	public abstract class PropertyExpressionParserTests<T> : ExpressionParserTests<T> where T : IBaseObject
	{
		[Test]
		public void Parsing_where_property_name_equals()
		{
			var expression = ExpressionFactory.CreateCallWhere<VaultServices.Entities.Property.Property>(p => p.Name == "Title");
			var propertyExpression = new RequestExpression<VaultServices.Entities.Property.Property>(expression);

			var expressionParser = Container.Resolve<PropertyExpressionParser>();
			var expressionForEntity = expressionParser.ParseFor<T>(propertyExpression);

			Assert.IsTrue(expressionForEntity.Where.IsSet());
			Assert.AreEqual(1,expressionForEntity.Where.Count());
			Assert.AreEqual(EntityTypeArray + ".Where(e => e.Properties.Any(entity => (entity.Name == \"Title\")))", expressionForEntity.Base.ToString());
		}

		[Test]
		public void Parsing_where_property_value_equals()
		{
			var expression = ExpressionFactory.CreateCallWhere<VaultServices.Entities.Property.Property>(p => p.Value == "Max&Barack4EverIn<3");
			var propertyExpression = new RequestExpression<VaultServices.Entities.Property.Property>(expression);

			var expressionParser = Container.Resolve<PropertyExpressionParser>();
			var expressionForEntity = expressionParser.ParseFor<T>(propertyExpression);

			Assert.IsTrue(expressionForEntity.Where.IsSet());
			Assert.AreEqual(1, expressionForEntity.Where.Count());
            Assert.AreEqual(EntityTypeArray + ".Where(e => e.Properties.Any(p => (p.Value == \"Max&Barack4EverIn<3\")))", expressionForEntity.Base.ToString());
		}

		[Test]
		public void Parsing_where_property_value_ends_with()
		{
			Expression<Func<VaultServices.Entities.Property.Property, bool>> predicate = p => ((p.Value == null) ? null : (Nullable<bool>)(p.Value.EndsWith("4EverIn<3"))) == (Nullable<bool>)false;
			var expression = ExpressionFactory.CreateCallWhere<VaultServices.Entities.Property.Property>(predicate);
			var propertyExpression = new RequestExpression<VaultServices.Entities.Property.Property>(expression);

			var expressionParser = Container.Resolve<PropertyExpressionParser>();
			var expressionForEntity = expressionParser.ParseFor<T>(propertyExpression);

			Assert.IsTrue(expressionForEntity.Where.IsSet());
			Assert.AreEqual(1, expressionForEntity.Where.Count());
            Assert.AreEqual(EntityTypeArray + ".Where(e => e.Properties.Any(p => (IIF((p.Value == null), null, Convert(p.Value.EndsWith(\"4EverIn<3\"))) == Convert(False))))", expressionForEntity.Base.ToString());
		}

		[Test]
		public void Parsing_where_property_value_starts_with()
		{
			Expression<Func<VaultServices.Entities.Property.Property, bool>> predicate = p => ((p.Value == null) ? null : (Nullable<bool>)(p.Value.StartsWith("Max&Barack"))) == (Nullable<bool>)false;
			var expression = ExpressionFactory.CreateCallWhere<VaultServices.Entities.Property.Property>(predicate);
			var propertyExpression = new RequestExpression<VaultServices.Entities.Property.Property>(expression);

			var expressionParser = Container.Resolve<PropertyExpressionParser>();
			var expressionForEntity = expressionParser.ParseFor<T>(propertyExpression);

			Assert.IsTrue(expressionForEntity.Where.IsSet());
			Assert.AreEqual(1, expressionForEntity.Where.Count());
            Assert.AreEqual(EntityTypeArray + ".Where(e => e.Properties.Any(p => (IIF((p.Value == null), null, Convert(p.Value.StartsWith(\"Max&Barack\"))) == Convert(False))))", expressionForEntity.Base.ToString());
		}

		[Test]
		public void Parsing_get_all_files_with_any_property()
		{
			var expression = ExpressionFactory.CreateCallTake<VaultServices.Entities.Property.Property>(1000);
			var propertyExpression = new RequestExpression<VaultServices.Entities.Property.Property>(expression);

			var expressionParser = Container.Resolve<PropertyExpressionParser>();
			var expressionForEntity = expressionParser.ParseFor<T>(propertyExpression);
            
            StringAssert.Contains(EntityTypeArray + ".Where(e => e.Properties.Any())", expressionForEntity.Base.ToString());
		}

	    [Test]
	    public void Test_ParseFor_PropertyExpressionWithTopCount_ReturnsEntityExpressionWithTake()
        {
            var expression = ExpressionFactory.CreateCallTake<VaultServices.Entities.Property.Property>(31);
            var propertyExpression = new RequestExpression<VaultServices.Entities.Property.Property>(expression);

            var expressionParser = Container.Resolve<PropertyExpressionParser>();
            var expressionForEntity = expressionParser.ParseFor<T>(propertyExpression);

            Assert.AreEqual(31, expressionForEntity.TopCount);
            Assert.AreEqual(EntityTypeArray + ".Where(e => e.Properties.Any()).Take(31)", expressionForEntity.Base.ToString());
	    }

	    [Test]
	    public void Test_ParseFor_PropertyExpressionWithSkipCount_ReturnsEntityExpressionWithAnyAndIgnoresSkip()
        {
            var expression = ExpressionFactory.CreateCallSkip<VaultServices.Entities.Property.Property>(69);
            var propertyExpression = new RequestExpression<VaultServices.Entities.Property.Property>(expression);

            var expressionParser = Container.Resolve<PropertyExpressionParser>();
            var expressionForEntity = expressionParser.ParseFor<T>(propertyExpression);

            Assert.AreEqual(EntityTypeArray + ".Where(e => e.Properties.Any())", expressionForEntity.Base.ToString());
	    }

	    [Test]
	    public void Test_ParseFor_PropertyExpressionWithOrderBy_ReturnsEntityExpressionWithAnyAndIgnoresOrderBy()
        {
            var expression = ExpressionFactory.CreateCallOrderBy<VaultServices.Entities.Property.Property>(property => property);
            var propertyExpression = new RequestExpression<VaultServices.Entities.Property.Property>(expression);

            var expressionParser = Container.Resolve<PropertyExpressionParser>();
            var expressionForEntity = expressionParser.ParseFor<T>(propertyExpression);

            Assert.AreEqual(EntityTypeArray + ".Where(e => e.Properties.Any())", expressionForEntity.Base.ToString());
	    }
	}
}