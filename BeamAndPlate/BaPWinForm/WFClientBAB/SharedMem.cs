
using System;
using System.Runtime.InteropServices;

namespace SM
{
	public sealed class SharedMem : IDisposable
	{

		enum FileProtection : uint{	ReadOnly=2,ReadWrite=4}
		enum FileRights: uint{	Read=4,Write=2,ReadWrite=Read+Write}

		[DllImport ("kernel32.dll",SetLastError=true)]
		static extern IntPtr CreateFileMapping(IntPtr hFile, int lpAttributes, FileProtection flProtect, uint dwMaxSizeHigh,uint dwMaxSizeLow,string lpName);


		[DllImport ("kernel32.dll",SetLastError=true)]
		static extern IntPtr OpenFileMapping(FileRights dwDesiredAccess, bool bInheritHandle, string lpName);

		[DllImport ("kernel32.dll",SetLastError=true)]
		static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject,FileRights dwDesiredAccess,uint dwFileOffsetHigh, uint dwFileOffsetLow,uint dwNumberOfBytesToMap);

		[DllImport ("kernel32.dll",SetLastError=true)]
		static extern bool UnmapViewOfFile(IntPtr map);


		[DllImport ("kernel32.dll",SetLastError=true)]
		static extern int CloseHandle(IntPtr hObject);

		static readonly IntPtr NoFileHandle = new IntPtr(-1);

		IntPtr fileHandle,fileMap;

		public IntPtr Root
		{
			get{ return fileMap;}
		}

		public SharedMem(string name,bool existing,uint sizeIntBytes)
		{
			if(existing) fileHandle= OpenFileMapping(FileRights.ReadWrite,false,name);
			else fileHandle = CreateFileMapping(NoFileHandle,0,FileProtection.ReadWrite,0,sizeIntBytes,name);

			if(fileHandle==IntPtr.Zero)
			{
				throw new Exception("probleme");
			}

			fileMap = MapViewOfFile(fileHandle,FileRights.ReadWrite,0,0,0);
			if(fileMap==IntPtr.Zero)
			{
				throw new Exception("probleme");
			}
		}

		public void Dispose()
		{
			if(fileMap!=IntPtr.Zero) UnmapViewOfFile(fileMap);
			if(fileHandle!=IntPtr.Zero) CloseHandle(fileHandle);
			fileMap=fileHandle=IntPtr.Zero;
		}
	}

}