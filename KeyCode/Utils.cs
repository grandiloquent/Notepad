namespace KeyCode
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using System.IO;
	using System.Net;
	using System.Net.Security;
	using System.Security.Cryptography.X509Certificates;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using System.Linq;
	using System.Xml;
	
	public static class Extensions
	{
		public static IEnumerable<string> ToBlocks(this string value)
		{
			var count = 0;
			var sb = new StringBuilder();
			var ls = new List<string>();
			for (var i = 0; i < value.Length; i++) {
				sb.Append(value[i]);
				if (value[i] == '{') {
					count++;
				} else if (value[i] == '}') {
					count--;
					if (count == 0) {
						ls.Add(sb.ToString());
						sb.Clear();
					}
				}
			}
			return ls;
		}
		public static string GetEntryPath(this string fileName)
		{
			return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), fileName);
		}
		public static string ReadAllText(this string path)
		{
			 //  Encoding.GetEncoding("gbk")
			using (StreamReader sr = new StreamReader(path,new UTF8Encoding(false), true))
				return sr.ReadToEnd();
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
		public static string SubstringBeforeLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		
		public static readonly char AltDirectorySeparatorChar = '/';
		public static readonly char DirectorySeparatorChar = '\\';
		public static readonly char VolumeSeparatorChar = ':';
		public static void CreateDirectoryIfNotExists(this string directory)
		{
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);
		}
		public static string GetDesktopPath(this string f)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), f);
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
		public static void WriteAllText(this String path, String contents)
		{
			using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(false), 1024))
				sw.Write(contents);
		}
			public static string	Decapitalize(this String text)
		{
    
				text = text.Substring(0, 1).ToLower() + text.Substring(1);
			return   text;
		}
		public static string	Capitalize(this String text)
		{
    
			text = text.Substring(0, 1).ToUpper() + text.Substring(1);
			return   text;
		}
		public static string SubstringAfterLast(this string str, char find)
		{
			var index = str.LastIndexOf(find);
			if (index != -1) {
				return str.Substring(index + 1);
			}
			return str;
		}
	 
		public static string SubstringBefore(this string str, char find)
		{
			var index = str.IndexOf(find);
			if (index != -1) {
				return str.Substring(0, index);
			}
			return str;
		}
		
		public static string GetFileNameWithoutExtension(this string fileName)
		{
			return Path.GetFileNameWithoutExtension(fileName);
		}
		
		
		public static string GetExtension(this string path)
		{
			return Path.GetExtension(path);
		}
		public static string GetCommandPath(this string fileName)
		{
			var dir = System.Reflection.Assembly.GetEntryAssembly().Location.GetDirectoryName();
			return dir.Combine(fileName);
		}
	
		public static string ChangeExtension(this string path, string extension)
		{
			
			if (path != null) {
				string s = path;
				for (int i = path.Length - 1; i >= 0; i--) {
					char ch = path[i];
					if (ch == '.') {
						s = path.Substring(0, i);
						break;
					}
					if (IsDirectorySeparator(ch))
						break;
				}

				if (extension != null && path.Length != 0) {
					s = (extension.Length == 0 || extension[0] != '.') ?
                        s + "." + extension :
                        s + extension;
				}

				return s;
			}
			return null;
		}
		internal static bool IsDirectorySeparator(char c)
		{
			return c == DirectorySeparatorChar || c == AltDirectorySeparatorChar;
		}
       
		public static string ReplaceFirst(this string line, string str1, string str2)
		{
			int idx = line.IndexOf(str1, StringComparison.Ordinal);
			if (idx >= 0) {
				line = line.Remove(idx, str1.Length);
				line = line.Insert(idx, str2);
			}
			return line;
		}
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (T item in items) {
				action(item);
			}
		}
		public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
		{
			int num = 0;
			foreach (T item in items) {
				action(item, num);
				num++;
			}
		}
		public static string ConcatenateLines(this IEnumerable<string> strings)
		{
			return strings.Concatenate((builder, nextValue) => builder.AppendLine(nextValue));
		}
		private static string Concatenate(this IEnumerable<string> strings,
			Func<StringBuilder, string, StringBuilder> builderFunc)
		{
			return strings.Aggregate(new StringBuilder(), builderFunc).ToString();
		}
		public static string SubstringAfter(this string value, string delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1+delimiter.Length);
		}
		public static void WriteAllLines(this string path, IEnumerable<string> contents)
		{
			using (var writer = new StreamWriter(path, false, new UTF8Encoding(false))) {
				foreach (string line in contents) {
					writer.WriteLine(line);
				}
			}
		}
		public static bool IsVacuum(this string value)
		{
			if (value == null)
				return true;
         

			for (int i = 0; i < value.Length; i++) {
				if (!char.IsWhiteSpace(value[i]))
					return false;
			}

			return true;
		}
		public static string SubstringAfterLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string[] ReadAllLines(this string path)
		{
			string line;
			List<string> lines = new List<string>();
			using (StreamReader sr = new StreamReader(path, new UTF8Encoding(false)))
				while ((line = sr.ReadLine()) != null)
					lines.Add(line);
			return lines.ToArray();
		}
		
		public static string RemoveNewLine(this string value)
		{
			return Regex.Replace(value, "[\r\n]+", "");
		}
		public static int HexStringToInt(this string value)
		{
			if (value.StartsWith("0x"))
				value = value.Substring(2);
			return int.Parse(value, NumberStyles.HexNumber);
		}
		public static string ToHex(this string value)
		{
			return	int.Parse(value).ToString("x");
		}
		public static string Joining(this IEnumerable<string> collection, string separator = "\n")
		{
			return string.Join(separator, collection);
		}
           public static bool IsWhiteSpace(this string value)
        {
            if (value == null) return true;
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i])) return false;
            }
            return true;
        }
           
        public static string GetUniqueFileName(this string v)
        {
            int i = 1;
            Regex regex = new Regex(" \\- [0-9]+");
            string t = Path.Combine(Path.GetDirectoryName(v),
                           regex.Split(Path.GetFileNameWithoutExtension(v), 2).First() + " - " + i.ToString().PadLeft(3, '0') +
                           Path.GetExtension(v));
            while (File.Exists(t))
            {
                i++;
                t = Path.Combine(Path.GetDirectoryName(v),
                    regex.Split(Path.GetFileNameWithoutExtension(v), 2).First() + " - " + i.ToString().PadLeft(3, '0') +
                    Path.GetExtension(v));
            }
            return t;
        }
        public static string GetDirectoryFileName(this string v)
        {
            return Path.GetFileName(Path.GetDirectoryName(v));
        }


	}
	public static class Utils
	{
		public static async Task<string> GetWebDatacAsync(string url, Encoding c = null)
		{
			if (c == null)
				c = Encoding.UTF8;
			
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
			| SecurityProtocolType.Ssl3
			| SecurityProtocolType.Tls11
			| SecurityProtocolType.Tls12;
			
			ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => {
				return true;
			};
			HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(url);
			hwr.Timeout = 20000;
			hwr.KeepAlive = true;
			hwr.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
			hwr.Headers.Add(HttpRequestHeader.Upgrade, "1");
			hwr.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.110 Safari/537.36";
			hwr.Accept = "*/*";
//			hwr.Referer = "https://y.qq.com/portal/player.html";
//			hwr.Host = "c.y.qq.com";
			hwr.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
			//hwr.Headers.Add(HttpRequestHeader.Cookie, Cookie);
			var o = await hwr.GetResponseAsync();
			StreamReader sr = new StreamReader(o.GetResponseStream(), c);
			var st = await sr.ReadToEndAsync();
			sr.Dispose();
			return st;
		}
	
		private static bool RemoteCertificateValidate(
			object sender, X509Certificate cert,
			X509Chain chain, SslPolicyErrors error)
		{
			// trust any certificate!!!
			System.Console.WriteLine("Warning, trust any certificate");
			return true;
		}
		public static void SetCertificatePolicy()
		{
			ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
		}
		
	
		public static void FormatHtml(string filePath)
		{
			var h = new HtmlAgilityPack.HtmlDocument();
			h.LoadHtml(filePath.ReadAllText());
			
			var title = string.Empty;
			var stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine(@"<!DOCTYPE html>");
			stringBuilder.AppendLine(@"<html lang=""en"">");
			stringBuilder.AppendLine(@"");
			stringBuilder.AppendLine(@"<head>");
			stringBuilder.AppendLine(@"   <meta charset=""UTF-8"">");
			stringBuilder.AppendLine(@"   <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">");
			stringBuilder.AppendLine(@"   <meta http-equiv=""X-UA-Compatible"" content=""ie=edge"">");
			//stringBuilder.AppendLine(@"   <link href=""../../default.css"" rel=""stylesheet"" type=""text/css"">");
			stringBuilder.AppendLine(string.Format("   <title>{0}</title>", title));
			stringBuilder.AppendLine(@"</head>");
			stringBuilder.AppendLine(@"");
			stringBuilder.AppendLine(@"<body>");
			stringBuilder.AppendLine(string.Format("<h1>{0}</h1>", title));
			
			var htm =	h.DocumentNode.SelectSingleNode("//body").OuterHtml;
			htm = Regex.Replace(htm, "(?<=\\<img src=\")[\\./]+", "../../");
			htm = Regex.Replace(htm, "(?<= src=\")[\\./]+(?=images)", "../../");
			
			stringBuilder.AppendLine(htm);
			stringBuilder.AppendLine(@"</body>");
			stringBuilder.AppendLine(@"");
			stringBuilder.AppendLine(@"</html>");

			Path.Combine(filePath.GetDirectoryName(), filePath.GetFileName() + ".html").WriteAllText(stringBuilder.ToString());
		}
		public static void FormatAndroidDocuments(string filePath, string outputDirectory)
		{
			var h = new HtmlAgilityPack.HtmlDocument();
			h.LoadHtml(filePath.ReadAllText());
			
			var title = h.DocumentNode.SelectSingleNode("//h1").InnerText.Trim();
			var stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine(@"<!DOCTYPE html>");
			stringBuilder.AppendLine(@"<html lang=""en"">");
			stringBuilder.AppendLine(@"");
			stringBuilder.AppendLine(@"<head>");
			stringBuilder.AppendLine(@"   <meta charset=""UTF-8"">");
			stringBuilder.AppendLine(@"   <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">");
			stringBuilder.AppendLine(@"   <meta http-equiv=""X-UA-Compatible"" content=""ie=edge"">");
			stringBuilder.AppendLine(@"   <link href=""../../default.css"" rel=""stylesheet"" type=""text/css"">");
			stringBuilder.AppendLine(string.Format("   <title>{0}</title>", title));
			stringBuilder.AppendLine(@"</head>");
			stringBuilder.AppendLine(@"");
			stringBuilder.AppendLine(@"<body>");
			stringBuilder.AppendLine(string.Format("<h1>{0}</h1>", title));
			
			var htm =	h.DocumentNode.SelectSingleNode("//*[@id='jd-content']").OuterHtml;
			htm = Regex.Replace(htm, "(?<=\\<img src=\")[\\./]+", "../../");
			htm = Regex.Replace(htm, "(?<= src=\")[\\./]+(?=images)", "../../");
			
			stringBuilder.AppendLine(htm);
			stringBuilder.AppendLine(@"</body>");
			stringBuilder.AppendLine(@"");
			stringBuilder.AppendLine(@"</html>");

			Path.Combine(outputDirectory, title.GetValidFileName() + ".html").WriteAllText(stringBuilder.ToString());
		}
		public static string SearchInAndroidResources(string value)
		{
			var files = Directory.GetFiles("res".GetDesktopPath(), "*.xml", SearchOption.AllDirectories);
			
			if (files.Length == 0) {
				return null;
			}
			var type = value.SubstringBefore('/');
			var name = value.SubstringAfter("/");
			
			var xd = new XmlDocument();
			var sb = new StringBuilder();
			foreach (var f in files) {
				if (f.GetFileName().Contains(type)) {
					xd.LoadXml(f.ReadAllText());
					var i = xd.DocumentElement.ChildNodes.GetEnumerator();
					while (i.MoveNext()) {
						var nodes = i.Current as XmlNode;
						if (nodes is XmlComment)
							continue;
						if (nodes.Name == type && nodes.Attributes["name"].Value == name) {
							sb.AppendLine(nodes.OuterXml).AppendLine();
						}
					}
				}
			}
			return sb.ToString();
			
			
			  
		}
		
		public static string JavaGenerateLogForMethods(this string value)
		{
					 
			return value.ToBlocks().Select(i => i.ReplaceFirst("{",
				string.Format("{{\nSystem.out.println(\"===> [{0}]\");\n", Regex.Match(i, "(?<= )[^ ]*?\\([^\\)]*?\\)").Value.RemoveNewLine())
			)).Joining();
		
		}

		public static void SplitAndroid(string value)
		{
			var targetDirectory = "res".GetDesktopPath();
			targetDirectory.CreateDirectoryIfNotExists();

			var xd = new XmlDocument();
			xd.LoadXml(value);
			var ls = new List<XmlNode>();
			//			var first =	xd.FirstChild;
			//			var sb = new StringBuilder();
			//			if (first is XmlDeclaration) {
			//				sb.AppendLine(first.OuterXml.ToString());
			//			}
			var i = xd.DocumentElement.ChildNodes.GetEnumerator();
			while (i.MoveNext()) {
				var nodes = i.Current as XmlNode;
				if (nodes is XmlComment)
					continue;
				ls.Add(nodes);
			}
			ls.GroupBy(ie => ie.LocalName).ForEach(ig => {
				var output = new XmlDocument();
				var element = output.CreateElement("resources");
				output.AppendChild(element);
				ig.OrderBy(x => x.Attributes["name"].Value).ForEach(n => {
					;

					element.AppendChild(output.ImportNode(n, true));
				});

				output.Save(Path.Combine(targetDirectory, string.Format("{0}s.xml", ig.First().LocalName)));

			});


		}
		public static string GenerateStaticFieldsFromAndroid(string filePath)
		{
			var xd = new XmlDocument();
			xd.LoadXml(filePath.ReadAllText());
			var ls = new List<XmlNode>();

			var i = xd.DocumentElement.ChildNodes.GetEnumerator();
			while (i.MoveNext()) {
				var nodes = i.Current as XmlNode;
				if (nodes is XmlComment)
					continue;
				ls.Add(nodes);
			}

			var list = new List<String>();

			foreach (var item in ls) {
				var value = "";

				if (item.LocalName == "color") {
					if (item.InnerText.StartsWith("@color")) {
						var name = item.InnerText.SubstringAfterLast('/');
						var node = ls.First(a => a.Attributes["name"].Value == name);
						value = node.InnerText.Substring(1).ToUpper().PadLeft(8, 'F');
					} else {
						value = item.InnerText.Substring(1).ToUpper().PadLeft(8, 'F');
					}
					list.Add(string.Format("private static final int DEFAULT_{0} =0x{1};", item.Attributes["name"].Value.ToUpper(), value));
				} else if (item.LocalName == "dimen") {
					value = Regex.Match(item.InnerText, "[0-9\\,\\-]+").Value;
					list.Add(string.Format("private static final int DEFAULT_{0} ={1};", item.Attributes["name"].Value.ToUpper(), value));

				}
			}

			return list.OrderBy(a => a).ConcatenateLines();


		}
		public static void CopySameLevelFiles(string file)
		{
			var dst = "CopyFiles".GetDesktopPath();
			if (!Directory.Exists(dst))
				Directory.CreateDirectory(dst);
			var sourceDirectory = file.GetDirectoryName().GetDirectoryName();
			foreach (var element in Directory.GetDirectories(sourceDirectory)) {
				var ls = Directory.GetFiles(element).Where(i => i.GetFileName() == file.GetFileName());
				if (ls != null && ls.Any()) {
					var f = ls.First();
					if (f != null) {
						var dstDir = Path.Combine(dst, f.GetDirectoryName().GetFileName());
						dstDir.CreateDirectoryIfNotExists();
						File.Copy(f, Path.Combine(dstDir, f.GetFileName()));
					}
				}
			}

		}
		
		public static void DownloadApk(string packageName)
		{
			/*
			 Process.Start(new ProcessStartInfo() {
				FileName = "cmd",
				WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				Arguments = "/K adb shell pm path " + packageName + ">1.txt"
			});
			 */
			 
			var path =	ExcuteCommand("cmd",
				           Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				           "/K adb shell pm path " + packageName);
			path = path.SubstringAfter(":");
			Process.Start(new ProcessStartInfo() {
				FileName = "cmd",
				WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				Arguments = "/C adb pull " + path
			});
		}
		
		
		public static string ExcuteCommand(string fileName, string workingDirectory, string arguments)
		{
			Process process = new Process();
			StringBuilder outputStringBuilder = new StringBuilder();

			try {
				process.StartInfo.FileName = fileName;
				process.StartInfo.WorkingDirectory = workingDirectory;
				process.StartInfo.Arguments = arguments;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
				process.StartInfo.CreateNoWindow = false;
				process.StartInfo.UseShellExecute = false;
				process.EnableRaisingEvents = false;
				process.OutputDataReceived += (sender, eventArgs) => outputStringBuilder.AppendLine(eventArgs.Data);
				process.ErrorDataReceived += (sender, eventArgs) => outputStringBuilder.AppendLine(eventArgs.Data);
				process.Start();
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
				var processExited = process.WaitForExit(5 * 1000);

//				if (processExited == false) { // we timed out...
//					process.Kill();
//					throw new Exception("ERROR: Process took too long to finish");
//				} else if (process.ExitCode != 0) {
//					var output = outputStringBuilder.ToString();
//					var prefixMessage = "";
//
//					throw new Exception("Process exited with non-zero exit code of: " + process.ExitCode + Environment.NewLine +
//					"Output from process: " + outputStringBuilder.ToString());
//				}
			} finally {                
				process.Close();
			}
			return outputStringBuilder.ToString();
		}
		public static void ClipboardText(Action<string> f)
		{
			var s = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(s))
				return;
			f(s);
			
		}
		public static void ClipboardString(Func<string,string> f)
		{
			var s = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(s))
				return;
			var r = f(s);
			
			if (string.IsNullOrWhiteSpace(r))
				return;
			Clipboard.SetText(r);
		}
		public static void ClipboardFile(Action<string> f)
		{
			var s = Clipboard.GetText().Trim();
			if (string.IsNullOrWhiteSpace(s) || !File.Exists(s)) {
				var c = Clipboard.GetFileDropList();
				foreach (var element in c) {
					if (File.Exists(element)) {
						s = element;
						break;
					}
				}
			}
			if (!File.Exists(s))
				return;
			
			f(s);
			
		}
		public static void ClipboardDirectory(Action<string> f)
		{
			var s = Clipboard.GetText().Trim();
			if (string.IsNullOrWhiteSpace(s) || !Directory.Exists(s)) {
				var c = Clipboard.GetFileDropList();
				foreach (var element in c) {
					if (Directory.Exists(element)) {
						s = element;
						break;
					}
				}
			}
			if (!Directory.Exists(s))
				return;
			
			f(s);
			
		}
		public static void ClipboardFiles(Action<string> f)
		{
			var s = Clipboard.GetFileDropList();
			if (s.Count == 0)
				return;
			foreach (var element in s) {
				f(element);
			}
			
			
		}
		public static void FormatPropertiesForLog()
		{
			
			ClipboardString(s => {
				var parts = Regex.Matches(s, "(?<=public (final )?(int|float|long) )get[a-zA-Z]+(?=\\(\\))").Cast<Match>().Select(i => i.Value)
				.Select(i => string.Format("\n\"\\n {0} = \"+ obj.{1}() ", i, i));
				;
				var r = string.Join("+", parts);
				return r;
			});
			
		
			
		
			
		}
	}
}