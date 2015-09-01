using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Connectivity.WebServices;
using powerGateServer.SDK;
using UserServices.Vault.Entities;
using VaultServices;
using File = Autodesk.Connectivity.WebServices.File;

namespace UserServices.Vault.Entities
{
	public partial class File
	{
		public File(IVaultConnection vaultConnection,
			Autodesk.Connectivity.WebServices.File vaultFile)
		{
			Id = (int)vaultFile.Id;
			MasterId = (int)vaultFile.MasterId;
			Category = vaultFile.Cat.CatName;
			Classification = vaultFile.FileClass.ToString();
			Name = vaultFile.Name;
			State = vaultFile.FileLfCyc.LfCycStateName;
			Version = vaultFile.VerNum;
			Created = vaultFile.CreateDate;
			CreatedBy = vaultFile.CreateUserName;
			Modified = vaultFile.ModDate;
			Revision = vaultFile.FileRev.Label;

			var category = vaultConnection.GetCategoryByName(vaultFile.Cat.CatName);
			CatColor = category.Color;
		}
	}
}


namespace UserServices.Vault
{
	public interface IEntityConverter
	{
		Entities.File ToDataServiceFile(File file);
		string ToVaultProperty(string dataServicePropertyName);
		
		IEnumerable<SrchCond> ToSearchConditions(IWhereClause<Entities.File> whereExpression);
		IEnumerable<SrchSort> ToSearchSort(IOrderByClause<Entities.File> orderBy);
		VaultEntityConverter.SrchOperatorType ToSearchOperator(OperatorType? operatorType);
		SearchRuleType ToSearchRule(LogicalOperator? rule);
	}

	public class VaultEntityConverter : IEntityConverter
	{
		private readonly IVaultConnection _vaultConnection;
		readonly Dictionary<string, string> _propertyMappings = new Dictionary<string, string>
			{
				{ "Name","File Name"},
				{ "Id","Id" },
				{ "MasterId" , "MasterId" },
				{ "Category" , "Category Name" },
				{ "Classification" , "Classification" },
				{ "State", "State" },
				{ "Version", "Version" },
				{ "Revision", "Revision" },
				{ "Created", "Original Create Date" },
				{ "Modified", "Date Modified" },
				{ "CreatedBy", "Created By" },
				{ "ModifiedBy", "ModifiedBy"},
				{ "CatColor", "CatColor"},
				{ "Title","Title"},
				{ "PartNumber","PartNumber"}
			};

		public VaultEntityConverter(IVaultConnection vaultConnection)
		{
			_vaultConnection = vaultConnection;
		}

		public IEnumerable<Entities.File> ToDataServiceFiles(IEnumerable<File> files)
		{
			var dsFiles = files.Select(f=>new Entities.File(_vaultConnection,f));
			var personsList = new LazyEnumerable<Property>(() =>
			{
				_vaultConnection.GetFileProperties(files.Select(f => f.Id));
			});
			return files;
		}

		public IEnumerable<SrchCond> ToSearchConditions(IWhereClause<Entities.File> whereExpression)
		{
			var srchConds = new List<SrchCond>();
			foreach (var tokenExpression in whereExpression)
			{
				var propDef = GetPropertyDefinition(tokenExpression.PropertyName);
				if (propDef == null)
					continue;

				var cnd = new SrchCond
				{
					PropDefId = propDef.Id,
					PropTyp = PropertySearchType.SingleProperty,
					SrchOper = (int)ToSearchOperator(tokenExpression.Operator),
					SrchRule = ToSearchRule(tokenExpression.Rule),
					SrchTxt = GetSearchValue(tokenExpression)
				};
				srchConds.Add(cnd);
			}
			return srchConds;
		}

		string GetSearchValue(IWhereToken<Entities.File> tokenExpression)
		{
			if (tokenExpression.Operator == OperatorType.EndsWith || tokenExpression.Operator == OperatorType.DoesNotEndsWith)
				return "*" + tokenExpression.Value;
			if (tokenExpression.Operator == OperatorType.StartsWith || tokenExpression.Operator == OperatorType.DoesNotStartWith)
				return tokenExpression.Value+"*";
			return tokenExpression.Value.ToString();
		}

		public IEnumerable<SrchSort> ToSearchSort(IOrderByClause<Entities.File> orderBy)
		{
			var srchSort = new List<SrchSort>();
			foreach (var orderby in orderBy)
			{
				var propDef = GetPropertyDefinition(@orderby.PropertyName);
				if(propDef == null)
					continue;
				var sort = new SrchSort
				{
					PropDefId = propDef.Id,
					SortAsc = orderby.Method == OrderingMethod.OrderBy || orderby.Method == OrderingMethod.ThenBy
				};
				srchSort.Add(sort);
			}
			return srchSort;
		}

		public SrchOperatorType ToSearchOperator(OperatorType? operatorType)
		{
			if (operatorType == OperatorType.EndsWith || operatorType == OperatorType.StartsWith)
				return SrchOperatorType.Equals;
			if (operatorType == OperatorType.DoesNotEndsWith || operatorType == OperatorType.DoesNotStartWith)
				return SrchOperatorType.NotEquals;

			return (SrchOperatorType)Enum.Parse(typeof(SrchOperatorType), operatorType.ToString());
		}
		public enum SrchOperatorType
		{
			Contains = 1,
			DoesNotContain = 2,
			Equals = 3,
			Empty = 4,
			NotEmpty = 5,
			GreatherThan = 6,
			GreatherThanOrEquals = 7,
			LessThan = 8,
			LessThanOrEquals = 9,
			NotEquals = 10,
		}

		public SearchRuleType ToSearchRule(LogicalOperator? rule)
		{
			return rule == LogicalOperator.Or ? SearchRuleType.May : SearchRuleType.Must;
		}

		public string ToVaultProperty(string dataServicePropertyName)
		{
			return _propertyMappings[dataServicePropertyName];
		}
		
		PropDef GetPropertyDefinition(string propertyName)
		{
			var vaultProperty = ToVaultProperty(propertyName);
			if (!_vaultConnection.PropertyDefinitionExists(vaultProperty))
				return null;
			return _vaultConnection.GetPropertyDefinitionByName(propertyName);
		}
	}
}