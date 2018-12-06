namespace Shared
{
	using System;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	public static  class FileExtensions
	{
		private static readonly char[] InvalidFileNameChars = {
			'\"',
			'<',
			'>',
			'|',
			'\0',
			':',
			'*',
			'?',
			'\\',
			'/'
		};
		public static UTF8Encoding sUTF8Encoding = new UTF8Encoding(false);
		public static void AppendAllLines(this String path, IEnumerable<String> contents, Encoding encoding)
		{
			File.AppendAllLines(path, contents, sUTF8Encoding);
		}
		public static void AppendAllText(this String path, String contents)
		{
			File.AppendAllText(path, contents, sUTF8Encoding);
		}
		public static StreamWriter AppendText(this String path)
		{
			return  File.AppendText(path);
		}
		public static String ChangeExtension(this String path, String extension)
		{
			return  Path.ChangeExtension(path, extension);
		}
		public static string ChangeFileName(this string path, string fileNameWithoutExtension)
		{
			return Path.Combine(Path.GetDirectoryName(path), fileNameWithoutExtension + Path.GetExtension(path));
		}
		public static void ClearEmptyLinesInDirectory(this string dir)
		{
			var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
			var regex = new Regex("\\.(?:java|kt|xml|txt|js|cs|c|h)$");
			foreach (var element in files) {
				if (regex.IsMatch(element)) {
					element.WriteAllText(element.ReadAllText().RemoveEmptyLines());
				}
			}
		}
		public static String Combine(this String path1, String path2)
		{
			return  Path.Combine(path1, path2);
		}
		public static FileStream Create(this String path)
		{
			return  File.Create(path);
		}
		public static void CreateDirectoryIfNotExists(this String path)
		{
			if (Directory.Exists(path))
				return;
			Directory.CreateDirectory(path);
		}
		public static StreamWriter CreateText(this String path)
		{
			return  File.CreateText(path);
		}

		public static void DirectoryDelete(this String path, bool recursive = false)
		{
			if (recursive) {
				Directory.Delete(path, true);
			} else {
				Directory.Delete(path);
			}
		}
		public static bool DirectoryExists(this String path)
		{
			return  Directory.Exists(path);
		}
		public static void FileCopy(this string path, string dstPath)
		{
			File.Copy(path, dstPath, false);
		}
		public static bool FileExists(this String path)
		{
			return  File.Exists(path);
		}
		public static DateTime FileGetCreationTimeUtc(this String path)
		{
			return  File.GetCreationTimeUtc(path);
		}
		public static DateTime FileGetLastAccessTimeUtc(this String path)
		{
			return  File.GetLastAccessTimeUtc(path);
		}
		public static DateTime FileGetLastWriteTimeUtc(this String path)
		{
			return  File.GetLastWriteTimeUtc(path);
		}
		public static void FileMove(this String sourceFileName, String destFileName)
		{
			File.Move(sourceFileName, destFileName);
		}
		public static void FileReplace(this String sourceFileName, String destinationFileName, String destinationBackupFileName)
		{
			File.Replace(sourceFileName, destinationFileName, destinationBackupFileName);
		}
		public static void GbkToUTF8(this string path)
		{
			var content = File.ReadAllText(path, Encoding.GetEncoding("gbk"));
			File.WriteAllText(path, content, new UTF8Encoding(false));
		}

		public static string GetApplicationPath(this string v)
		{
			return Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), v);
		}
		public static String GetCommandPath(this string fileName)
		{
			var dir = System.Reflection.Assembly.GetEntryAssembly().Location.GetDirectoryName();
			return dir.Combine(fileName);
		}
		public static string GetDirectoryFileName(this string v)
		{
			return Path.GetFileName(Path.GetDirectoryName(v));
		}
		public static string GetDirectoryName(this string path)
		{
			return  Path.GetDirectoryName(path);
		}
		public static string GetExePath(this string path)
		{
			return 	Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), path);
			
		}
		public static String GetExtension(this String path)
		{
			return  Path.GetExtension(path);
		}
		public static String GetFileName(this String path)
		{
			return  Path.GetFileName(path);
		}
		public static String GetFileNameWithoutExtension(this String path)
		{
			return  Path.GetFileNameWithoutExtension(path);
		}
		public static IEnumerable<string> GetFiles(this string dir, string pattern, bool bExclude = false)
		{
			if (bExclude)
				return Directory.GetFiles(dir).Where(i => !Regex.IsMatch(i, "\\.(?:" + pattern + ")$"));
			else
				return Directory.GetFiles(dir).Where(i => Regex.IsMatch(i, "\\.(?:" + pattern + ")$"));
			
		}
		public static string GetFileSha1(this string path)
		{
			using (FileStream fs = new FileStream(path, FileMode.Open))
			using (BufferedStream bs = new BufferedStream(fs))
			using (var reader = new StreamReader(bs)) {
				using (System.Security.Cryptography.SHA1Managed sha1 = new System.Security.Cryptography.SHA1Managed()) {
					byte[] hash = sha1.ComputeHash(bs);
					StringBuilder formatted = new StringBuilder(2 * hash.Length);
					foreach (byte b in hash) {
						formatted.AppendFormat("{0:X2}", b);
					}
				}
				return reader.ReadToEnd();
			}
		}
		public static String GetFullPath(this String path)
		{
			return  Path.GetFullPath(path);
		}

		public static string[] GetFiles(this String path, Boolean isAllDirectories = false, string pattern = "")
		{
			if (!isAllDirectories && pattern.IsReadable()) {
				
				// \\.(?:txt|css)$
				return Directory.GetFiles(path, "*").Where(i => Regex.IsMatch(i, pattern)).ToArray();
			}
			if (!isAllDirectories && pattern.IsVacuum()) {
				return Directory.GetFiles(path, "*");
				
			}
			
			if (isAllDirectories && pattern.IsReadable()) {
				
				// \\.(?:txt|css)$
				return Directory.GetFiles(path, "*", SearchOption.AllDirectories).Where(i => Regex.IsMatch(i, pattern)).ToArray();
			}
			if (isAllDirectories && pattern.IsVacuum()) {
				return Directory.GetFiles(path, "*", SearchOption.AllDirectories);
				
			}
			return Directory.GetFiles(path, "*");
			
		}
		public static string[] GetDirectories(this String path, Boolean isAllDirectories = false, string pattern = "")
		{
			if (!isAllDirectories && pattern.IsReadable()) {
				
				// \\.(?:txt|css)$
				return Directory.GetDirectories(path, "*").Where(i => Regex.IsMatch(i, pattern)).ToArray();
			}
			if (!isAllDirectories && pattern.IsVacuum()) {
				return Directory.GetDirectories(path, "*");
				
			}
			
			if (isAllDirectories && pattern.IsReadable()) {
				
				// \\.(?:txt|css)$
				return Directory.GetDirectories(path, "*", SearchOption.AllDirectories).Where(i => Regex.IsMatch(i, pattern)).ToArray();
			}
			if (isAllDirectories && pattern.IsVacuum()) {
				return Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
				
			}
			return Directory.GetDirectories(path, "*");
			
		}
		public static DirectoryInfo GetParent(this String path)
		{
			return  Directory.GetParent(path);
		}
		public static String GetPathRoot(this String path)
		{
			return  Path.GetPathRoot(path);
		}

		public static string GetUniqueFileName(this String v)
		{
			int i = 1;
			Regex regex = new Regex(" \\- [0-9]+");
			String t = Path.Combine(Path.GetDirectoryName(v),
				           regex.Split(Path.GetFileNameWithoutExtension(v), 2).First() + " - " + i.ToString().PadLeft(3, '0') +
				           Path.GetExtension(v));

			while (File.Exists(t)) {
				i++;
				t = Path.Combine(Path.GetDirectoryName(v),
					regex.Split(Path.GetFileNameWithoutExtension(v), 2).First() + " - " + i.ToString().PadLeft(3, '0') +
					Path.GetExtension(v));
			}
			return t;
		}
		public static string GetUniqueImageRandomFileName(this string path)
		{
			var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Select(i => i.GetFileNameWithoutExtension());
			var fileName = 8.GetRandomString();
			while (files.Contains(fileName))
				fileName = 8.GetRandomString();

			return fileName;
		}

		public static string GetValidFileName(this String v)
		{
			if (v == null)
				return null;
			// (Char -> Int) 1-31 Invalid;
			List<char> chars = new List<char>(v.Length);

			for (int i = 0; i < v.Length; i++) {
				if (InvalidFileNameChars.Contains(v[i])) {
					chars.Add(' ');
				} else {
					chars.Add(v[i]);
				}
			}

			return new String(chars.ToArray());
		}
 
		public static string GetValidFileName(this string value, char c)
		{

			var chars = Path.GetInvalidFileNameChars();

			return new string(value.Select<char, char>((i) => {
				if (chars.Contains(i))
					return c;
				return i;
			}).Take(125).ToArray());
		}
		public static FileStream Open(this String path, FileMode mode, FileAccess access)
		{
			return  File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		}
		public static FileStream OpenRead(this String path)
		{
			return  File.OpenRead(path);
		}
		public static StreamReader OpenText(this String path)
		{
			return  File.OpenText(path);
		}
		public static FileStream OpenWrite(this String path)
		{
			return  File.OpenWrite(path);
		}
		public static byte[] ReadAllBytes(this String path)
		{
			return  File.ReadAllBytes(path);
		}
		 
		public static String  ReadAllText(this String path)
		{
           
 
			using (StreamReader sr = new StreamReader(path, new UTF8Encoding(false), true))
				return sr.ReadToEnd();
		}
		public static IEnumerable<string> ReadLines(this String path)
		{
			return  File.ReadLines(path, new UTF8Encoding(false));
		}
		
		public static String[] ReadAllLines(this String path)
		{
          
 
			String line;
			List<String> lines = new List<String>();
 
			using (StreamReader sr = new StreamReader(path, new UTF8Encoding(false)))
				while ((line = sr.ReadLine()) != null)
					lines.Add(line);
 
			return lines.ToArray();
		}
		public static string ToLine(this IEnumerable<string> value, string separator = "\r\n")
		{
			return string.Join(separator, value);
		}

		public static void UTF8ToGbk(this string path)
		{
			var content = File.ReadAllText(path, new UTF8Encoding(false));
			File.WriteAllText(path, content, Encoding.GetEncoding("gbk"));
		}
		public static void WriteAllBytes(this String path, byte[] bytes)
		{
			File.WriteAllBytes(path, bytes);
		}
		public static void WriteAllLines(this String path, IEnumerable<String> contents)
		{
		
				
			using (var writer =	new StreamWriter(path, false, new UTF8Encoding(false))) {
				foreach (String line in contents) {
					writer.WriteLine(line);
				}
			}
		}
		public static void WriteAllText(this String path, String contents)
		{
			using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(false)))
				sw.Write(contents);
		}
	}
}