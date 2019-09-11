using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using powerGateServer.SDK;
using VaultServices.Entities.Base;
using VaultServices.Entities.Base.FindStrategies;
using VaultServices.Entities.Folder;
using VaultServices.Entities.Property.FindStrategies;

namespace VaultServices.Tests.Entities.Property.FindStrategies
{
    [TestFixture]
    public class FindPropertiesTests : ContainerBasedTests
    {
        [Test]
        public void Test_CanExecute_WithQFilesAndQFoldersNull_ExpressionNameEquals_ReturnsFalse()
        {
            var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Property.Property>>();
            whereToken.PropertyName.Returns("Id");
            whereToken.Operator.Returns(OperatorType.Equals);

            var expression = Substitute.For<IExpression<VaultServices.Entities.Property.Property>>();
            expression.Where.GetEnumerator().Returns(new List<IWhereToken<VaultServices.Entities.Property.Property>> { whereToken }.GetEnumerator());

            var findProperties = Container.Resolve<FindProperties>();
            Assert.IsFalse(findProperties.CanExecute(expression));
        }

        [Test]
        public void Test_CanExecute_WithQueryFilesCanExecuteReturnsTrueAndQFoldersNull_ReturnsTrue()
        {
			Container.SubstituteFor<IExpressionParser<VaultServices.Entities.Property.Property>>();

            var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
            queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);

            var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Property.Property>>();
            whereToken.PropertyName.Returns("Id");
            whereToken.Operator.Returns(OperatorType.Equals);

            var expression = Substitute.For<IExpression<VaultServices.Entities.Property.Property>>();
            expression.Where.GetEnumerator().Returns(new List<IWhereToken<VaultServices.Entities.Property.Property>> { whereToken }.GetEnumerator());

            var findProperties = Container.Resolve<FindProperties>();
            findProperties.QueryFiles = queryFiles;
            Assert.IsTrue(findProperties.CanExecute(expression));
        }

        [Test]
        public void Test_CanExecute_WithQFilesAndQFoldersCanExecuteReturningTrue_ReturnsTrue()
        {
            var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
            queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);

