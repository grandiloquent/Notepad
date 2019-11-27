namespace StringCompare
{
	using System.Security.Cryptography;
	using Microsoft.Ajax.Utilities;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using Common;
	
	
	public static class OtherDelegate
	{
		private	static string[] Extensions = {
			".c",
			".cc",
			".cpp",
			".cs",
			".go",
			".h",
			".hpp",
			".java",
			".js",
			".kt",
			".md",
			".mod",
			".s",
			".sh",
			".ts",
			".xaml",
			".xml"
		};
		
		
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

		
		public static string Base64UrlEncode(byte[] input, int offset, int count)
		{
           
			var buffer = new char[GetArraySizeRequiredToEncode(count)];
			var numBase64Chars = Base64UrlEncode(input, offset, buffer, outputOffset: 0, count: count);

			return new String(buffer, startIndex: 0, length: numBase64Chars);
		}

		public static string Base64UrlEncode(byte[] input)
		{
			
			return Base64UrlEncode(input, offset: 0, count: input.Length);
		}

		public static void CombineDirectoryInternal(string dir)
		{
			var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories)
								.Where(i => Extensions.Contains(i.GetExtension().ToLower()));
			var sb = new StringBuilder();
			sb.AppendLine("/*")
								.AppendLine(files.Select(i => i.GetFileNameWithoutExtension()).OrderBy(i => i).ConcatenateLines())
								.AppendLine("*/")
								.AppendLine()
								.AppendLine();
			foreach (var element in files) {
				// element.GetFileName() == "CMakeLists.txt"
				var s = element.ReadAllText();
				while (s.TrimStart().StartsWith("/*")) {
					s = s.SubstringAfter("*/");
				}
				sb.AppendLine()
										.AppendLine()
										.AppendLine("# =====================================")
										.AppendLine("# " + element)
										.AppendLine("# =====================================")
										.AppendLine()
										.AppendLine()
										.AppendLine(s);
				;
			}
			(dir.GetFileName() + ".txt").GetDesktopPath().WriteAllText(sb.ToString());
		}

		public static void CombineSubDirectory(string dir)
		{
			var subDirectories = Directory.GetDirectories(dir);
			foreach (var subDirectory in subDirectories) {
				var files = Directory.GetFiles(subDirectory, "*", SearchOption.AllDirectories)
								.Where(i => Extensions.Contains(i.GetExtension().ToLower()));
				var sb = new StringBuilder();
				sb.AppendLine("/*")
								.AppendLine(files.Select(i => i.GetFileNameWithoutExtension()).OrderBy(i => i).ConcatenateLines())
								.AppendLine("*/")
								.AppendLine()
								.AppendLine();
				foreach (var element in files) {
					// element.GetFileName() == "CMakeLists.txt"
					var s = element.ReadAllText();
					while (s.TrimStart().StartsWith("/*")) {
						s = s.SubstringAfter("*/");
					}
					sb.AppendLine()
										.AppendLine()
										.AppendLine("# =====================================")
										.AppendLine("# " + element)
										.AppendLine("# =====================================")
										.AppendLine()
										.AppendLine()
										.AppendLine(s);
					;
				}
				Directory.Delete(subDirectory, true);
				(dir + "\\" + subDirectory.GetFileName() + ".txt").WriteAllText(sb.ToString());
			}
		}

		public static void CombineSubSubDirectory(string dir)
		{
			var directories = Directory.GetDirectories(dir);
			foreach (var directory in directories) {
				var subDirectories = Directory.GetDirectories(directory);
				foreach (var subDirectory in subDirectories) {
						
					var files = Directory.GetFiles(subDirectory, "*", SearchOption.AllDirectories)
								.Where(i => Extensions.Contains(i.GetExtension().ToLower()));
					var sb = new StringBuilder();
					sb.AppendLine("/*")
								.AppendLine(files.Select(i => i.GetFileNameWithoutExtension()).OrderBy(i => i).ConcatenateLines())
								.AppendLine("*/")
								.AppendLine()
								.AppendLine();
					foreach (var element in files) {
						// element.GetFileName() == "CMakeLists.txt"
						var s = element.ReadAllText();
						while (s.TrimStart().StartsWith("/*")) {
							s = s.SubstringAfter("*/");
						}
						sb.AppendLine()
										.AppendLine()
										.AppendLine("# =====================================")
										.AppendLine("# " + element)
										.AppendLine("# =====================================")
										.AppendLine()
										.AppendLine()
										.AppendLine(s);
						;
					}
					Directory.Delete(subDirectory, true);
					(directory + "\\" + subDirectory.GetFileName() + ".txt").WriteAllText(sb.ToString());
				}
					
			}
			               
		}

		public static void CommandOCR(string f)
		{
			const string fileName = @"C:\Program Files\Tesseract-OCR\tesseract.exe";
			Process.Start(new ProcessStartInfo() {
				WorkingDirectory = Path.GetDirectoryName(f),
				FileName = fileName,
				Arguments = string.Format("--tessdata-dir \"C:\\Program Files\\Tesseract-OCR\\tessdata\" \"{0}\" {1} -l chi_sim", f, f.GetFileNameWithoutExtension()),
			});
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

  
		public static int GetArraySizeRequiredToEncode(int count)
		{
			var numWholeOrPartialInputBlocks = checked(count + 2) / 3;
			return checked(numWholeOrPartialInputBlocks * 4);
		}

		public static string GetHashForFile(string fileInfo)
		{
			using (var sha256 = CreateSHA256()) {
				using (var readStream = new FileStream(fileInfo, FileMode.Open)) {
					var hash = sha256.ComputeHash(readStream);
					return Base64UrlEncode(hash);
				}
			}
		}

		
		
		[BindMenuItem(Name = "合并目录", Control = "codeButton", Toolbar = "toolbar3")]
		public static void CombineDirectory()
		{
			Methods.OnClipboardDirectory(CombineDirectoryInternal);
		}
		
	
	
		[BindMenuItem(Name = "打开当前目录", Control = "commandSplitButton", Toolbar = "toolbar3")]
		public static void Run()
		{
			Process.Start(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
		}
		
		[BindMenuItem(Name = "创建", Control = "fileButton", Toolbar = "toolbar3")]
		public static void CreateFile()
		{
			Methods.OnClipboardText(v => {
				File.Create(v.GetValidFileName().GetDesktopPath()).Close();
			});
		}
	}
}