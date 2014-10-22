using System;
using System.IO;
using System.Linq;
using powerGateServer.Addins;
using powerGateServer.Addins.Extensions;
using UserServices.Entities;
using UserServices.FileSystem;
using DirectoryInfo = UserServices.FileSystem.DirectoryInfo;
using FileInfo = UserServices.FileSystem.FileInfo;

namespace UserServices.ServiceDefinition
{
	public class DocumentInfoRecordOriginalCollection :
		DirNavigationPropertyCollectionEntitySet<DocumentInfoRecordOriginal>, 
		IStreamableServiceMethod<DocumentInfoRecordOriginal>
	{
		public DocumentInfoRecordOriginalCollection(IEntityStores entityStores) 
			: base(entityStores)
		{
			var fileStore = GetFileStoreDirectory();
			if (!fileStore.Exists)
				fileStore.Create();
		}

		public override string Name
		{
			get { return "DocumentInfoRecordOriginalCollection"; }
		}

		public Stream GetQueryStream(DocumentInfoRecordOriginal entity)
		{
			var file = GetFileStore(entity);
			if (!file.Exists)
				Throw(entity, "The entity {0} with key: [{1}] has no file attached!");
			return file.OpenRead();
		}

		public Stream GetCreateStream(DocumentInfoRecordOriginal entity)
		{
			return File.Create(GetFileStore(entity).FullName);
		}

		public Stream GetUpdateStream(DocumentInfoRecordOriginal entity)
		{
			return GetCreateStream(entity);
		}

		public void DeleteStream(DocumentInfoRecordOriginal entity)
		{
			GetFileStore(entity).Delete();
		}

		IFileInfo GetFileStore(DocumentInfoRecordOriginal entity)
		{
			var fileLoc= Path.Combine(GetFileStoreDirectory().FullName, GetFileName(entity));
			return new FileInfo(fileLoc);
		}

		string GetFileName(DocumentInfoRecordOriginal entity)
		{
			return string.Join("_", entity.GetDataServiceKeys().Values.ToArray());
		}

		IDirectoryInfo GetFileStoreDirectory()
		{
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var specificFolder = Path.Combine(folder, "coolorange\\powerGateServer\\SAP\\Files");
			return new DirectoryInfo(specificFolder);
		}
	}
}