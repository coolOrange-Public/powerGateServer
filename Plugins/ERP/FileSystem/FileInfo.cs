using System.IO;

namespace ErpServices.FileSystem
{
	public interface IFileInfo
	{
		string FullName { get; }
		Stream Create();
		Stream OpenRead();
		Stream OpenWrite();
		void Delete();
		bool Exists { get; }
	}

	public class FileInfo : IFileInfo
	{
		private readonly System.IO.FileInfo _base;

		public FileInfo(System.IO.FileInfo @base)
		{
			_base = @base;
		}

		public FileInfo(string fileLocation)
		{
			_base = new System.IO.FileInfo(fileLocation);
		}

		public string FullName { get { return _base.FullName; } }

		public Stream Create()
		{
			return _base.Create();
		}

		public Stream OpenWrite()
		{
			return _base.OpenWrite();
		}

		public Stream OpenRead()
		{
			return _base.OpenRead();
		}

		public void Delete()
		{
			_base.Delete();
		}

		public bool Exists
		{
			get { return _base.Exists; }
		}
	}
}