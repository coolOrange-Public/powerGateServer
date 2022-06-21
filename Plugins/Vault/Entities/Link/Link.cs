using System.Data.Services.Common;
using VaultServices.Entities.Base;
using VaultServices.Extensions;
using VDF = Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using Vault = Autodesk.Connectivity.WebServices;

namespace VaultServices.Entities.Link
{
	/// <summary>
	/// Description has to be a part of the Key because a Folder can have a Shortcut and a Dependency for the same File
	/// </summary>
	[DataServiceKey("ParentId", "ParentType", "ChildId", "ChildType", "Description")]
	[DataServiceEntity]
	public class Link
	{
		public long ParentId { get; set; }
		public string ParentType { get; set; }
		public long ChildId { get; set; }
		public string ChildType { get; set; }
		public string Description { get; set; }

		public Link()
		{
			Description = GetType().Name;
		}
	}

	public class Shortcut : Link
	{
		internal Shortcut(VDF.Link link)
		{
			ChildId = link.TargetId;
			ChildType = link.TargetEntityClassId.ToEntityType();
			ParentId = link.Parent.EntityIterationId;
			ParentType = link.Parent.EntityClass.Id.ToEntityType();
		}
	}

	public class Dependency : Link
	{
		internal Dependency(IBaseObject parent, Vault.File child)
		{
			ParentType = parent.Type;
			ParentId = parent.Id;
			ChildId = child.Id;
			ChildType = typeof(File.File).Name;
		}

		internal Dependency(Vault.Folder parent, IBaseObject childEntity)
		{
			ParentType = typeof(Folder.Folder).Name;
			ParentId = parent.Id;
			ChildId = childEntity.Id;
			ChildType = childEntity.Type;
		}
	}

	public class Attachment : Dependency
	{
		public Attachment(IBaseObject parent, Vault.File child)
			: base(parent, child)
		{
		}
	}
}