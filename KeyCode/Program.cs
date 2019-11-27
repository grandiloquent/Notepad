namespace KeyCode
{
	using Microsoft.Ajax.Utilities;
	using Renci.SshNet;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Runtime.InteropServices;
	using System.Security.Cryptography;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	class Program
	{
		private static SshClient _sshClient;
		private const string HostName =
			//"114.115.201.176";
			// "114.115.133.23";
			"106.12.125.201";
		private static string mFileName = null;
		private const string Password = "jhiSBI2s!";
		private const string UserName = "root";
		
		public static string Base64UrlEncode(byte[] input)
		{
			if (input == null) {
				throw new ArgumentNullException("");
			}
			return Base64UrlEncode(input, offset: 0, count: input.Length);
		}
		public static string Base64UrlEncode(byte[] input, int offset, int count)
		{
			if (input == null) {
				throw new ArgumentNullException("");
			}
			// Special-case empty input
			if (count == 0) {
				return string.Empty;
			}
			var buffer = new char[GetArraySizeRequiredToEncode(count)];
			var numBase64Chars = Base64UrlEncode(input, offset, buffer, outputOffset: 0, count: count);
			return new String(buffer, startIndex: 0, length: numBase64Chars);
		}
		public static int Base64UrlEncode(byte[] input, int offset, char[] output, int outputOffset, int count)
		{
			if (input == null) {
				throw new ArgumentNullException("");
			}
			if (output == null) {
				throw new ArgumentNullException("");
			}
			if (outputOffset < 0) {
				throw new ArgumentOutOfRangeException("");
			}
			var arraySizeRequired = GetArraySizeRequiredToEncode(count);
			if (output.Length - outputOffset < arraySizeRequired) {
				throw new ArgumentException(
//                    string.Format(
//                        CultureInfo.CurrentCulture,
//                        EncoderResources.WebEncoders_InvalidCountOffsetOrLength,
//                        nameof(count),
//                        nameof(outputOffset),
//                        nameof(output)),
//                    nameof(count)
				);
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
	 
		public static void CompileTypeScript(string fileName)
		{
			if (!fileName.EndsWith(".ts"))
				return;
			Process.Start(new ProcessStartInfo() {
				FileName = "tsc",
				Arguments = "\"" + fileName + "\""
			});                                                                     
		}
		public static void CompressScripts(string sourceDirectory, string destinationDirectory,
			bool isJavaScript = true)
		{
			var searchPattern = "*.js";
			if (!isJavaScript) {
				searchPattern = "*.css";
			}
			var files = Directory.GetFiles(sourceDirectory, searchPattern).OrderBy(i => Path.GetFileNameWithoutExtension(i));
			var sb = new StringBuilder();
			foreach (var file in files) {
				var n = file.GetFileName();
				if (n.StartsWith(".")) {
					continue;
				}
				if (n.StartsWith("$"))
					continue;
				sb.AppendLine(file.ReadAllText());
			}
			var fileName = "app" + (isJavaScript ? ".js" : ".css");
			var min = new  Minifier();
			if (isJavaScript) {
				var r =	min.MinifyJavaScript(sb.ToString());
				Path.Combine(destinationDirectory, fileName).WriteAllText(r);
			} else {
				var r =	min.MinifyStyleSheet(sb.ToString());
				Path.Combine(destinationDirectory, fileName).WriteAllText(r); 
			}
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
	
		private static void DeleteFiles()
		{
			var files = Clipboard.GetFileDropList();
			foreach (var element in files) {
				if (File.Exists(element))
					File.Delete(element);
				else if (Directory.Exists(element))
					Directory.Delete(element, true);
			}
		}
		private static void DeleteFiles(string directory, Func<string,bool> predict)
		{
			Directory.GetFiles(directory)
				.Where(predict)
				.AsParallel()
				.ForAll(File.Delete);
		}
		public static int GetArraySizeRequiredToEncode(int count)
		{
			var numWholeOrPartialInputBlocks = checked(count + 2) / 3;
			return checked(numWholeOrPartialInputBlocks * 4);
		}
		private static void InitializeCSS()
		{
			InitializeFile();
			HWND h = HWND.Cast(IntPtr.Zero);
			Console.WriteLine("F6 格式化 " + HotKeys.HotKey(h, 117, 0, (int)Keys.F6));
			
			var msg = new MSG();
			var handleRef = new HandleRef(null, IntPtr.Zero);
			while (HotKeys.GetMessageW(ref msg, handleRef, 0, 0)) {
				if (msg.message == HotKeys.WM_HOTKEY) {
					var k = ((int)msg.lParam >> 16) & 0xFFFF;
					var m = (int)msg.lParam & 0xFFFF;
					if (m == 1) {
						if (HandleFile(k, 2)) {
						}
					}
					
				}
			}
		}
		private static string GetHashForFile(string fileInfo)
		{
			using (var sha256 = CreateSHA256()) {
				using (var readStream = File.OpenRead(fileInfo)) {
					var hash = sha256.ComputeHash(readStream);
					return Base64UrlEncode(hash);
				}
			}
		}
		private static bool HandleFile(int key, int mode)
		{
			switch (key) {
				case 70:
					{
						Utils.ClipboardDirectory(dir => {
						                         
							var toc = Safari.GenerateToc1(dir);
							Clipboard.SetText(toc);
						});
					
						break;
					}
				case 71:
					{
						Utils.ClipboardDirectory(Safari.FormatFileNamesBaseToc);
					
						break;
					}
				case 72:
					{
						var extensions = new string[] {
							".c",
							".cs",
							".xaml",
							".cc",
							".cpp",
							".h",
							".hpp",
							".js",
							".ts",
							".java",
							".kt",
							".md",
							".xml"
						};
						Utils.ClipboardDirectory(dir => {
							var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories)
								.Where(i => extensions.Contains(i.GetExtension().ToLower()));
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
						});
						//Utils.ClipboardDirectory(C.CopyDirectory);
						return true;	
					}
				 
				case 75:
					{
						Utils.ClipboardText(s => {
							if (Regex.IsMatch(s, "^\\d+\\-\\d+$")) {
								var parts = s.Split('-');
								for (int i = int.Parse(parts[0]); i < int.Parse(parts[1]) + 1; i++) {
									var f = i.ToString().GetValidFileName();
									File.WriteAllText((f + ".cpp").GetDesktopPath(), "");
									File.WriteAllText((f + ".h").GetDesktopPath(), "");
								}
							} else {
								var f = s.GetValidFileName();
								if (mode == 1) {
									File.WriteAllText((f + ".c").GetDesktopPath(), "");
									File.WriteAllText((f + ".cpp").GetDesktopPath(), "");
									File.WriteAllText((f + ".h").GetDesktopPath(), "");
								} else {
									File.WriteAllText((f + ".css").GetDesktopPath(), "");
									File.WriteAllText((f + ".js").GetDesktopPath(), "");
									
								}
							}
						});
						return true;
					}
				case 76:
					{
						Utils.ClipboardDirectory(Androids.MoveFilesInDirectories);
						return true;
					}
			}
			return false;
		}
		private static void InitializeAndroid()
		{
			HWND h = HWND.Cast(IntPtr.Zero);
			//Console.WriteLine("F1 移除重复书籍 " + HotKeys.HotKey(h, 112, 0, (int)Keys.F1));
			Console.WriteLine("F6 在资源文件中模糊查找 " + HotKeys.HotKey(h, 117, 0, (int)Keys.F6));
			Console.WriteLine("F7 检查未下载成功的页面 " + HotKeys.HotKey(h, 118, 0, (int)Keys.F7));
			Console.WriteLine("F9 生成Log " + HotKeys.HotKey(h, 120, 0, (int)Keys.F9));
			Console.WriteLine("Alt+Q 复制资源文件 " + HotKeys.HotKey(h, 1181, 1, (int)Keys.Q));
			Console.WriteLine("Alt+W 排序资源文件 " + HotKeys.HotKey(h, 1187, 1, (int)Keys.W));
			Console.WriteLine("Ctrl+T 生成记录(类) " + HotKeys.HotKey(h, 284, 2, (int)Keys.T));
			Console.WriteLine("Ctrl+R 格式化字符串常量 " + HotKeys.HotKey(h, 282, 2, (int)Keys.R));
			Console.WriteLine("Ctrl+E 生成记录(变量) " + HotKeys.HotKey(h, 269, 2, (int)Keys.E));
			Console.WriteLine("Ctrl+W 生成记录(字段) " + HotKeys.HotKey(h, 287, 2, (int)Keys.W));
			Console.WriteLine("Ctrl+Q 生成记录(参数值) " + HotKeys.HotKey(h, 281, 2, (int)Keys.Q));
			Console.WriteLine("Ctrl+M 反编译 " + HotKeys.HotKey(h, 277, 2, (int)Keys.M));
			Console.WriteLine("Ctrl+L 字符串到UTF8数组 " + HotKeys.HotKey(h, 276, 2, (int)Keys.L));
			var msg = new MSG();
			var handleRef = new HandleRef(null, IntPtr.Zero);
			while (HotKeys.GetMessageW(ref msg, handleRef, 0, 0)) {
				if (msg.message == HotKeys.WM_HOTKEY) {
					var k = ((int)msg.lParam >> 16) & 0xFFFF;
					var m = (int)msg.lParam & 0xFFFF;
					if (m == 1) {
						switch (k) {
						
							case 81://q
								Utils.ClipboardFiles(Utils.CopySameLevelFiles);
								break;
							case 87://q
								Utils.ClipboardFile(f => {
									if (f.EndsWith(".xml")) {
										f.WriteAllText(Androids.OrderAndroidResource(f.ReadAllText()));
									}
								});
								break;
						}
					}
					if (m == 2) {
						switch (k) {
							case 69://e
								Utils.ClipboardString(Androids.LogVariables);
								break;
							case 76:
								Utils.ClipboardString(s => string.Join(",", new UTF8Encoding(false).GetBytes(s).Select(i => i.ToString()))
								+ "\r\n"
								+ string.Join(",", Encoding.GetEncoding("gbk").GetBytes(s).Select(i => i.ToString())));
								break;
							case 77://m
								Utils.ClipboardFile(file => {
									if (file.EndsWith(".apk")) {
										Androids.ExtractApk(file);
									} else if (file.EndsWith(".dex")) {
										Androids.ExtractJar(file);
									}
								});
								break;
							case 81://q
								Utils.ClipboardString(Androids.LogParameters);
								break;
							case 82://R
								Utils.ClipboardString(Androids.FormatConstStringFields);
								break;
							case 84://T
								Utils.ClipboardString(Androids.GenerateAndroidLog);
								break;
							case 87://W
								Utils.ClipboardString(Androids.LogFields);
								break;
						}
					} else {
						switch (k) {
							case 117:
								Utils.ClipboardString(Androids.SearchInAndroidResourcesFuzzyUsePublic);
								break;
							case 118:
								Utils.ClipboardDirectory(Safari.ExecuteCheckUnDownloadLink);
								break;
							
							case 120:
								Utils.ClipboardString(Androids.LogParameters);
								//Utils.ClipboardString(Androids.GenerateInterfaceFromClass);
								break;
						}
					}
				}
			}
		}
		private static void InitializeC()
		{
			InitializeFile();
			HWND h = HWND.Cast(IntPtr.Zero);
			Console.WriteLine("F6 格式化 " + HotKeys.HotKey(h, 117, 0, (int)Keys.F6));
			Console.WriteLine("F5 编译 " + HotKeys.HotKey(h, 116, 0, (int)Keys.F5));
			Console.WriteLine("F4 " + HotKeys.HotKey(h, 115, 0, (int)Keys.F4));
			Console.WriteLine("Ctrl+W 清空文件变量 " + HotKeys.HotKey(h, 287, 2, (int)Keys.W));
			Console.WriteLine("Ctrl+E 收集文件名 " + HotKeys.HotKey(h, 269, 2, (int)Keys.E));
			Console.WriteLine("Ctrl+Q 排序方法 " + HotKeys.HotKey(h, 281, 2, (int)Keys.Q));
			// Console.WriteLine("F6 " + HotKeys.HotKey(h, 117, 0, (int)Keys.F6));
//			Console.WriteLine("F7 检查未下载成功的页面 " + HotKeys.HotKey(h, 118, 0, (int)Keys.F7));
			Console.WriteLine("F8 格式化评注 " + HotKeys.HotKey(h, 119, 0, (int)Keys.F8));
			Console.WriteLine("F9 命令2 " + HotKeys.HotKey(h, 120, 0, (int)Keys.F9));
//			
//			
			var msg = new MSG();
			var handleRef = new HandleRef(null, IntPtr.Zero);
			while (HotKeys.GetMessageW(ref msg, handleRef, 0, 0)) {
				if (msg.message == HotKeys.WM_HOTKEY) {
					var k = ((int)msg.lParam >> 16) & 0xFFFF;
					var m = (int)msg.lParam & 0xFFFF;
					if (m == 1) {
						if (HandleFile(k, 1)) {
						}
					}
					if (m == 2) {
						switch (k) {
							case 69:
								Utils.ClipboardDirectory(dir => {
									var files = Directory.GetFiles(dir);
									var ls = new List<string>();
									foreach (var element in files) {
										var extension = element.GetExtension();
										if (extension == ".c" || extension == ".cc"
										    || extension == ".cpp") {
											ls.Add(Path.GetFileName(element));
										}
									}
									Clipboard.SetText(string.Join("\n", ls.OrderBy(i => Path.GetExtension(i))
								                         	                              .ThenBy(i => Path.GetFileName(i))
									));
								});
								break;
							case 81:
								Utils.ClipboardString(C.FormatCode);
								break;
							case 87:
								mFileName = null;
								break;
						}
					} else {
						switch (k) {
							case 115:
								C.Command1();
								break;
							case 119:
								Utils.ClipboardString(C.FormatAndroidComments);
								break;
							case 117:
								
								bool skip = false;
								Utils.ClipboardFile(f => {
									var extension = f.GetExtension().ToLower();
									if (extension == ".c" || extension == ".cc" || extension == ".h"
									    || extension == ".cpp" || extension == ".hpp" || extension == ".js") {
										C.Foramt(f);
										skip = true;
										mFileName = f;
									}
								});
								if (!skip && mFileName != null) {
									C.Foramt(mFileName);
								}
								break;
							case 120:
								C.Command2();
								break;
						}
					}
				}
			}
		}
		private static void InitializeFile()
		{
			HWND h = HWND.Cast(IntPtr.Zero);
			Console.WriteLine("Alt+F " + HotKeys.HotKey(h, 1170, 1, (int)Keys.F));
			
			Console.WriteLine("Alt+G " + HotKeys.HotKey(h, 1171, 1, (int)Keys.G));
			
			Console.WriteLine("Alt+H 复制目录 " + HotKeys.HotKey(h, 1172, 1, (int)Keys.H));
			Console.WriteLine("Alt+J 压缩Android项目 " + HotKeys.HotKey(h, 1174, 1, (int)Keys.J));
			Console.WriteLine("Alt+K 创建文件 " + HotKeys.HotKey(h, 1175, 1, (int)Keys.K));
			Console.WriteLine("Alt+L 移动代码文件 " + HotKeys.HotKey(h, 1176, 1, (int)Keys.L));
		}
		private static void InitializeSafari()
		{
			HWND h = HWND.Cast(IntPtr.Zero);
			Console.WriteLine("F1 移除重复书籍 " + HotKeys.HotKey(h, 112, 0, (int)Keys.F1));
			Console.WriteLine("F2 压缩子目录 " + HotKeys.HotKey(h, 113, 0, (int)Keys.F2));
			Console.WriteLine("F5 格式化文件 " + HotKeys.HotKey(h, 116, 0, (int)Keys.F5)); 
			Console.WriteLine("F8 下载书籍 " + HotKeys.HotKey(h, 119, 0, (int)Keys.F8));
			Console.WriteLine("F9 搜索页 " + HotKeys.HotKey(h, 120, 0, (int)Keys.F9));
			Console.WriteLine("Alt+P " + HotKeys.HotKey(h, 1180, 1, (int)Keys.P));
			var msg = new MSG();
			var handleRef = new HandleRef(null, IntPtr.Zero);
			while (HotKeys.GetMessageW(ref msg, handleRef, 0, 0)) {
				if (msg.message == HotKeys.WM_HOTKEY) {
					var k = ((int)msg.lParam >> 16) & 0xFFFF;
					var m = (int)msg.lParam & 0xFFFF;
					if (m == 1) {
						switch (k) {
							case 80:
								Utils.ClipboardDirectory(Mobis.PrettyName);
								break;
						}
					} else {
						switch (k) {
								
							case 112:
								var found = false;
								Utils.ClipboardDirectory(dir => {
									var styles = Path.Combine(dir, "style.txt").ReadAllText();
									foreach (var element in Directory.GetFiles(dir,"*.html")) {
										element.WriteAllText(element.ReadAllText().Replace("<head>", "<head>\r\n" + styles));
									}
									found = true;
								});
								if (found)
									break;
								Utils.ClipboardString(text => {
									var matches =	Regex.Matches(text, "<style[\\s\\S]*?</style>").Cast<Match>().Select(i => i.Value);
								
									return string.Join("\r\n", matches).Replace("#sbo-rt-content", "").Replace("}", "}\r\n\r\n");
								});
								break;
							case 113:
								Utils.ClipboardDirectory(Safari.CreateFromDirectories);
								break;
						
							
							case 116:
								Utils.ClipboardDirectory(Safari.ExecuteFormatFiles);
								break;
							
							case 119:
								Utils.ClipboardDirectory(Safari.DownloadBooks);
								break;
							case 120:
								Utils.ClipboardText(Safari.ExtractParseSearch);
								break;
						}	
					}
				}
			}
		}
		[STAThread]
		static void Main()
		{
			/*var dirDocument = Clipboard.GetFileDropList()[0];
			if (!Directory.Exists(dirDocument))
				return;
			var targetDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Android");
			targetDirectory.CreateDirectoryIfNotExists();
//			targetDirectory = Path.Combine(targetDirectory, dirDocument.GetFileName());
//			targetDirectory.CreateDirectoryIfNotExists();
//			try {
//				var files = Directory.GetFiles(dirDocument, "*.html", SearchOption.AllDirectories);
//				targetDirectory = Path.Combine(targetDirectory, dirDocument.GetFileName());
//				targetDirectory.CreateDirectoryIfNotExists();
//			 
//				foreach (var element in files) {
//					if (element.GetFileName() == "index.html")
//						continue;
//					Utils.FormatAndroidDocuments(element, targetDirectory);
//				}
//			} catch {
//			}
//			
			foreach (var sub in Directory.GetDirectories(dirDocument)) {
				try {
					var files = Directory.GetFiles(sub, "*.html", SearchOption.AllDirectories);
					var	t = Path.Combine(targetDirectory, dirDocument.GetFileName());
					t.CreateDirectoryIfNotExists();
					t = Path.Combine(t, sub.GetFileName());
					t.CreateDirectoryIfNotExists();
					foreach (var element in files) {
						if (element.GetFileName() == "index.html")
							continue;
						Utils.FormatAndroidDocuments(element, t);
					}
				} catch {
				}
			}*/
			//InitializeCSS();
			
			//InitializeC();
			//InitializeAndroid();
			InitializeSafari();
		}
		private static void PublishScript(string sourceFile, string destinationDirectory, string  templateFile)
		{
			var destinationFileName = Path.Combine(destinationDirectory, "app.js");
			var process =	Process.Start(new ProcessStartInfo() {
				FileName = "tsc",
				Arguments = "\"" + sourceFile + "\" --outFile \"" + destinationFileName + "\""
			});     
			process.WaitForExit();
			var min = new Minifier();
			var result = min.MinifyJavaScript(destinationFileName.ReadAllText());
			destinationFileName = Path.Combine(destinationDirectory, "app_v_" + GetHashForFile(destinationFileName) + ".min.js");
			destinationFileName.WriteAllText(result);
			var str = Regex.Replace(templateFile.ReadAllText(), 
				          "(?<=src=\"/static/)"
				          + "app_v_"
				          + "[\\.\\w\\-]+(?=\">)", Path.GetFileName(destinationFileName));
			templateFile.WriteAllText(str);
			_sshClient.RunCommand("systemctl start psychogo");
		}
		private static void PublishStyle(string sourceDirectory, string destinationDirectory, string  templateFile)
		{
			DeleteFiles(destinationDirectory, fileName => fileName.StartsWith("app_v_"));
			var files = 
				Directory.GetFiles(sourceDirectory, "*.css")
				.OrderBy(i => Path.GetFileNameWithoutExtension(i));
			var sb = new StringBuilder();
			foreach (var file in files) {
				var n = file.GetFileName();
				if (n.StartsWith(".")) {
					continue;
				}
				if (n.StartsWith("$"))
					continue;
				sb.AppendLine(file.ReadAllText());
			}
			var min = new  Minifier();
			var r =	min.MinifyStyleSheet(sb.ToString());
			var destinationFileName = string.Empty;
			using (var sha256 = CreateSHA256()) {
				using (var readStream = new MemoryStream()) {
					var buffer = new UTF8Encoding(false).GetBytes(r);
					readStream.Write(buffer, 0, buffer.Length);
					var hash = sha256.ComputeHash(readStream);
					destinationFileName = "app_v_" + Base64UrlEncode(hash) + ".min.css";
				}
			}
			destinationFileName = Path.Combine(destinationDirectory, destinationFileName);
			destinationFileName.WriteAllText(r);
			var str = Regex.Replace(templateFile.ReadAllText(), 
				          "(?<=rel=\"stylesheet\" href=\"/static/)" + "app_v_" + "[\\.\\-\\w]+(?=\">)", Path.GetFileName(destinationFileName));
			templateFile.WriteAllText(str);
			_sshClient.RunCommand("systemctl stop psychogo");
		}
		
	}
}

//			public static void CompressScript(string source
//		                                      , string destinationDirectory
//		                                     , string templateFile)
//		{
//
//			var fileName = Path.Combine(destinationDirectory, Path.ChangeExtension(GenerateFileVersion(source), ".js"));
//
//			var searchPattern = "*.js";
//
//
//			var matched = Path.GetFileNameWithoutExtension(fileName).TrimStart('.') + "_v";
////			var xxx=Directory.GetFiles(destinationDirectory, searchPattern)
////				.Where(i => i.StartsWith(source.GetFileNameWithoutExtension().TrimStart('.') + "_v_")).ToArray();
//			//	var files=Directory.GetFiles(destinationDirectory, searchPattern);
////				.Where(i => i.StartsWith(matched))
////				.AsParallel().ToArray();
//			Directory.GetFiles(destinationDirectory, searchPattern)
//				.Where(i => i.GetFileName().StartsWith(matched))
//				.AsParallel().ForAll(File.Delete);
//			
//	 
//			var js = CompileTypeScript(source, fileName);
//			//CompressJavaScript(js,fileName);
//				
//			var str = Regex.Replace(templateFile.ReadAllText(), 
//				          "(?<=src=\"/static/)"
//				          + source.GetFileNameWithoutExtension()
//				          + "[\\.\\w\\-]+(?=\">)", fileName.GetFileName());
//			templateFile.WriteAllText(str);
//
//		}
//		public static string GenerateFileVersion(string path)
//		{
//	 
//			return path.GetFileNameWithoutExtension().TrimStart('.')
//			                    +"_v_"+GetHashForFile(path)
//			                    + path.GetExtension();
//		}
//		 
		
//
//		private void a()
//		{
//			HotKey(HWND.Cast(IntPtr.Zero), 1, 0, (int)Keys.F6);
//			HotKey(HWND.Cast(IntPtr.Zero), 2, 0, (int)Keys.F7);
//			HotKey(HWND.Cast(IntPtr.Zero), 3, 0, (int)Keys.F8);
//			HotKey(HWND.Cast(IntPtr.Zero), 4, 0, (int)Keys.F9);
//			HWND h = HWND.Cast(IntPtr.Zero);
//
//
//			Console.WriteLine("F8 " + HotKey(h, 5, 0119, (int)Keys.F8));
//			Console.WriteLine("Ctrl+D " + HotKey(h, 5, 0x0002, (int)Keys.D));
//			Console.WriteLine("Ctrl+9 CopySameLevelFiles " + HotKey(h, 21, 0x0002, (int)Keys.D9));
//			Console.WriteLine("Ctrl+5 HEX to Int " + HotKey(h, 22, 0x0002, (int)Keys.D5));
//
//
//
//
//			Console.WriteLine("Alt+1 " + HotKey(h, 11, 0x0001, (int)Keys.D1));
//			Console.WriteLine("Alt+2 " + HotKey(h, 12, 0x0001, (int)Keys.D2));
//			Console.WriteLine("Alt+3 " + HotKey(h, 13, 0x0001, (int)Keys.D3));
//			Console.WriteLine("Alt+4 " + HotKey(h, 14, 0x0001, (int)Keys.D4));
//			Console.WriteLine("Alt+6 " + HotKey(h, 15, 0x0001, (int)Keys.D6));
//
//			Console.WriteLine("Alt+7 反编译 Android" + HotKey(h, 17, 0x0001, (int)Keys.D7));
//			Console.WriteLine("Alt+8 反编译 Android" + HotKey(h, 19, 0x0001, (int)Keys.D8));
//			Console.WriteLine("Alt+9 分割 Android 资源文件" + HotKey(h, 19, 0x0001, (int)Keys.D9));
//
//			Console.WriteLine("Alt+X SearchInAndroidResources" + HotKey(h, 188, 0x0001, (int)Keys.X));
//
//			Console.WriteLine("Alt+Z Int to HEX " + HotKey(h, 190, 0x0001, (int)Keys.Z));
//			Console.WriteLine("Alt+C SearchInAndroidResources Use id " + HotKey(h, 167, 0x0001, (int)Keys.C));
//			Console.WriteLine("Ctrl+ALT+9" + HotKey(h, 1257, 0x0001 | 0x0002, (int)Keys.D9));
//			Console.WriteLine("Ctrl+ALT+8" + HotKey(h, 1256, 0x0001 | 0x0002, (int)Keys.D8));
//			Console.WriteLine("Ctrl+ALT+7" + HotKey(h, 1255, 0x0001 | 0x0002, (int)Keys.D7));
//
//			MSG msg = new MSG();
//			var handleRef = new HandleRef(null, IntPtr.Zero);
//
//			while (GetMessageW(ref msg, handleRef, 0, 0)) {
//				if (msg.message == WM_HOTKEY) {
//
//					var k = ((int)msg.lParam >> 16) & 0xFFFF;
//					var m = (int)msg.lParam & 0xFFFF;
//					Console.WriteLine(m);
//					if (m == 1) {
//						switch (k) {
//
//
//							case 55:
//								Utils.ClipboardText(Utils.DownloadApk);
//								break;
//							case 56:
//								Utils.ClipboardFile(file => {
//									if (file.EndsWith(".apk")) {
//										Utils.ExtractApk(file);
//									} else if (file.EndsWith(".dex")) {
//										Utils.ExtractJar(file);
//									}
//								});
//								break;
//							case  57:
//								Utils.ClipboardFile(f => {
//									if (f.EndsWith(".xml")) {
//										Utils.SplitAndroid(f.ReadAllText());
//									}
//								});
//								break;
//							case 67:
//								Utils.ClipboardString(Utils.SearchInAndroidResourcesFuzzyUsePublic);
//								break;
//							case 88:
//								Utils.ClipboardString(Utils.SearchInAndroidResourcesFuzzy);
//								break;
//							case 90:
//								Utils.ClipboardString(s => s.ToHex());
//								break;
//
//						}
////						if (k == 49) {
////							FormatParameters();
////						} else if (k == 50) {
////							FormatVariables();
////						} else if (k == 51) {
////							Utils.FormatPropertiesForLog();
////						} else if (k == 52) {
////							Utils.ClipboardText(Utils.DownloadApk);
////						} else if (k == 53) {
////							Utils.ClipboardFile(Utils.ExtractApk);
////						} else if (k == 54) {
////							Utils.ClipboardFile(Utils.ExtractJar);
////						}
//					} else if (m == 2) {
//						switch (k) {
//							case 53:
//								Utils.ClipboardString(s => s.HexStringToInt().ToString());
//								break;
//							case 57:
//								
//								break;
//							case 68:
//								DeleteFiles();
//								break;
//						}
////						if (k == 49) {
////							Utils.ClipboardFiles(Utils.CopySameLevelFiles);
////						} else if (k == 50) {
////							Utils.ClipboardString(s => s.HexStringToInt().ToString());
////						} else if (k == 68) {
////						
////						}
//						 
//					} else if (m == 3) {
//						switch (k) {
//							case 55:
//								Utils.ClipboardFile(Utils.FormatHtml);
//								break;
//							case 56: //D9
//								Utils.ClipboardDirectory(Utils.MoveFilesInDirectories);
//								break;
//							case 57: //D9
//								Utils.ClipboardString(Utils.GenerateAndroidLog);
//								break;
//						}
//					} else {
//						if (k == 117) {
//					
//							CompressScripts(
//								@"C:\Users\psycho\go\src\psycho\static\styles",
//								@"C:\Users\psycho\go\src\psycho\static",
//								false);
//							Console.WriteLine("F6");
//						} else if (k == 118) {
//							var dir = @"C:\Users\psycho\go\src\psycho\static\typescripts";
//							var fileNames = new string[] {
//								"songs.ts",
//								"utils.ts"
//							};
//						
//							foreach (var fileName in fileNames) {
//								CompileTypeScript(Path.Combine(dir, fileName));
//							}
//							Console.WriteLine("F7");
//						
//						} else if (k == 119) {
//							FormatAndroidComments();
//						
//						
//						} else if (k == 120) {
//
//							PublishStyle(
//								@"C:\Users\psycho\go\src\psycho\static\styles",
//								@"C:\Users\psycho\go\src\psycho\static",
//								@"C:\Users\psycho\go\src\psycho\templates\_header.html");
//							PublishScript(
//								@"C:\Users\psycho\go\src\psycho\static\typescripts\app.ts",
//								@"C:\Users\psycho\go\src\psycho\static",
//								@"C:\Users\psycho\go\src\psycho\templates\_footer.html"
//							);
//							Console.WriteLine("F9");
//						
//						}
//					}
//					
//					   
//				}
//			}
//		}
//
//		private static void FormatAndroidComments()
//		{
//			var s = Clipboard.GetText();
//			if (string.IsNullOrWhiteSpace(s))
//				return;
//			var value = Regex.Replace(s, "\\{@(link|see|linkplain|code)[\\s\\*]+#?([^\\}]+?)\\}", new MatchEvaluator((m) => m.Groups[2].Value));
//			value = Regex.Replace(value, @"<[^>]*>", String.Empty);
//			var lines = value.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries)
//				.Select(i => i.Trim().TrimStart(" \t/*".ToArray()).Trim());
////			var lines=value.Split(Environment.NewLine.ToArray(),StringSplitOptions.RemoveEmptyEntries)
////				.Select(i=>"         "+i.Trim().TrimStart(" \t/*".ToArray()).Trim());
//			//value="         /*\n"+string.Join("\n",lines)+"\n         */";
//			
//			var ls = lines.ToList();
//			var title = ls[ls.Count - 1];
//			ls.RemoveAt(ls.Count - 1);
//			value = "## " + title + "\n" + string.Join("\n", ls);
//			
//			if (string.IsNullOrWhiteSpace(value))
//				return;
//			Clipboard.SetText(value);
//		}
/*
		 */