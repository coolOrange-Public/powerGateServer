using System;
using System.IO;
using System.Linq;
using powerGateServer.SDK;
using powerGateServer.SDK.Helpers;
using SapServices.Database;
using SapServices.FileSystem;
using SapServices.Services.DocumentInfoRecordService.Entities;
using DirectoryInfo = SapServices.FileSystem.DirectoryInfo;
using FileInfo = SapServices.FileSystem.FileInfo;
using FileStream = powerGateServer.SDK.Streams.FileStream;

namespace SapServices.Services.DocumentInfoRecordService
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
			var specificFolder = Path.Combine(folder, @"coolorange\powerGateServer\Plugins\SAP\Store\Files");
			return new DirectoryInfo(specificFolder);
		}
	}
}