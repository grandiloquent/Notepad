namespace Common
{

	using System;
	using System.Buffers;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Text.RegularExpressions;
	public static  class Zips
	{
		internal static ZipArchiveEntry DoCreateEntryFromFile(this ZipArchive destination,
			string sourceFileName, string entryName, CompressionLevel? compressionLevel)
		{
			if (destination == null)
				throw new ArgumentNullException();
			if (sourceFileName == null)
				throw new ArgumentNullException();
			if (entryName == null)
				throw new ArgumentNullException();
			// Checking of compressionLevel is passed down to DeflateStream and the IDeflater implementation
			// as it is a pluggable component that completely encapsulates the meaning of compressionLevel.
			// Argument checking gets passed down to FileStream's ctor and CreateEntry
			using (Stream fs = new FileStream(sourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 0x1000, useAsync: false)) {
				ZipArchiveEntry entry = compressionLevel.HasValue
                                    ? destination.CreateEntry(entryName, compressionLevel.Value)
                                    : destination.CreateEntry(entryName);
				DateTime lastWrite = File.GetLastWriteTime(sourceFileName);
				// If file to be archived has an invalid last modified time, use the first datetime representable in the Zip timestamp format
				// (midnight on January 1, 1980):
				if (lastWrite.Year < 1980 || lastWrite.Year > 2107)
					lastWrite = new DateTime(1980, 1, 1, 0, 0, 0);
				entry.LastWriteTime = lastWrite;
				using (Stream es = entry.Open())
					fs.CopyTo(es);
				return entry;
			}
		}
		private static void EnsureCapacity(ref char[] buffer, int min)
		{
			if (buffer.Length < min) {
				int newCapacity = buffer.Length * 2;
				if (newCapacity < min)
					newCapacity = min;
				ArrayPool<char>.Shared.Return(buffer);
				buffer = ArrayPool<char>.Shared.Rent(newCapacity);
			}
		}
		private static string EntryFromPath(this string entry, int offset, int length, ref char[] buffer, bool appendPathSeparator = false)
		{
			const char PathSeparator = '/';
			while (length > 0) {
				if (entry[offset] != Path.DirectorySeparatorChar &&
				    entry[offset] != Path.AltDirectorySeparatorChar)
					break;
				offset++;
				length--;
			}
			if (length == 0)
				return appendPathSeparator ? PathSeparator.ToString() : string.Empty;
			int resultLength = appendPathSeparator ? length + 1 : length;
			EnsureCapacity(ref buffer, resultLength);
			entry.CopyTo(offset, buffer, 0, length);
			for (int i = 0; i < length; i++) {
				char ch = buffer[i];
				if (ch == Path.DirectorySeparatorChar || ch == Path.AltDirectorySeparatorChar)
					buffer[i] = PathSeparator;
			}
			if (appendPathSeparator)
				buffer[length] = PathSeparator;
			return new string(buffer, 0, resultLength);
		}
		
		static string GetZipFileName(string dir)
		{
			// DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") 
			return  dir + "-" + (long)DateTimes.ToUnixTimeMillisecond() + ".zip";
		}
		public static void ZipAndroidProject(this string dir,
			CompressionLevel compressionLevel = CompressionLevel.Fastest)
		{
			
			var targetFile = GetZipFileName(dir);
			if (File.Exists(targetFile))
				return;
			var encoding = Encoding.GetEncoding("gbk");
			using (ZipArchive archive = ZipFile.Open(targetFile, ZipArchiveMode.Create, encoding)) {
				bool directoryIsEmpty = true;
				var di = new DirectoryInfo(dir);
				var basePath = di.FullName;
				const int DefaultCapacity = 260;
				char[] entryNameBuffer = ArrayPool<char>.Shared.Rent(DefaultCapacity);
				try {
					var gradleFiles = new DirectoryInfo(Path.Combine(basePath, "gradle")).GetFileSystemInfos("*", SearchOption.AllDirectories);
					var appFiles = new DirectoryInfo(Path.Combine(basePath, "app")).GetFileSystemInfos("*");
					var srcFiles = new DirectoryInfo(Path.Combine(basePath, "app\\src")).GetFileSystemInfos("*", SearchOption.AllDirectories);
					var files = di.GetFileSystemInfos("*");
					var list = files.Concat(gradleFiles).Concat(appFiles).Concat(srcFiles);
					foreach (var file in list) {
						directoryIsEmpty = false;
						int entryNameLength = file.FullName.Length - basePath.Length;
						if (file is FileInfo) {
							string entryName = EntryFromPath(file.FullName, basePath.Length, entryNameLength, ref entryNameBuffer);
							DoCreateEntryFromFile(archive, file.FullName, entryName, compressionLevel);
						} else {
							DirectoryInfo possiblyEmtpy = file as DirectoryInfo;
							if (possiblyEmtpy != null && possiblyEmtpy.IsDirectoryEmpty()) {
								string entryName = EntryFromPath(file.FullName, basePath.Length, entryNameLength, ref entryNameBuffer, true);
								archive.CreateEntry(entryName);
							}
						}
					}
					//if (directoryIsEmpty)
					//{
					//    archive.CreateEntry(EntryFromPath(di.Name, 0, di.Name.Length, ref entryNameBuffer, true));
					//}
				} finally {
					ArrayPool<char>.Shared.Return(entryNameBuffer);
				}
			}
		}
		public static void ZipAndroidProjects(this string dir)
		{
			Directory.GetDirectories(dir)
				.ForEach(directory => directory.ZipAndroidProject());
		}
		public static void ZipCSharp(this string dir, string filter = null
		)
		{
			var targetFile = GetZipFileName(dir);
			var encoding = Encoding.GetEncoding("gbk");
			using (ZipArchive archive = ZipFile.Open(targetFile, ZipArchiveMode.Create, encoding)) {
				bool directoryIsEmpty = true;
				var di = new DirectoryInfo(dir);
				var basePath = di.FullName;
				const int DefaultCapacity = 260;
				char[] entryNameBuffer = ArrayPool<char>.Shared.Rent(DefaultCapacity);
				try {
					var list = filter == null ? new DirectoryInfo(dir).GetFileSystemInfos("*", SearchOption.AllDirectories)
                    	.Where(i => !Regex.IsMatch(i.FullName, "\\\\(?:\\.[\\w]+|obj|bin|packages)\\\\"))
                    	: new DirectoryInfo(dir).GetFileSystemInfos("*", SearchOption.AllDirectories)
                    	.Where(i => !Regex.IsMatch(i.FullName, "\\\\(?:\\.git|\\.vs|\\.idea|obj|bin|packages|" + filter + ")\\\\"));
					//                        .Where(i => !i.FullName.Contains("\\packages\\")
//                        && !i.FullName.Contains("\\bin\\")
//                        && !i.FullName.Contains("\\obj\\")
//                          && !i.FullName.Contains("\\.vs\\")
//                          && !i.FullName.Contains("\\.git\\")
//                         && !i.FullName.Contains("\\.idea\\"));

					foreach (var file in list) {
						directoryIsEmpty = false;
						int entryNameLength = file.FullName.Length - basePath.Length;
						if (file is FileInfo) {
							string entryName = EntryFromPath(file.FullName, basePath.Length, entryNameLength, ref entryNameBuffer);
							DoCreateEntryFromFile(archive, file.FullName, entryName, CompressionLevel.Fastest);
						} else {
							DirectoryInfo possiblyEmtpy = file as DirectoryInfo;
							if (possiblyEmtpy != null && possiblyEmtpy.IsDirectoryEmpty()) {
								string entryName = EntryFromPath(file.FullName, basePath.Length, entryNameLength, ref entryNameBuffer, true);
								archive.CreateEntry(entryName);
							}
						}
					}
					//if (directoryIsEmpty)
					//{
					//    archive.CreateEntry(EntryFromPath(di.Name, 0, di.Name.Length, ref entryNameBuffer, true));
					//}
				} finally {
					ArrayPool<char>.Shared.Return(entryNameBuffer);
				}
			}
		}
		public static void ZipDirectories(this string dir)
		{

			var directories = Directory.GetDirectories(dir, "*");
			foreach (var item in directories) {
				var targetFile = Path.Combine(Path.GetDirectoryName(item), Path.GetFileName(item) + ".zip");
				if (File.Exists(targetFile))
					continue;
				ZipFile.CreateFromDirectory(item, targetFile,
					CompressionLevel.Fastest,
					false,
					Encoding.GetEncoding("gbk"));
			}
		}
		public static void ZipDirectory(this string dir)
		{
			var targetFile = Path.Combine(dir.GetDirectoryName(), Path.GetFileName(dir) + "-" + DateTimes.ToUnixTimeMillisecond() + ".zip");
			ZipFile.CreateFromDirectory(dir, targetFile,
				CompressionLevel.Fastest,
				false,
				Encoding.GetEncoding("gbk"));
		}
		public static void ZipNetCore(this string dir, string fileName = null, string filter = null
		)
		{
			var targetFile = fileName ?? GetZipFileName(dir);
			
			var encoding = Encoding.GetEncoding("gbk");
			using (ZipArchive archive = ZipFile.Open(targetFile, ZipArchiveMode.Create, encoding)) {
				bool directoryIsEmpty = true;
				var di = new DirectoryInfo(dir);
				var basePath = di.FullName;
				const int DefaultCapacity = 260;
				char[] entryNameBuffer = ArrayPool<char>.Shared.Rent(DefaultCapacity);
				try {
					var list = filter == null ? new DirectoryInfo(dir).GetFileSystemInfos("*", SearchOption.AllDirectories)
                    	.Where(i => !Regex.IsMatch(i.FullName, "\\\\(?:\\.[\\w]+|obj|bin|Migrations|wwwroot)\\\\"))
                    	: new DirectoryInfo(dir).GetFileSystemInfos("*", SearchOption.AllDirectories)
                    	.Where(i => !Regex.IsMatch(i.FullName, "\\\\(?:\\.git|\\.vs|\\.idea|obj|bin|packages|" + filter + ")\\\\"));
					//                        .Where(i => !i.FullName.Contains("\\packages\\")
//                        && !i.FullName.Contains("\\bin\\")
//                        && !i.FullName.Contains("\\obj\\")
//                          && !i.FullName.Contains("\\.vs\\")
//                          && !i.FullName.Contains("\\.git\\")
//                         && !i.FullName.Contains("\\.idea\\"));

					foreach (var file in list) {
						directoryIsEmpty = false;
						int entryNameLength = file.FullName.Length - basePath.Length;
						if (file is FileInfo) {
							if (file.Name == "appsettings.json")
								continue;
							string entryName = EntryFromPath(file.FullName, basePath.Length, entryNameLength, ref entryNameBuffer);
							DoCreateEntryFromFile(archive, file.FullName, entryName, CompressionLevel.Fastest);
						} else {
							DirectoryInfo possiblyEmtpy = file as DirectoryInfo;
							if (possiblyEmtpy != null && possiblyEmtpy.IsDirectoryEmpty()) {
								string entryName = EntryFromPath(file.FullName, basePath.Length, entryNameLength, ref entryNameBuffer, true);
								archive.CreateEntry(entryName);
							}
						}
					}
					//if (directoryIsEmpty)
					//{
					//    archive.CreateEntry(EntryFromPath(di.Name, 0, di.Name.Length, ref entryNameBuffer, true));
					//}
				} finally {
					ArrayPool<char>.Shared.Return(entryNameBuffer);
				}
			}
		}
		public static string ZipNode(this string dir, string filter = null
		)
		{
			var targetFile = GetZipFileName(dir);
			var encoding = Encoding.GetEncoding("gbk");
			using (ZipArchive archive = ZipFile.Open(targetFile, ZipArchiveMode.Create, encoding)) {
				bool directoryIsEmpty = true;
				var di = new DirectoryInfo(dir);
				var basePath = di.FullName;
				const int DefaultCapacity = 260;
				char[] entryNameBuffer = ArrayPool<char>.Shared.Rent(DefaultCapacity);
				try {
					var list = filter == null ? new DirectoryInfo(dir).GetFileSystemInfos("*", SearchOption.AllDirectories)
                    	.Where(i => !Regex.IsMatch(i.FullName, "\\\\(?:\\.[\\w]+|node_modules)\\\\"))
                    	: new DirectoryInfo(dir).GetFileSystemInfos("*", SearchOption.AllDirectories)
                    	.Where(i => !Regex.IsMatch(i.FullName, "\\\\(?:\\.[\\w]+|node_modules|" + filter + ")\\\\"));
					//                        .Where(i => !i.FullName.Contains("\\packages\\")
//                        && !i.FullName.Contains("\\bin\\")
//                        && !i.FullName.Contains("\\obj\\")
//                          && !i.FullName.Contains("\\.vs\\")
//                          && !i.FullName.Contains("\\.git\\")
//                         && !i.FullName.Contains("\\.idea\\"));

					foreach (var file in list) {
						directoryIsEmpty = false;
						int entryNameLength = file.FullName.Length - basePath.Length;
						if (file is FileInfo) {
							string entryName = EntryFromPath(file.FullName, basePath.Length, entryNameLength, ref entryNameBuffer);
							DoCreateEntryFromFile(archive, file.FullName, entryName, CompressionLevel.Fastest);
						} else {
							DirectoryInfo possiblyEmtpy = file as DirectoryInfo;
							if (possiblyEmtpy != null && possiblyEmtpy.IsDirectoryEmpty()) {
								string entryName = EntryFromPath(file.FullName, basePath.Length, entryNameLength, ref entryNameBuffer, true);
								archive.CreateEntry(entryName);
							}
						}
					}
					//if (directoryIsEmpty)
					//{
					//    archive.CreateEntry(EntryFromPath(di.Name, 0, di.Name.Length, ref entryNameBuffer, true));
					//}
				} finally {
					ArrayPool<char>.Shared.Return(entryNameBuffer);
				}
			}
			return targetFile;
		}
		
		
		public static string ZipSrt(this string dir	)
		{
			var targetFile = GetZipFileName(dir);
			var encoding = Encoding.GetEncoding("gbk");
			using (ZipArchive archive = ZipFile.Open(targetFile, ZipArchiveMode.Create, encoding)) {
				bool directoryIsEmpty = true;
				var di = new DirectoryInfo(dir);
				var basePath = di.FullName;
				const int DefaultCapacity = 260;
				char[] entryNameBuffer = ArrayPool<char>.Shared.Rent(DefaultCapacity);
				try {
					var list = new DirectoryInfo(dir).GetFileSystemInfos("*", SearchOption.AllDirectories)
						.Where(i => Regex.IsMatch(i.FullName, "\\.(?:srt)$)"));

					foreach (var file in list) {
						directoryIsEmpty = false;
						int entryNameLength = file.FullName.Length - basePath.Length;
						if (file is FileInfo) {
							string entryName = EntryFromPath(file.FullName, basePath.Length, entryNameLength, ref entryNameBuffer);
							DoCreateEntryFromFile(archive, file.FullName, entryName, CompressionLevel.Fastest);
						} else {
							DirectoryInfo possiblyEmtpy = file as DirectoryInfo;
							if (possiblyEmtpy != null && possiblyEmtpy.IsDirectoryEmpty()) {
								string entryName = EntryFromPath(file.FullName, basePath.Length, entryNameLength, ref entryNameBuffer, true);
								archive.CreateEntry(entryName);
							}
						}
					}
					//if (directoryIsEmpty)
					//{
					//    archive.CreateEntry(EntryFromPath(di.Name, 0, di.Name.Length, ref entryNameBuffer, true));
					//}
				} finally {
					ArrayPool<char>.Shared.Return(entryNameBuffer);
				}
			}
			return targetFile;
		}
	}
}