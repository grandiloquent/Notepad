namespace KeyCode
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Xml;
	public static  class C
	{
		public static void Foramt(string fileName)
		{
			
			Process.Start(new ProcessStartInfo() {
				FileName = "clang-format",
				WorkingDirectory = Path.GetDirectoryName(fileName),
				Arguments = "-style=\"{BasedOnStyle: WebKit, BreakBeforeBraces: Allman,IndentPPDirectives: AfterHash}\" -sort-includes=false -i \"" + Path.GetFileName(fileName) + "\""
			});
		}
		
		
		public static string FormatAndroidComments(string s)
		{
			if (string.IsNullOrWhiteSpace(s))
				return string.Empty;
			var value = string.Empty;
			var parts =	s.Split(' ');
			var ls = new List<string>();
			var sb = new StringBuilder();
			for (int i = 0; i < parts.Length; i++) {
				if (sb.Length > 60) {
					ls.Add(sb.ToString());
					sb.Clear();
				}
				if (!string.IsNullOrWhiteSpace(parts[i]))
					sb.Append(parts[i].Trim()).Append(' ');
			}
			if (sb.Length > 0)
				ls.Add(sb.ToString());
			value = "/*\r\n" + ls.ConcatenateLines().Trim() + "\r\n*/";
			if (string.IsNullOrWhiteSpace(value))
				return string.Empty;
			return value;
		}
		public static void CopyDirectory(string dir)
		{
			var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories)
				.Where(i => Regex.IsMatch(i, "\\.(?:c|cc|cpp|h|hpp|txt)$")).ToArray();
			var targetDirectory = "CopyFiles".GetDesktopPath();
			targetDirectory.CreateDirectoryIfNotExists();
			foreach (var f in files) {
				
				var td = f.SubstringAfter(dir);
				if (td.IndexOf("\\") == -1) {
					td = "";
				} else {
					td = td.SubstringBeforeLast("\\");
				}
				if (string.IsNullOrWhiteSpace(td)) {
					Path.Combine(targetDirectory, Path.GetFileName(f)).WriteAllText("");
				} else {
					var parts = td.Split('\\');
					td = targetDirectory;
					foreach (var element in parts) {
						td = Path.Combine(td, element);
						td.CreateDirectoryIfNotExists();
					}
					Path.Combine(td, Path.GetFileName(f)).WriteAllText("");
					
				}
				
			}
		}
		public static void Command2()
		{
//			var f = @"C:\Users\psycho\CLionProjects\Http\httplib";
//
//			var f1 = "main.cpp";
//			//var f2 = "detail.cpp";
//			//var cmd = string.Format("/K g++ \"{0}\" \"{1}\" -lws2_32 && a", f1, f2);
//			var cmd = string.Format("/K g++ \"{0}\" && a", f1);
			
			var f = @"C:\Users\psycho\CLionProjects\Http";
			
			var sb = new StringBuilder();
			
			sb.Append("/K")
				.Append(' ')
				.Append(@"g++ -DCPPHTTPLIB_OPENSSL_SUPPORT -DDEBUG -o bin\Request.o -c Request.cpp")
				.Append(" && ")
				.Append(@"g++ -g -shared -o bin\libRequest.dll bin\Request.o -lws2_32 -lssl -lcrypto")
				.Append(" && ")
				.Append(@"g++ main.cpp -o bin\main.exe -Lbin -lRequest")
				.Append(" && ")
				.Append(@"bin\main.exe")
				.Append(' ');
			
//			sb.Append("/K")
//				.Append(' ')
//				.Append(@"g++ main.cpp -o bin\main.exe -Lbin -lRequest")
//				.Append(" && ")
//				.Append(@"bin\main.exe")
//				.Append(' ');
			
			try {

				var ps = Process.GetProcesses().Where(i => i.ProcessName == "a" || i.ProcessName == "cmd");
				if (ps.Any()) {
					foreach (var p in ps) {
						p.Kill();
					}
				}
			} catch (Exception e) {
				Console.WriteLine("ERROR:" + e.Message);
			}
			Process.Start(new ProcessStartInfo() {
				FileName = "cmd",
				Arguments = sb.ToString(),
				WorkingDirectory = f
			});


		}
		public static void Command1()
		{
//			var f = @"C:\Users\psycho\CLionProjects\Http\httplib";
//
//			var f1 = "main.cpp";
//			//var f2 = "detail.cpp";
//			//var cmd = string.Format("/K g++ \"{0}\" \"{1}\" -lws2_32 && a", f1, f2);
//			var cmd = string.Format("/K g++ \"{0}\" && a", f1);
			
			var f = @"C:\Users\psycho\CLionProjects\Http";
			
			var sb = new StringBuilder();
			
			sb.Append("/K")
				.Append(' ')
//				.Append(@"g++ -DCPPHTTPLIB_OPENSSL_SUPPORT -DDEBUG -o bin\Request.o -c Request.cpp")
//				.Append(" && ")
//				.Append(@"g++ -g -shared -o bin\libRequest.dll bin\Request.o -lws2_32 -lssl -lcrypto")
//				.Append(" && ")
				.Append(@"g++ main.cpp -o bin\main.exe -Lbin -lRequest")
				.Append(" && ")
				.Append(@"bin\main.exe")
				.Append(' ');
			
//			sb.Append("/K")
//				.Append(' ')
//				.Append(@"g++ main.cpp -o bin\main.exe -Lbin -lRequest")
//				.Append(" && ")
//				.Append(@"bin\main.exe")
//				.Append(' ');
			
			try {

				var ps = Process.GetProcesses().Where(i => i.ProcessName == "a" || i.ProcessName == "cmd");
				if (ps.Any()) {
					foreach (var p in ps) {
						p.Kill();
					}
				}
			} catch (Exception e) {
				Console.WriteLine("ERROR:" + e.Message);
			}
			Process.Start(new ProcessStartInfo() {
				FileName = "cmd",
				Arguments = sb.ToString(),
				WorkingDirectory = f
			});


		}
		
		public static string FormatCode(string s)
		{
			var ls =	s.ToBlocks();
			var a = ls.Select(i => i.SubstringBefore('{').Trim() + ";").OrderBy(i => i);
			var b = ls.OrderBy(i => i.SubstringBefore('(').TrimEnd().SubstringAfterLast(" "));
			return	a.Concat(b).ConcatenateLines();
		}
	}
	
}