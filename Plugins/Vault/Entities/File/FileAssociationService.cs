using System;
using System.Collections.Generic;
using System.Linq;
using powerVault.Cmdlets.Cmdlets.Vault;
using powerVault.Cmdlets.Cmdlets.Vault.Facade;
using coolOrange.VaultServices.Vault;
using VaultServices.Entities.Base;
using VaultServices.Entities.Link;
using Vault = Autodesk.Connectivity.WebServices;

namespace VaultServices.Entities.File
{
	public class FileAssociationService : IAssociationService<File>
	{
		private readonly IAssociationService _assocService;
		private readonly IVaultServices _vaultServices;

		public FileAssociationService(IAssociationService assocService, IVaultServices vaultServices)
		{
			_assocService = assocService;
			_vaultServices = vaultServices;
		}

		public IEnumerable<Link.Link> GetParents(File entity)
		{
			var parentFolder = GetParentFolder(entity);
			if (parentFolder == null)
				return Enumerable.Empty<Link.Link>();
			return new[] { new Dependency(parentFolder, entity) };
		}

		public IEnumerable<Link.Link> GetChildren(File entity)
		{
			var childs = GetDependencies(entity);
			var attachments = GetAttachments(entity);
			var shortcuts = GetShortcuts(entity);
			return childs.Concat(attachments).Concat(shortcuts).ToList();
		}

		private Vault.Folder GetParentFolder(File entity)
		{
			Exception ex;
			var file = _vaultServices.TryGetFileById(entity.Id, out ex);
			return ex == null ? _vaultServices.TryGetFolderById(file.FolderId, out ex) : default(Vault.Folder);
		}

		private IEnumerable<Link.Link> GetDependencies(File entity)
		{
			var children = GetAssociations(entity, ReadAssocsTyp.Dependencies);
			return children.Select(c => new Dependency(entity, c));
		}

		private IEnumerable<Link.Link> GetAttachments(File entity)
		{
			var attachments = GetAssociations(entity, ReadAssocsTyp.Attachments);
			return attachments.Select(a => new Attachment(entity, a));
		}

		private IEnumerable<Vault.File> GetAssociations(File entity, ReadAssocsTyp assocsTyp)
		{
			return _assocService.GetFileAssociations(new Vault.File { Id = entity.Id }, assocsTyp).Children;
		}

		private IEnumerable<Link.Link> GetShortcuts(File entity)
		{
			Exception ex;
			var vdfFile = _vaultServices.GetFileIterationById(entity.Id);
			var shortcuts = _vaultServices.TryGetLinkedChildren(vdfFile, null, out ex);
			return ex != null ? Enumerable.Empty<Shortcut>() : shortcuts.Select(s => new Shortcut(s.LinkInfo));
		}
	}
}