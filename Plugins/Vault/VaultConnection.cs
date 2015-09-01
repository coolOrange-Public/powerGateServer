using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using UserServices.Vault.Entities;
using File = Autodesk.Connectivity.WebServices.File;

namespace UserServices.Vault
{
	public interface IVaultConnection
	{
		WebServiceManager WebServiceManager { get; }

		Folder RootFolder { get; }
		IEnumerable<PropDef> PropertyDefinitions { get; }

		Dictionary<long,IEnumerable<Property>> GetFileProperties(IEnumerable<long> fileIds);
		IEnumerable<File> SearchFiles(IEnumerable<SrchCond> conditions,
			IEnumerable<SrchSort> sorts, int quantity);

		PropDef GetPropertyDefinitionByName(string name);
		bool PropertyDefinitionExists(string name);
		Cat GetCategoryByName(string categoryName);

		File UpdateFileCategory(File file, string category);
	}

	public class VaultConnection : IVaultConnection
	{
		public WebServiceManager WebServiceManager { get; private set; }
		
		public IEnumerable<PropDef>  PropertyDefinitions { get; private set; }
		public IEnumerable<Cat> CategoryDefinitions { get; private set; }
		public VaultConnection(WebServiceManager webServiceManager)
		{
			WebServiceManager = webServiceManager;
			PropertyDefinitions = WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
			CategoryDefinitions = WebServiceManager.CategoryService.GetCategoriesByEntityClassId("FILE", false);
		}

		public Folder RootFolder
		{
			get
			{
				return WebServiceManager.DocumentService.GetFolderRoot();
			}
		}

		public Cat GetCategoryByName(string categoryName)
		{
			return CategoryDefinitions.First(c => c.Name.Equals(categoryName));
		}

		public Dictionary<long, IEnumerable<Property>> GetFileProperties(IEnumerable<long> fileIds)
		{
			var fileProps = new Dictionary<long, IEnumerable<Property>>();
			var properties = WebServiceManager.PropertyService.GetProperties("FILE", fileIds.ToArray(), PropertyDefinitions.Select(p => p.Id).ToArray());
			foreach (var prop in properties)
			{
				if (prop.Val == null)
					prop.Val = string.Empty;
				var props = fileProps[prop.EntityId] ?? new List<Property>();

				((List<Property>)props).Add(new Property
				{ 
					EntityId = (int)prop.EntityId, 
					Name = PropertyDefinitions.First(p => p.Id == prop.PropDefId).DispName, 
					Value = prop.Val.ToString() });
			}
			return fileProps;
		}

		public IEnumerable<Autodesk.Connectivity.WebServices.File> SearchFiles(IEnumerable<SrchCond> conditions,IEnumerable<SrchSort> sorts, int quantity)
		{
			var srcSorts = new List<SrchSort>();
			string bookmark = string.Empty;
			SrchStatus status = null;
			var results = new List<Autodesk.Connectivity.WebServices.File>();

			while (status == null || results.Count < status.TotalHits)
			{
				try
				{
					var result = WebServiceManager.DocumentService.FindFilesBySearchConditions(conditions.ToArray(), srcSorts.ToArray(), new[] { RootFolder.Id }, true, true, ref bookmark, out status);
					if (result == null)
						break;
					results.AddRange(result);
					if (results.Count >= quantity) 
						break;
				}
				catch (Exception ex)
				{
				}
			}
			return results.Take(quantity);
		}

		public PropDef GetPropertyDefinitionByName(string name)
		{
			return PropertyDefinitions.FirstOrDefault(p => p.DispName.Equals(name));
		}

		public bool PropertyDefinitionExists(string name)
		{
			return GetPropertyDefinitionByName(name) != null;
		}

		public File UpdateFileCategory(File file, string category)
		{
			var newCat = GetCategoryByName(category);
			return WebServiceManager.DocumentServiceExtensions.UpdateFileCategories(new[] { file.MasterId }, new[] { newCat.Id }, "Updated category").FirstOrDefault();
		}
	}
}