using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ErpServices.FileSystem
{
	public interface IDirectoryInfo
	{
		void Create();
		IEnumerable<IFileInfo> GetFiles(string searchPattern, SearchOption option);
		string FullName { get; }
		bool Exists { get; }
		IFileInfo AddFile(string fileName);
	}

	public class DirectoryInfo : IDirectoryInfo
	{
		private readonly System.IO.DirectoryInfo _base;

		public DirectoryInfo(string path)
		{
			_base = new System.IO.DirectoryInfo(path);
		}

		public void Create()
		{
			_base.Create();
		}

		public IEnumerable<IFileInfo> GetFiles(string searchPattern, SearchOption option)
		{
			return _base.GetFiles(searchPattern, option).Select(f=>new FileInfo(f));
		}

		public string FullName
		{
			get { return _base.FullName; }
		}

		public bool Exists
		{
			get { return _base.Exists; }
		}

		public IFileInfo AddFile(string fileName)
		{
			var filePath = Path.Combine(_base.FullName, fileName);
			return new FileInfo(filePath);
		}
	}
}