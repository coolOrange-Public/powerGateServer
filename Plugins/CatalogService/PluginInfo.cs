﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CatalogService
{
	public class PluginInfo
	{
		private readonly Assembly _assembly;

		public PluginInfo(Assembly assembly)
		{
			_assembly = assembly;
		}

		public string GetCompany()
		{
			var companyAttribute = GetAssemblyAttribute<AssemblyCompanyAttribute>();
			if (companyAttribute != null)
				return companyAttribute.Company;
			return string.Empty;
		}

		public DateTime GetBuildTime()
		{
			if (!_assembly.IsDynamic && File.Exists(_assembly.Location))
			{
				var buffer = new byte[Math.Max(Marshal.SizeOf(typeof(_IMAGE_FILE_HEADER)), 4)];
				using (var fileStream = new FileStream(_assembly.Location, FileMode.Open, FileAccess.Read))
				{
					fileStream.Position = 0x3C;
					fileStream.Read(buffer, 0, 4);
					fileStream.Position = BitConverter.ToUInt32(buffer, 0);
					fileStream.Read(buffer, 0, 4);
					fileStream.Read(buffer, 0, buffer.Length);
				}
				var pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
				try
				{
					var coffHeader = (_IMAGE_FILE_HEADER)Marshal.PtrToStructure(pinnedBuffer.AddrOfPinnedObject(), typeof(_IMAGE_FILE_HEADER));
					return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1) + new TimeSpan(coffHeader.TimeDateStamp * TimeSpan.TicksPerSecond));
				}
				finally
				{
					pinnedBuffer.Free();
				}
			}
			return new DateTime();
		}

		public T GetAssemblyAttribute<T>() where T : Attribute
		{
			return (T)Attribute.GetCustomAttribute(_assembly, typeof(T));
		}

		struct _IMAGE_FILE_HEADER
		{
			public ushort Machine;
			public ushort NumberOfSections;
			public uint TimeDateStamp;
			public uint PointerToSymbolTable;
			public uint NumberOfSymbols;
			public ushort SizeOfOptionalHeader;
			public ushort Characteristics;
		};
	}
}