namespace Common
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading;
	
	public static  class Files
	{
		internal static readonly char[] InvalidPathChars = {
			'\"', '<', '>', '|', '\0',
			(char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8, (char)9, (char)10,
			(char)11, (char)12, (char)13, (char)14, (char)15, (char)16, (char)17, (char)18, (char)19, (char)20,
			(char)21, (char)22, (char)23, (char)24, (char)25, (char)26, (char)27, (char)28, (char)29, (char)30,
			(char)31
		};
		public static readonly char VolumeSeparatorChar = ':';
 
		public static readonly char DirectorySeparatorChar = '\\';
		internal const string DirectorySeparatorCharAsString = "\\";
		public static readonly char AltDirectorySeparatorChar = '/';
 
		 
		public static bool AnyPathHasIllegalCharacters(this string path, bool checkAdditional = false)
		{
			return path.IndexOfAny(InvalidPathChars) >= 0 || (checkAdditional && AnyPathHasWildCardCharacters(path));
		}
		public static String GetFileNameWithoutExtension(this String path)
		{
			path = GetFileName(path);
			if (path != null) {
				int i;
				if ((i = path.LastIndexOf('.')) == -1)
					return path; // No path extension found
                else
					return path.Substring(0, i);
			}
			return null;
		}public static bool IsDirectoryEmpty(this DirectoryInfo possiblyEmptyDirectory)
		{
			using (var enumerator = Directory.EnumerateFileSystemEntries(possiblyEmptyDirectory.FullName).GetEnumerator()) {
				return !enumerator.MoveNext();
			}
		}
		public static String ChangeFileNameWithoutExtension(this String path,string fileName)
		{
			 
			return Path.Combine(Path.GetDirectoryName(fileName),fileName+Path.GetExtension(fileName));
		}
		internal static bool AnyPathHasWildCardCharacters(string path, int startIndex = 0)
		{
			char currentChar;
			for (int i = startIndex; i < path.Length; i++) {
				currentChar = path[i];
				if (currentChar == '*' || currentChar == '?')
					return true;
			}
			return false;
		}
		public static SHA256 CreateSHA256()
		{
			try {
				return SHA256.Create();
			}
            // SHA256.Create is documented to throw this exception on FIPS compliant machines.
            // See: https://msdn.microsoft.com/en-us/library/z08hz7ad%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
            catch (System.Reflection.TargetInvocationException) {
				// Fallback to a FIPS compliant SHA256 algorithm.
				return new SHA256CryptoServiceProvider();
			}
		}
		public static string Base64UrlEncode(byte[] input)
		{
			
			return Base64UrlEncode(input, offset: 0, count: input.Length);
		}

		public static string GetHashForFile(this string fileInfo)
		{
			using (var sha256 = CreateSHA256()) {
				using (var readStream = new FileStream(fileInfo, FileMode.Open)) {
					var hash = sha256.ComputeHash(readStream);
					return Base64UrlEncode(hash);
				}
			}
		}
		public static int GetArraySizeRequiredToEncode(int count)
		{
			var numWholeOrPartialInputBlocks = checked(count + 2) / 3;
			return checked(numWholeOrPartialInputBlocks * 4);
		}
		public static string Base64UrlEncode(byte[] input, int offset, int count)
		{
           
			var buffer = new char[GetArraySizeRequiredToEncode(count)];
			var numBase64Chars = Base64UrlEncode(input, offset, buffer, outputOffset: 0, count: count);

			return new String(buffer, startIndex: 0, length: numBase64Chars);
		}
		public static int Base64UrlEncode(byte[] input, int offset, char[] output, int outputOffset, int count)
		{
            
			var arraySizeRequired = GetArraySizeRequiredToEncode(count);
			if (output.Length - outputOffset < arraySizeRequired) {
				throw new ArgumentException();
			}

			// Special-case empty input.
			if (count == 0) {
				return 0;
			}

			// Use base64url encoding with no padding characters. See RFC 4648, Sec. 5.

			// Start with default Base64 encoding.
			var numBase64Chars = Convert.ToBase64CharArray(input, offset, count, output, outputOffset);

			// Fix up '+' -> '-' and '/' -> '_'. Drop padding characters.
			for (var i = outputOffset; i - outputOffset < numBase64Chars; i++) {
				var ch = output[i];
				if (ch == '+') {
					output[i] = '-';
				} else if (ch == '/') {
					output[i] = '_';
				} else if (ch == '=') {
					// We've reached a padding character; truncate the remainder.
					return i - outputOffset;
				}
			}

			return numBase64Chars;
		}
  
		public static bool FileExists(this string path)
		{
			return File.Exists(path);
		}
		public static string Combine(this string dir, string name)
		{
			return Path.Combine(dir, name);
		}
		public static bool CreateDirectoryIfNotExists(this string dir)
		{
			if (Directory.Exists(dir)) {
				return true;
			}
			return Directory.CreateDirectory(dir).Exists;
		}
		private static volatile Encoding _UTF8NoBOM;
		static Encoding UTF8NoBOM {
			get { 
				if (_UTF8NoBOM == null) {
					// No need for double lock - we just want to avoid extra
					// allocations in the common case.
					UTF8Encoding noBOM = new UTF8Encoding(false, true);
					Thread.MemoryBarrier();
					_UTF8NoBOM = noBOM;
				}
				return _UTF8NoBOM;
			}
		}
		public static string GetDesktopPath(this string f)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), f);
		}
		public static void WriteAllLines(this string path, IEnumerable<String> contents)
		{
			 
			var writer =	new StreamWriter(path, false, UTF8NoBOM);
			using (writer) {
				foreach (String line in contents) {
					writer.WriteLine(line);
				}
			}
		}
		public static string GetExecutingPath(this string f)
		{
			return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), f);
		}
		public static string GetDirectoryName(this string path)
		{
			return Path.GetDirectoryName(path);
		}
		public static String GetExtension(this String path)
		{
			if (path == null)
				return null;
 
			int length = path.Length;
			for (int i = length; --i >= 0;) {
				char ch = path[i];
				if (ch == '.') {
					if (i != length - 1)
						return path.Substring(i, length - i);
					else
						return String.Empty;
				}
				if (ch == DirectorySeparatorChar || ch == AltDirectorySeparatorChar || ch == VolumeSeparatorChar)
					break;
			}
			return String.Empty;
		}
		public static String GetFileName(this String path)
		{
			if (path != null) {
    
				int length = path.Length;
				for (int i = length; --i >= 0;) {
					char ch = path[i];
					if (ch == DirectorySeparatorChar || ch == AltDirectorySeparatorChar || ch == VolumeSeparatorChar)
						return path.Substring(i + 1, length - i - 1);
 
				}
			}
			return path;
		}
		public static string GetValidFileName(this string value, char c = ' ')
		{

			var chars = Path.GetInvalidFileNameChars();

			return new string(value.Select<char, char>((i) => {
				if (chars.Contains(i))
					return c;
				return i;
			}).Take(125).ToArray()).Trim();
		}
		
		public static void WriteAllText(this String path, String contents)
		{
			using (var sw = new StreamWriter(path, false, new UTF8Encoding(false)))
				sw.Write(contents);
		}
	}
}