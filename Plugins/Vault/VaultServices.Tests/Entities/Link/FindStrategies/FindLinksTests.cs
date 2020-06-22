using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NSubstitute;
using NUnit.Framework;
using powerGateServer.SDK;
using VaultServices.Entities.Base;
using VaultServices.Entities.Folder;
using VaultServices.Entities.Link.FindStrategies;

namespace VaultServices.Tests.Entities.Link.FindStrategies
{
	[TestFixture]
	public class FindLinksTests : ContainerBasedTests
	{
		[Test]
		public void CanExecute_returns_true_when_asking_for_Links_with_certain_ParentId_and_files_supports_this_search()
		{
			var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);

			var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Link.Link>>();
			whereToken.PropertyName.Returns("ParentId");
			whereToken.Operator.Returns(OperatorType.Equals);

			var expression = Substitute.For<IExpression<VaultServices.Entities.Link.Link>>();
			expression.Where.GetEnumerator().Returns(new List<IWhereToken<VaultServices.Entities.Link.Link>> { whereToken }.GetEnumerator());

			var findLinks = Container.Resolve<FindLinks>();
			findLinks.QueryFiles = queryFiles;

			Assert.IsTrue(findLinks.CanExecute(expression));
		}

		[Test]
		public void CanExecute_returns_true_when_asking_for_Links_with_certain_ParentId_and_folders_are_supporting_this_search()
		{
			var queryFolders = Substitute.For<IQueryOperation<Folder>>();
			queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);

			var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Link.Link>>();
			whereToken.PropertyName.Returns("ParentId");
			whereToken.Operator.Returns(OperatorType.Equals);

			var expression = Substitute.For<IExpression<VaultServices.Entities.Link.Link>>();
			expression.Where.GetEnumerator().Returns(new List<IWhereToken<VaultServices.Entities.Link.Link>> { whereToken }.GetEnumerator());

			var findLinks = Container.Resolve<FindLinks>();
			findLinks.QueryFolders = queryFolders;

