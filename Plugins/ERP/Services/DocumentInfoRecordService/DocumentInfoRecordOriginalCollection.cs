using System;
using System.IO;
using System.Linq;
using ErpServices.Database;
using ErpServices.FileSystem;
using ErpServices.Services.DocumentInfoRecordService.Entities;
using powerGateServer.SDK;
using powerGateServer.SDK.Helpers;
using DirectoryInfo = ErpServices.FileSystem.DirectoryInfo;
using FileInfo = ErpServices.FileSystem.FileInfo;
using FileStream = powerGateServer.SDK.Streams.FileStream;

namespace ErpServices.Services.DocumentInfoRecordService
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

		public IStream Download(DocumentInfoRecordOriginal entity)
		{
			var file = GetFileStore(entity);
			if (!file.Exists)
				Throw(entity, "The entity {0} with key: [{1}] has no file attached!");
			return new FileStream(file.FullName);
		}

		public void Upload(DocumentInfoRecordOriginal entity, IStream stream)
		{
			using (var fileStream = File.Create(GetFileStore(entity).FullName))
				stream.Source.CopyTo(fileStream);
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
			var folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			var specificFolder = Path.Combine(folder, @"coolorange\powerGateServer\Plugins\ERP\Store\Files\");
			if (string.IsNullOrEmpty(ClientId))
				specificFolder = Path.Combine(specificFolder, "default");
			else
				specificFolder = Path.Combine(specificFolder, ClientId);
			Directory.CreateDirectory(specificFolder);
			return new DirectoryInfo(specificFolder);
		}
	}
}