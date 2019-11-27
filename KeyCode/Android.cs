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
	public static  class Androids
	{
//		public static string GeneratInterfaceMethods(string s)
//		{
//		 
//			var j = s.RemoveComments().ToBlocks().Where(i => i.Contains("interface ")).Select(i => i.SubstringAfter("{").SubstringBefore(';')).ToArray();
//			
//			var ls = new List<string>();
//			
//			foreach (var element in j) {
//				ls.Add(element + "\r\n{\r\n\r\n}\r\n");
//			}
//			return ls.ConcatenateLines();
//		}
		
			
		public static string OrderAndroidResource(string value)
		{
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

			var array = ls.ToArray();
			xd.DocumentElement.InnerXml = string.Join(Environment.NewLine, ls.GroupBy(ie => ie.Attributes["name"].Value).Select(g => g.First()).OrderBy(x => {

				if (x.InnerText.Contains('/'))
					return "zzzzzzz" + x.Attributes["name"].Value + x.InnerText.SubstringBefore('/');
				else
					return x.Attributes["name"].Value;
			}).Select(ie => ie.OuterXml.ToString()));
			return xd.OuterXml.Replace("\r", "");

		}
		public static void MoveFilesInDirectories(string dir)
		{
			
			var directories = Directory.GetDirectories(dir);
			var desktopDirectory =	Path.Combine(dir.GetDirectoryName(), "代码");
			desktopDirectory.CreateDirectoryIfNotExists(); 
			foreach (var directory in directories) {
				 
				var targetDirectory = Path.Combine(desktopDirectory, directory.GetFileName());
				targetDirectory.CreateDirectoryIfNotExists();
				var files = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
				foreach (var f in files) {
					//xml|aidl|css|c|js|html|h|mk
					if (!Regex.IsMatch(f, "\\.(?:java|c|cc|cpp|h|txt|gradle)$"))
						continue;
					
					//if(f.EndsWith(".class")||f.EndsWith(".dex"))continue;
					var targetFile = Path.Combine(targetDirectory, f.GetFileName());
				
					var i = 0;
					while (File.Exists(targetFile)) {
						targetFile = Path.Combine(targetDirectory, Path.GetFileNameWithoutExtension(f) + " - " + (++i) + Path.GetExtension(f));
					}
					File.Move(f, targetFile);
				}
			
			}
		}
		private static void RemoveFirstComment(string file)
		{
			
			var lines = file.ReadAllLines();
			var list = new List<string>();
			var skip = false;
			var first = true;
			foreach (var line in lines) {
				
				if (first && !skip && line.TrimStart().StartsWith("/*")) {
					skip = true;
					continue;
				}
				if (first && skip && line.TrimEnd().EndsWith("*/")) {
					skip = false;
					first = false;
					continue;
				}
				 
				if (skip || line.IsVacuum())
					continue;
				list.Add(line);
			}
			file.WriteAllLines(list);

		}
		public static string GenerateAndroidLog(this string value)
		{
			return value.ToBlocks()
						.Select(i => i.ReplaceFirst("{", string.Format("{{\nLog.e(TAG,\"[{0}]\");\n", i.SubstringBefore('(').Trim().SubstringAfterLast(' '))
			)).ConcatenateLines();
		
		}
		public static void ExtractApk(string packageName)
		{
			var apktool =	@"C:\bin\ApkTool\apktool.jar";
			Process.Start(new ProcessStartInfo() {
				FileName = "java",
				WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				Arguments = string.Format("-jar \"{0}\" d \"{1}\"", apktool, packageName)
			});
		}
		
		
		public static void ExtractJar(string packageName)
		{
			var fileName =	@"C:\bin\ApkTool\dex-tools-2.1-SNAPSHOT\d2j-dex2jar.bat";
			Process.Start(new ProcessStartInfo() {
				FileName = fileName,
				WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				Arguments = string.Format("\"{0}\"", packageName)
			});
		}
		
		public static string FormatConstFields(string s)
		{
			return	Regex.Replace(s, "(?<=(String|int|float|boolean|long|double) )\\w+(?= =)", new MatchEvaluator((m) => m.Value.ToUpper()));
		}
		public static string FormatConstStringFields(string s)
		{
			return	Regex.Replace(s, "(?<=String )(\\w+ =) \"([^\"]*?)(?=\";)", new MatchEvaluator((m) => m.Groups[1].Value + "\"" + m.Groups[1].Value.SubstringAfter("_").SubstringBefore(' ').ToLower().Replace('_', '-')));
		}
		public static string GenerateLeft(String s)
		{
			var patterns1 = new string[]{ "left", "top", "right", "bottom" };
			var patterns2 = new string[]{ "measuredWidth", "measuredHeight" };
			
			var list1 = new List<string>();
			var list2 = new List<string>();
			var list3 = new List<string>();
			
			for (int i = 0; i < patterns1.Length; i++) {
				var n = new String(s.Skip(1).ToArray()).Decapitalize();
				// https://github.com/JetBrains/intellij-community/blob/4999f5293e4307870020f1d0d672a3d35a52f22d/platform/lang-impl/src/com/intellij/codeInsight/template/macro/CapitalizeMacro.java
				list1.Add(string.Format("int {0}{1} = {2};", n, patterns1[i].Capitalize(), patterns1[i]));
				list2.Add(n + patterns1[i].Capitalize());
			}
			for (int i = 0; i < patterns2.Length; i++) {
				list3.Add(string.Format("int {0}{1} = {2}.get{3}();", new String(s.Skip(1).ToArray()).Decapitalize(), patterns2[i].Capitalize()
				                        , s
				                        , patterns2[i].Capitalize()));
			
			}
			return list1.ConcatenateLines() + list3.ConcatenateLines() + string.Join(",", list2);
		}
		public static string FormatGradleImplementation(string value)
		{
			var array = Regex.Matches(value, "'([^']*?)'").Cast<Match>().Select(i => i.Groups[1].Value).ToArray();
			var len = array.Length;
			var list = new List<String>();
			for (int i = 0; i < len; i++) {
				if (i + 2 < len) {
					list.Add(string.Format("implementation \"{0}:{1}:{2}\"",
						array[i],
						array[i + 1],
						array[i + 2]
					));
				}
			}
			return list.OrderBy(i => i).Distinct().ConcatenateLines();
		}
//		public static string GenerateInterfaceFromClass(string s)
//		{
//			var array =	Regex.Matches(Regex.Replace(s.RemoveComments(), "@[^ ]*? ", ""), "(?<=(protected|public) )[^)^{^=]*?\\)").Cast<Match>()
//			            //.Where(i=>!i.Value.StartsWith("final"))
//				.Select(i => Regex.Replace(i.Value, "[\r\n]+", " ").SubstringAfter("abstract ") + ";")
//			.OrderBy(i => i)
//				.Distinct().ToArray();
//			return array.ConcatenateLines();
//		}
		public static string LogFields(string s)
		{
			var parts = Regex
				.Matches(s, "m[a-zA-Z0-9_]+")
				.Cast<Match>()
				.Select(i => i.Value)
				.Distinct().OrderBy(i => i)
				.Select(i => string.Format("\n\"\\n {0} = \"+ ({1}) ", i.Trim(), i.Trim()));
			var r = string.Join("+", parts);
			if (string.IsNullOrWhiteSpace(r))
				return string.Empty;
			return r;
		}
		public static string LogParameters(string s)
		{
			if (string.IsNullOrWhiteSpace(s))
				return string.Empty;
			var xx = Regex
			.Matches(s, "");
			var	parts = Regex
				.Matches(s, "(\\bm[a-zA-Z_0-9]+\\b)|((?<=\\w+ )[a-zA-Z_0-9]+(?=,|\\)))|((?<=\\w+ )[a-z][a-zA-Z_0-9]+(?= =))")
				.Cast<Match>()
			.Select(i => i.Value).ToArray();
		
			parts = parts.Distinct().OrderBy(i => i)
			.Select(i => string.Format("\n\"\\n {0} = \"+ ({1}) ", i.Trim(), i.Trim())).ToArray();
			var	r = string.Join("+", parts);
			if (string.IsNullOrWhiteSpace(r))
				return string.Empty;
			return r;
		}
		public static string LogVariables(string s)
		{
			if (string.IsNullOrWhiteSpace(s))
				return string.Empty;
			var parts = Regex.Matches(s, "(?<=[a-z] )\\w+(?= =)").Cast<Match>().Select(i => i.Value)
				.Select(i => string.Format("\n\"\\n {0} = \"+ {1} ", i, i));
			;
			var r = string.Join("+", parts);
			if (string.IsNullOrWhiteSpace(r))
				return r;
			return r;
		}
		private static void MatchAndroidXML(StringBuilder sb, IEnumerable<string> files, XmlDocument xd, string searchName)
		{
			bool isFirst = true;
			foreach (var f in files) {
				xd.LoadXml(f.ReadAllText());
				var	i = xd.DocumentElement.ChildNodes.GetEnumerator();
				while (i.MoveNext()) {
					var nodes = i.Current as XmlNode;
					if (nodes is XmlComment || nodes.Name == "public" || (nodes.Name == "item" && nodes.Attributes["type"].Value == "id"))
						continue;
					if (nodes.Attributes["name"] != null && nodes.Attributes["name"].Value == searchName) {
						if (isFirst) {
							sb.Clear();
							isFirst = false;
						}
						var dirName = f.GetDirectoryName().GetFileName();
						sb.AppendLine(dirName.IndexOf('-') != -1 ? string.Format("<!--{0}-->", dirName) : "")
							.AppendLine(nodes.OuterXml).AppendLine();
					}
				}
			}
		}
		public static string SearchInAndroidResourcesFuzzy(string value)
		{
			var files = Directory.GetFiles("res".GetDesktopPath(), "*.xml", SearchOption.AllDirectories);
			if (files.Length == 0) {
				return null;
			}
			var xd = new XmlDocument();
			var sb = new StringBuilder();
			foreach (var f in files) {
				xd.LoadXml(f.ReadAllText());
				var i = xd.DocumentElement.ChildNodes.GetEnumerator();
				while (i.MoveNext()) {
					var nodes = i.Current as XmlNode;
					if (nodes is XmlComment || nodes.Name == "public")
						continue;
					if (nodes.Attributes["name"].Value == value) {
						sb.AppendLine(nodes.OuterXml).AppendLine();
					}
				}
			}
			return sb.ToString();
		}
		public static string SearchInAndroidResourcesFuzzyUsePublic(string value)
		{
			var files = Directory.GetFiles("res".GetDesktopPath(), "*.xml", SearchOption.AllDirectories);
			var xd = new XmlDocument();
			var sb = new StringBuilder();
			if (!Regex.IsMatch(value, "^\\d+$")) {
				MatchAndroidXML(sb, files, xd, value);
				if (sb.ToString().Trim().StartsWith("<"))
					return sb.ToString().Trim();
				return string.Format("findViewById(R.id.{0})", sb.ToString().Trim());
			}
			if (files.Length == 0) {
				return null;
			}
			var publicXMLFile = string.Empty;
			foreach (var publicXML in files) {
				if (publicXML.GetFileName() == "public.xml") {
					publicXMLFile = publicXML;
					break;
				}
			}
			if (publicXMLFile == string.Empty) {
				return null;
			}
			var pattern = value.ToHex();
			var searchName = string.Empty;
			xd.LoadXml(publicXMLFile.ReadAllText());
			var i = xd.DocumentElement.ChildNodes.GetEnumerator();
			while (i.MoveNext()) {
				var nodes = i.Current as XmlNode;
				if (nodes is XmlComment)
					continue;
				if (nodes.Attributes["id"].Value.ToLower().EndsWith(pattern)) {
					searchName = nodes.Attributes["name"].Value;
					sb.AppendLine(searchName);
					break;
				}
			}
			if (searchName == string.Empty)
				return null;
			MatchAndroidXML(sb, files, xd, searchName);
			return sb.ToString().Trim();
		}
	}
}