			Assert.IsTrue(findLinks.CanExecute(expression));
		}

		[Test]
		public void CanExecute_returns_false_when_asking_for_Links_with_where_ParentId_greather_than_and_no_entity_supports_this_search()
		{
			var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(false);

			var queryFolders = Substitute.For<IQueryOperation<Folder>>();
			queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(false);


			var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Link.Link>>();
			whereToken.PropertyName.Returns("ParentId");
			whereToken.Operator.Returns(OperatorType.GreatherThan);

			var expression = Substitute.For<IExpression<VaultServices.Entities.Link.Link>>();
			expression.Where.GetEnumerator().Returns(new List<IWhereToken<VaultServices.Entities.Link.Link>> { whereToken }.GetEnumerator());

			var findLinks = Container.Resolve<FindLinks>();
			findLinks.QueryFiles = queryFiles;
			findLinks.QueryFolders = queryFolders;

			Assert.IsFalse(findLinks.CanExecute(expression));
		}

		[Test]
		public void Execute_returns_all_links_of_all_entities()
		{
			var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);
			queryFiles.Execute().Returns(_files);
			var queryFolders = Substitute.For<IQueryOperation<Folder>>();
			queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);
			queryFolders.Execute().Returns(_folders);

			var expression = Substitute.For<IExpression<VaultServices.Entities.Link.Link>>();
			expression.Where.GetEnumerator().Returns(Enumerable.Empty<IWhereToken<VaultServices.Entities.Link.Link>>().GetEnumerator());

			var findLinks = Container.Resolve<FindLinks>();
			findLinks.QueryFiles = queryFiles;
			findLinks.QueryFolders = queryFolders;

			Assert.IsTrue(findLinks.CanExecute(expression));
			
			var result = findLinks.Execute().ToList();
			
			CollectionAssert.AreEquivalent(_files.SelectMany(f=>f.Children).Concat(_files.SelectMany(f=>f.Parents))
				.Concat(_folders.SelectMany(f => f.Children).Concat(_folders.SelectMany(f => f.Parents))), result);
		}


		[Test]
		public void Execute_where_top_is_set_does_only_ask_for_files_and_not_for_folders_because_it_is_not_needed()
		{
			var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);
			queryFiles.Execute().Returns(_files);
			var queryFolders = Substitute.For<IQueryOperation<Folder>>();
			queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);
			queryFolders.Execute().Returns(_folders);

			var expression = Substitute.For<IExpression<VaultServices.Entities.Link.Link>>();
			expression.Where.GetEnumerator().Returns(Enumerable.Empty<IWhereToken<VaultServices.Entities.Link.Link>>().GetEnumerator());
			expression.TopCount.Returns(3);

			var findLinks = Container.Resolve<FindLinks>();
			findLinks.QueryFiles = queryFiles;
			findLinks.QueryFolders = queryFolders;

			Assert.IsTrue(findLinks.CanExecute(expression));

			var result = findLinks.Execute().ToList();

			Assert.AreEqual(expression.TopCount, result.Count);
			queryFolders.DidNotReceive().Execute();
		}

		static readonly Expression<Func<VaultServices.Entities.Link.Link, bool>>[] WhereParentIsEqualsReturnsAllChildrenLinksOfThatParent = {
			link => link.ParentId == 66,
			link => link.ParentType == "File"
		};
		[Test, TestCaseSource("WhereParentIsEqualsReturnsAllChildrenLinksOfThatParent")]
		public void Execute_where_parent_is_equals_returns_all_children_links_of_that_parent(Expression<Func<VaultServices.Entities.Link.Link,bool>> where)
		{
			var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);
			queryFiles.Execute().Returns(_files);
			var queryFolders = Substitute.For<IQueryOperation<Folder>>();
			queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);
			queryFolders.Execute().Returns(_folders);

			var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Link.Link>>();
			whereToken.PropertyName.Returns(((dynamic)where.Body).Left.Member.Name as string);
			whereToken.Operator.Returns(OperatorType.Equals);
			whereToken.Value.Returns(((dynamic)where.Body).Right.Value as object);

			var expression = Substitute.For<IExpression<VaultServices.Entities.Link.Link>>();
			expression.Where.GetEnumerator().Returns(info => new List<IWhereToken<VaultServices.Entities.Link.Link>> { whereToken }.GetEnumerator());
			expression.Where.IsSet().Returns(true);

			var findLinks = Container.Resolve<FindLinks>();
			findLinks.QueryFiles = queryFiles;
			findLinks.QueryFolders = queryFolders;

			Assert.IsTrue(findLinks.CanExecute(expression));

			var result = findLinks.Execute().ToList();

			CollectionAssert.AreEquivalent(_files.SelectMany(f => f.Children).Where(where.Compile()), result);
		}

		static readonly Expression<Func<VaultServices.Entities.Link.Link, bool>>[] WhereChildIsEqualsReturnsAllParentLinksOfThatChild = {
			link => link.ChildId == 66,
			link => link.ChildType == "File"
		};
		[Test, TestCaseSource("WhereChildIsEqualsReturnsAllParentLinksOfThatChild")]
		public void Execute_where_child_is_equals_returns_all_parent_links_of_that_child(Expression<Func<VaultServices.Entities.Link.Link, bool>> where)
		{
			var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);
			queryFiles.Execute().Returns(_files);
			var queryFolders = Substitute.For<IQueryOperation<Folder>>();
			queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);
			queryFolders.Execute().Returns(_folders);

			var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Link.Link>>();
			whereToken.PropertyName.Returns(((dynamic)where.Body).Left.Member.Name as string);
			whereToken.Operator.Returns(OperatorType.Equals);
			whereToken.Value.Returns(((dynamic)where.Body).Right.Value as object);

			var expression = Substitute.For<IExpression<VaultServices.Entities.Link.Link>>();
			expression.Where.GetEnumerator().Returns(info => new List<IWhereToken<VaultServices.Entities.Link.Link>> { whereToken }.GetEnumerator());
			expression.Where.IsSet().Returns(true);

			var findLinks = Container.Resolve<FindLinks>();
			findLinks.QueryFiles = queryFiles;
			findLinks.QueryFolders = queryFolders;

			Assert.IsTrue(findLinks.CanExecute(expression));

			var result = findLinks.Execute().ToList();

			CollectionAssert.AreEquivalent(_files.SelectMany(f => f.Parents).Where(where.Compile()), result);
		}

		[Test]
		public void Execute_returns_all_link_types_when_searching_for_description_only_without_parent_or_child()
		{
			var queryFiles = Substitute.For<IQueryOperation<VaultServices.Entities.File.File>>();
			queryFiles.CanExecute(Arg.Any<IExpression<VaultServices.Entities.File.File>>()).Returns(true);
			queryFiles.Execute().Returns(_files);
			var queryFolders = Substitute.For<IQueryOperation<Folder>>();
			queryFolders.CanExecute(Arg.Any<IExpression<Folder>>()).Returns(true);
			queryFolders.Execute().Returns(_folders);

			var whereToken = Substitute.For<IWhereToken<VaultServices.Entities.Link.Link>>();
			whereToken.PropertyName.Returns("Description");
			whereToken.Operator.Returns(OperatorType.Contains);
			whereToken.Value.Returns("e");

			var expression = Substitute.For<IExpression<VaultServices.Entities.Link.Link>>();
			expression.Where.GetEnumerator().Returns(info => new List<IWhereToken<VaultServices.Entities.Link.Link>> { whereToken }.GetEnumerator());
			expression.Where.IsSet().Returns(true);

			var findLinks = Container.Resolve<FindLinks>();
			findLinks.QueryFiles = queryFiles;
			findLinks.QueryFolders = queryFolders;

			Assert.IsTrue(findLinks.CanExecute(expression));

			var result = findLinks.Execute().ToList();

			CollectionAssert.AreEquivalent(_files.SelectMany(f => f.Children).Concat(_files.SelectMany(f => f.Parents))
				.Concat(_folders.SelectMany(f => f.Children).Concat(_folders.SelectMany(f => f.Parents)))
				.Where(l=>l.Description.Contains("e")), result);
		}


		readonly VaultServices.Entities.File.File[] _files = {
                new VaultServices.Entities.File.File
                {
	                Id = 66,
					Parents =  new[]{ new VaultServices.Entities.Link.Link
						{
							ParentId = 33, ParentType = "Folder", ChildId = 66, ChildType = "File", Description = ""
						}, new VaultServices.Entities.Link.Link{
							ParentId = 65, ParentType = "File", ChildId = 66, ChildType = "File", Description = "Dependency"
						}
					},
					Children = new[]{ new VaultServices.Entities.Link.Link
						{
							ParentId = 66, ParentType = "File", ChildId = 67, ChildType = "File", Description = "Dependency"
						}, new VaultServices.Entities.Link.Link
						{
							ParentId = 66, ParentType = "File", ChildId = 68, ChildType = "File", Description = "Attachment"
						}
					}
                },
                new VaultServices.Entities.File.File
                {
	                Id = 99,
					Parents =  new[]{ new VaultServices.Entities.Link.Link
						{
							ParentId = 77, ParentType = "Folder", ChildId = 99, ChildType = "File", Description = ""
						}
					},
					Children = new[]{ new VaultServices.Entities.Link.Link
						{
							ParentId = 99, ParentType = "File", ChildId = 66, ChildType = "File", Description = "Dependency"
						}
					}
                }
            };

		readonly Folder[] _folders = {
                new Folder
                {
	                Id = 666,
					Parents =  new[]{ new VaultServices.Entities.Link.Link
						{
							ParentId = 333, ParentType = "Folder", ChildId = 666, ChildType = "Folder", Description = ""
						}
					},
					Children = new[]{ new VaultServices.Entities.Link.Link
						{
							ParentId = 666, ParentType = "Folder", ChildId = 667, ChildType = "Folder", Description = ""
						}, new VaultServices.Entities.Link.Link
						{
							ParentId = 666, ParentType = "Folder", ChildId = 668, ChildType = "Folder", Description = "Shortcut"
						}
					}
                },
                new Folder
                {
	                Id = 999,
					Parents =  new[]{ new VaultServices.Entities.Link.Link
						{
							ParentId = 777, ParentType = "Folder", ChildId = 999, ChildType = "Folder", Description = "Shortcut"
						}
					},
					Children = new[]{ new VaultServices.Entities.Link.Link
						{
							ParentId = 999, ParentType = "Folder", ChildId = 696, ChildType = "Folder", Description = ""
						}
					}
                }
            };
	}
}