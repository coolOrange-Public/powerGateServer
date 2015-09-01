using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ErpServices.Tests
{
	public static class StringExtensions
	{
		public static Stream ToStream(this string @this, Action<MemoryStream> onDisposing = null)
		{
			if (onDisposing == null)
				onDisposing = memoryStream => { };
			var stream = new MyMemoryStream(onDisposing);
			var writer = new StreamWriter(stream);
			writer.Write(@this);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}

		public static IEnumerable<string> ReadAllLines(this Stream @this,
									 Encoding encoding)
		{
			@this.Position = 0;
			Stream copy = new MemoryStream();
			@this.CopyTo(copy);
			copy.Position = 0;
			using(copy)
			using (var reader = new StreamReader(copy, encoding))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
					yield return line;
			}
		}

		private class MyMemoryStream : MemoryStream
		{
			private readonly Action<MemoryStream> _onDisposing;

			public MyMemoryStream(Action<MemoryStream> onDisposing)
			{
				_onDisposing = onDisposing;
			}

			protected override void Dispose(bool disposing)
			{
				_onDisposing(this);
				base.Dispose(disposing);
			}
		}
	}
}