            var queryFolders = Substitute.For<IQueryOperation<Folder>>();
            queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);

            var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Property.Property>>();
            whereToken.PropertyName.Returns("Id");
            whereToken.Operator.Returns(OperatorType.Equals);

            var expression = Substitute.For<IExpression<VaultServices.Entities.Property.Property>>();
            expression.Where.GetEnumerator().Returns(new List<IWhereToken<VaultServices.Entities.Property.Property>> { whereToken }.GetEnumerator());

            var findProperties = Container.Resolve<FindProperties>();
            findProperties.QueryFiles = queryFiles;
            Assert.IsTrue(findProperties.CanExecute(expression));
        }

        [Test]
        public void Test_CanExecute_WithQFilesCanExecuteReturnsFalseAndQFoldersTrue_ReturnsTrue()
        {
			Container.SubstituteFor<IExpressionParser<VaultServices.Entities.Property.Property>>();

            var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
            queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(false);

            var queryFolders = Substitute.For<IQueryOperation<Folder>>();
            queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);

            var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Property.Property>>();
            whereToken.PropertyName.Returns("Id");
            whereToken.Operator.Returns(OperatorType.Equals);

            var expression = Substitute.For<IExpression<VaultServices.Entities.Property.Property>>();
            expression.Where.GetEnumerator().Returns(new List<IWhereToken<VaultServices.Entities.Property.Property>> { whereToken }.GetEnumerator());

            var findProperties = Container.Resolve<FindProperties>();
            findProperties.QueryFiles = queryFiles;
            findProperties.QueryFolders = queryFolders;
            Assert.IsTrue(findProperties.CanExecute(expression));
        }

        [Test]
        public void Test_CanExecute_WithQFilesAndQFoldersCanExecuteReturnsFalse_ReturnsFalse()
        {
            var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
            queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(false);

            var queryFolders = Substitute.For<IQueryOperation<Folder>>();
            queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(false);

            var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Property.Property>>();
            whereToken.PropertyName.Returns("Id");
            whereToken.Operator.Returns(OperatorType.Equals);

            var expression = Substitute.For<IExpression<VaultServices.Entities.Property.Property>>();
            expression.Where.GetEnumerator().Returns(new List<IWhereToken<VaultServices.Entities.Property.Property>> { whereToken }.GetEnumerator());

            var findProperties = Container.Resolve<FindProperties>();
            findProperties.QueryFiles = queryFiles;
            findProperties.QueryFolders = queryFolders;
            Assert.IsFalse(findProperties.CanExecute(expression));
            queryFolders.Received().CanExecute(Arg.Any<IExpression<Folder>>());
            queryFiles.Received().CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>());
        }

        [Test]
        public void Test_Execute_ExpressionWithName_ReturnsPropertiesFromFolderAndFile()
        {
			var expressionParser = Container.SubstituteFor<IExpressionParser<VaultServices.Entities.Property.Property>>();
            expressionParser.ParseFor<VaultServices.Entities.File.File>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<VaultServices.Entities.File.File>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));
            expressionParser.ParseFor<Folder>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<Folder>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));

            var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
            queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);
            queryFiles.Execute().Returns(new[]
            {
                new VaultServices.Entities.File.File { Id = 31, Properties = new[]{ new VaultServices.Entities.Property.Property{ Type = "File", Name = "Name", Value = "Pad Lock.iam"} }},
                new VaultServices.Entities.File.File { Id = 69, Properties = new[]{ new VaultServices.Entities.Property.Property{ Type = "File", Name = "Name", Value = "Pad Lock.iam"} }}
            });
            var queryFolders = Substitute.For<IQueryOperation<Folder>>();
            queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);
            queryFolders.Execute().Returns(new[] {
                new Folder { Id = 310, Properties = new[]{ new VaultServices.Entities.Property.Property{ Type = "Folder", Name = "Name", Value = "Pad Lock.iam"} }},
                new Folder { Id = 690, Properties = new[]{ new VaultServices.Entities.Property.Property{ Type = "Folder", Name = "Name", Value = "Pad Lock.iam"} }}
            });

            var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Property.Property>>();
            whereToken.PropertyName.Returns("Value");
            whereToken.Operator.Returns(OperatorType.Equals);
            whereToken.Value.Returns("Pad lock.iam");
            var whereToken2 = Substitute.For<IWhereToken<VaultServices.Entities.Property.Property>>();
            whereToken2.PropertyName.Returns("Name");
            whereToken2.Operator.Returns(OperatorType.Equals);
            whereToken2.Value.Returns("Name");
            var expression = Substitute.For<IExpression<VaultServices.Entities.Property.Property>>();
            expression.Where.GetEnumerator().Returns(info => new List<IWhereToken<VaultServices.Entities.Property.Property>> { whereToken, whereToken2 }.GetEnumerator());

            var findProperties = Container.Resolve<FindProperties>();
            findProperties.QueryFolders = queryFolders;
            findProperties.QueryFiles = queryFiles;

            Assert.IsTrue(findProperties.CanExecute(expression));
            var result = findProperties.Execute().ToList();
            Assert.AreEqual(4, result.Count());
            Assert.AreEqual(2, result.Count(property => property.Type == "File"));
            Assert.AreEqual(2, result.Count(property => property.Type == "Folder"));
            queryFiles.Received().Execute();
            queryFolders.Received().Execute();
        }


        [Test]
        public void Test_Execute_ExpressionWhereVersionEquals_ReturnsOnlyFromFile()
        {
			var expressionParser = Container.SubstituteFor<IExpressionParser<VaultServices.Entities.Property.Property>>();
            expressionParser.ParseFor<VaultServices.Entities.File.File>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<VaultServices.Entities.File.File>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));
            expressionParser.ParseFor<Folder>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<Folder>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));

            var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
            queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);
            queryFiles.Execute().Returns(new[]
            {
                new VaultServices.Entities.File.File { Id = 31, Properties = new[]{ new VaultServices.Entities.Property.Property{ Type = "File", Name = "Version", Value = "31"} }},
                new VaultServices.Entities.File.File { Id = 69, Properties = new[]{ new VaultServices.Entities.Property.Property{ ParentId = 69, Type = "File", Name = "Version", Value = "3"} }}
            });
            var queryFolders = Substitute.For<IQueryOperation<Folder>>();
            queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);
            queryFolders.Execute().Returns(new[] {
                new Folder { Id = 310, Properties = new[]{ new VaultServices.Entities.Property.Property{ Type = "Folder", Name = "Name", Value = "Lock"} }},
                new Folder { Id = 690, Properties = new[]{ new VaultServices.Entities.Property.Property{ Type = "Folder", Name = "Name", Value = "Pad"} }}
            });

            var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Property.Property>>();
            whereToken.PropertyName.Returns("Value");
            whereToken.Operator.Returns(OperatorType.Equals);
            whereToken.Value.Returns("3");
            var whereToken2 = Substitute.For<IWhereToken<VaultServices.Entities.Property.Property>>();
            whereToken2.PropertyName.Returns("Name");
            whereToken2.Operator.Returns(OperatorType.Equals);
            whereToken2.Value.Returns("Version");
            var expression = Substitute.For<IExpression<VaultServices.Entities.Property.Property>>();
            expression.Where.GetEnumerator().Returns(info => new List<IWhereToken<VaultServices.Entities.Property.Property>> { whereToken, whereToken2 }.GetEnumerator());

            var findProperties = Container.Resolve<FindProperties>();
            findProperties.QueryFolders = queryFolders;
            findProperties.QueryFiles = queryFiles;

            Assert.IsTrue(findProperties.CanExecute(expression));
            var result = findProperties.Execute().ToList();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(69, result.First().ParentId);
            queryFiles.Received().Execute();
            queryFolders.Received().Execute();
        }

        [Test]
        public void Test_Execute_ExpressionWhichNoneSupports_ReturnsEmptyList()
        {
			var expressionParser = Container.SubstituteFor<IExpressionParser<VaultServices.Entities.Property.Property>>();
            expressionParser.ParseFor<VaultServices.Entities.File.File>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<VaultServices.Entities.File.File>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));
            expressionParser.ParseFor<Folder>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<Folder>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));

            var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
            queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);
            queryFiles.Execute().Returns(new[]
            {
                new VaultServices.Entities.File.File { Id = 69, Properties = new[]{ new VaultServices.Entities.Property.Property{ ParentId = 69, Type = "File", Name = "Version", Value = "3"} }}
            });
            var queryFolders = Substitute.For<IQueryOperation<Folder>>();
            queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);
            queryFolders.Execute().Returns(new[] {
                new Folder { Id = 310, Properties = new[]{ new VaultServices.Entities.Property.Property{ Type = "Folder", Name = "Name", Value = "Lock"} }}
            });

            var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Property.Property>>();
            whereToken.PropertyName.Returns("Name");
            whereToken.Operator.Returns(OperatorType.Equals);
            whereToken.Value.Returns("NotExisting");
            var expression = Substitute.For<IExpression<VaultServices.Entities.Property.Property>>();
            expression.Where.GetEnumerator().Returns(info => new List<IWhereToken<VaultServices.Entities.Property.Property>> { whereToken }.GetEnumerator());

            var findProperties = Container.Resolve<FindProperties>();
            findProperties.QueryFolders = queryFolders;
            findProperties.QueryFiles = queryFiles;

            Assert.IsTrue(findProperties.CanExecute(expression));
            CollectionAssert.IsEmpty(findProperties.Execute());
            queryFiles.Received().Execute();
            queryFolders.Received().Execute();
        }

        [Test]
        public void Test_Execute_ExpressionWhereTypeEqualsFolder_ReturnsPropertiesFromOnlyFolder()
        {
			var expressionParser = Container.SubstituteFor<IExpressionParser<VaultServices.Entities.Property.Property>>();
            expressionParser.ParseFor<VaultServices.Entities.File.File>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<VaultServices.Entities.File.File>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));
            expressionParser.ParseFor<Folder>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<Folder>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));

            var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
            queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);
            queryFiles.Execute().Returns(new[]
            {
                new VaultServices.Entities.File.File { Id = 69, Properties = new[]{ new VaultServices.Entities.Property.Property{ ParentId = 69, Type = "File", Name = "Version", Value = "3"} }}
            });
            var queryFolders = Substitute.For<IQueryOperation<Folder>>();
            queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);
            queryFolders.Execute().Returns(new[] {
                new Folder { Id = 310, Properties = new[]{ new VaultServices.Entities.Property.Property{ Type = "Folder", Name = "Name", Value = "Lock"} }}
            });

            var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Property.Property>>();
            whereToken.PropertyName.Returns("Type");
            whereToken.Operator.Returns(OperatorType.Equals);
            whereToken.Value.Returns("Folder");
            var expression = Substitute.For<IExpression<VaultServices.Entities.Property.Property>>();
            expression.Where.GetEnumerator().Returns(info => new List<IWhereToken<VaultServices.Entities.Property.Property>> { whereToken }.GetEnumerator());

            var findProperties = Container.Resolve<FindProperties>();
            findProperties.QueryFolders = queryFolders;
            findProperties.QueryFiles = queryFiles;

            Assert.IsTrue(findProperties.CanExecute(expression));
            Assert.AreEqual("Lock", findProperties.Execute().First().Value);
            queryFiles.DidNotReceive().Execute();
            queryFolders.Received().Execute();
        }

        [Test]
        public void Test_Execute_ExpressionTop3_ReturnsOnlyFiles()
        {
			var expressionParser = Container.SubstituteFor<IExpressionParser<VaultServices.Entities.Property.Property>>();
            expressionParser.ParseFor<VaultServices.Entities.File.File>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<VaultServices.Entities.File.File>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));
            expressionParser.ParseFor<Folder>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<Folder>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));

            var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
            queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);
            queryFiles.Execute().Returns(Enumerable.Repeat(new VaultServices.Entities.File.File { Id = 69, Properties = new[] { new VaultServices.Entities.Property.Property { ParentId = 69, Type = "File", Name = "Version", Value = "3" } } }, 5));
            var queryFolders = Substitute.For<IQueryOperation<Folder>>();
            queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);
            queryFolders.Execute().Returns(new[] {
                new Folder { Id = 310, Properties = new[]{ new VaultServices.Entities.Property.Property{ Type = "Folder", Name = "Name", Value = "Lock"} }}
            });

            var expression = Substitute.For<IExpression<VaultServices.Entities.Property.Property>>();
            expression.TopCount.Returns(3);

            var findProperties = Container.Resolve<FindProperties>();
            findProperties.QueryFolders = queryFolders;
            findProperties.QueryFiles = queryFiles;

            Assert.IsTrue(findProperties.CanExecute(expression));
            Assert.AreEqual(3, findProperties.Execute().Count());
            queryFiles.Received().Execute();
            queryFolders.DidNotReceive().Execute();
        }

        [Test]
        public void Test_Execute_ExpressionTop10_ReturnsFilesAndFolderBecauseOnly6FilesExisting()
        {
			var expressionParser = Container.SubstituteFor<IExpressionParser<VaultServices.Entities.Property.Property>>();
            expressionParser.ParseFor<VaultServices.Entities.File.File>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<VaultServices.Entities.File.File>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));
            expressionParser.ParseFor<Folder>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<Folder>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));

            var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
            queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);
            queryFiles.Execute().Returns(Enumerable.Repeat(new VaultServices.Entities.File.File { Id = 7, Properties = new[] { new VaultServices.Entities.Property.Property { ParentId = 69, Type = "File", Name = "Version", Value = "3" } } }, 7));
            var queryFolders = Substitute.For<IQueryOperation<Folder>>();
            queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);
            queryFolders.Execute().Returns(Enumerable.Repeat(new Folder { Id = 1, Properties = new[] { new VaultServices.Entities.Property.Property { ParentId = 31, Type = "Folder", Name = "Path", Value = "$/Designs" } } }, 5));

            var expression = Substitute.For<IExpression<VaultServices.Entities.Property.Property>>();
            expression.TopCount.Returns(10);

            var findProperties = Container.Resolve<FindProperties>();
            findProperties.QueryFolders = queryFolders;
            findProperties.QueryFiles = queryFiles;

            Assert.IsTrue(findProperties.CanExecute(expression));
            var result = findProperties.Execute().ToList();
            Assert.AreEqual(10, result.Count());
            Assert.AreEqual(7, result.Count(property => property.Type == "File"));
            Assert.AreEqual(3, result.Count(property => property.Type == "Folder"));
            queryFiles.Received().Execute();
            queryFolders.Received().Execute();
        }

        [Test]
        public void Test_Execute_ExpressionSkip3_ItSkips2Files1FolderAndReturnsOneFolder()
        {
			var expressionParser = Container.SubstituteFor<IExpressionParser<VaultServices.Entities.Property.Property>>();
            expressionParser.ParseFor<VaultServices.Entities.File.File>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<VaultServices.Entities.File.File>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));
            expressionParser.ParseFor<Folder>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<Folder>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));

            var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
            queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);
            queryFiles.Execute().Returns(Enumerable.Repeat(new VaultServices.Entities.File.File { Id = 7, Properties = new[] { new VaultServices.Entities.Property.Property { ParentId = 69, Type = "File", Name = "Version", Value = "3" } } }, 2));
            var queryFolders = Substitute.For<IQueryOperation<Folder>>();
            queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);
            queryFolders.Execute().Returns(new[] {
                new Folder { Id = 310, Properties = new[]{ new VaultServices.Entities.Property.Property{ ParentId = 310, Type = "Folder", Name = "Name", Value = "Lock"} }},
                new Folder { Id = 690, Properties = new[]{ new VaultServices.Entities.Property.Property{ ParentId = 690, Type = "Folder", Name = "Name", Value = "Pad"} }}
            });

            var expression = Substitute.For<IExpression<VaultServices.Entities.Property.Property>>();
            expression.SkipCount.Returns(3);

            var findProperties = Container.Resolve<FindProperties>();
            findProperties.QueryFolders = queryFolders;
            findProperties.QueryFiles = queryFiles;

            Assert.IsTrue(findProperties.CanExecute(expression));
            Assert.AreEqual(690, findProperties.Execute().First().ParentId);
            queryFiles.Received().Execute();
            queryFolders.Received().Execute();
        }

        [Test]
        public void Test_Execute_ExpressionNameEndsWithGoryAndValueStartsWithWork_ReturnsTwoFilesWithThisCategory()
        {
			var expressionParser = Container.SubstituteFor<IExpressionParser<VaultServices.Entities.Property.Property>>();
            expressionParser.ParseFor<VaultServices.Entities.File.File>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<VaultServices.Entities.File.File>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));
            expressionParser.ParseFor<Folder>(Arg.Any<IExpression<VaultServices.Entities.Property.Property>>()).Returns(info => CreateExpressionMock<Folder>(info.Arg<IExpression<VaultServices.Entities.Property.Property>>()));

            var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
            queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);
            queryFiles.Execute().Returns(new[]{new VaultServices.Entities.File.File
            {
                Id = 7,
                Properties = new[] { new VaultServices.Entities.Property.Property {ParentId = 69, Type = "File", Name = "Version", Value = "3"}, new VaultServices.Entities.Property.Property{ ParentId = 69, Name = "Category", Value = "Work in progress"} 
                }
            }});
            var queryFolders = Substitute.For<IQueryOperation<Folder>>();
            queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);
            queryFolders.Execute().Returns(new[] {
                new Folder { Id = 310, Properties = new[]{ new VaultServices.Entities.Property.Property{ ParentId = 310, Type = "Folder", Name = "Name", Value = "Work in progress"} }},
                new Folder { Id = 690, Properties = new[]{ new VaultServices.Entities.Property.Property{ ParentId = 690, Type = "Folder", Name = "Category", Value = "Released"} }}
            });

            var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Property.Property>>();
            whereToken.PropertyName.Returns("Value");
            whereToken.Operator.Returns(OperatorType.StartsWith);
            whereToken.Value.Returns("Wor");

            var whereToken2 = Substitute.For<IWhereToken<VaultServices.Entities.Property.Property>>();
            whereToken2.PropertyName.Returns("Name");
            whereToken2.Operator.Returns(OperatorType.EndsWith);
            whereToken2.Value.Returns("gory");
            var expression = Substitute.For<IExpression<VaultServices.Entities.Property.Property>>();
            expression.Where.GetEnumerator().Returns(info => new List<IWhereToken<VaultServices.Entities.Property.Property>> { whereToken, whereToken2 }.GetEnumerator());

            var findProperties = Container.Resolve<FindProperties>();
            findProperties.QueryFolders = queryFolders;
            findProperties.QueryFiles = queryFiles;

            Assert.IsTrue(findProperties.CanExecute(expression));
            Assert.AreEqual(69, findProperties.Execute().First().ParentId);
            queryFiles.Received().Execute();
            queryFolders.Received().Execute();
        }

        private IExpression<T> CreateExpressionMock<T>(IExpression<VaultServices.Entities.Property.Property> propertyExpression)
        {
            var whereToken = Substitute.For<IWhereToken<T>>();
            var token = propertyExpression.Where.FirstOrDefault();
            if (token != null)
            {
                whereToken.Value.Returns(token.Value);
                whereToken.PropertyName.Returns(token.PropertyName);
                whereToken.Operator.Returns(token.Operator);
            }

            var entityExpression = Substitute.For<IExpression<T>>();
            entityExpression.Where.GetEnumerator()
                .Returns(new List<IWhereToken<T>> { whereToken }.GetEnumerator());
            entityExpression.TopCount.Returns(propertyExpression.TopCount);
            entityExpression.SkipCount.Returns(propertyExpression.SkipCount);
            return entityExpression;
        }
    }
}