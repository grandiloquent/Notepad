namespace Notepad
{
	using System.Collections.Generic;
	using System.Text;
	using Helpers;
	using System.IO;
	using System.Text.RegularExpressions;
	using System.Linq;
	using Microsoft.Ajax.Utilities;
	using System;
	using System.Windows.Forms;
using Common;
	static class Helper
	{
		public static void GenerateString(TextBox textBox,string find,string replace){
		var text = textBox.Text;
			
			if (string.IsNullOrWhiteSpace(text)
			    || string.IsNullOrWhiteSpace(find)
			    || string.IsNullOrWhiteSpace(replace)) {
				return;
			}
			var patternCount = Regex.Matches(replace, "(?<!\\{)\\{\\d+\\}(?!\\})").Count;
			var matches = Regex.Matches(text, find).Cast<Match>().Select(i => i.Value);
			var ls = new List<string>();
			foreach (var element in matches) {
				var elements = new string[patternCount];
				for (int i = 0; i < patternCount; i++) {
					
						elements[i] = element;
				}
				ls.Add(string.Format(replace, elements));
			}
			var result = ls.ConcatenateLines();
			if (!string.IsNullOrWhiteSpace(result))
				Clipboard.SetText(result);	
//			var text = textBox.Text;
//		
//			if (string.IsNullOrWhiteSpace(text)
//			    || string.IsNullOrWhiteSpace(find)
//			    || string.IsNullOrWhiteSpace(replace)) {
//				return;
//			}
//			var matches = Regex.Matches(text, find).Cast<Match>().Select(i => i.Value);
//			var ls = new List<string>();
//			foreach (var element in matches) {
//				ls.Add(string.Format(replace, element));
//			}
//			var result = ls.ConcatenateLines();
//			if (!string.IsNullOrWhiteSpace(result))
//				Clipboard.SetText(result);
		}
		public static string FormatDevdocsProperties(string s)
		{
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(s);
			var parent = hd.DocumentNode.SelectSingleNode("//dl");
			var ls = new List<string>();
			foreach (var n in parent.ChildNodes) {
				if (n.NodeType == HtmlAgilityPack.HtmlNodeType.Element) {
					if (n.Name == "dt") {
						var code = n.SelectSingleNode(".//code");
						ls.Add("### " + code.InnerText.SubstringAfterLast('.') + Environment.NewLine);
					} else if (n.Name == "dd") {
						var ps = n.SelectNodes(".//p");
						if (ps != null) {
							foreach (var p in ps) {
								ls.Add(HtmlAgilityPack.HtmlEntity.DeEntitize(p.InnerText) + Environment.NewLine);
							}
						} else {
							ls.Add(HtmlAgilityPack.HtmlEntity.DeEntitize(n.InnerText) + Environment.NewLine);
						}
					}
				}
			}
			return ls.ConcatenateLines();
		}
		public static string FormatDevdocsTable3(string s)
		{
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(s);
			var nodes = hd.DocumentNode.SelectNodes("//tbody/tr");
			var ls = new List<Tuple<string,string, string>>();
			foreach (HtmlAgilityPack.HtmlNode n in nodes) {
				var tds =	n.SelectNodes(".//td");
				ls.Add(new Tuple<string, string, string>(tds[0].InnerText.Trim(),
				                                         HtmlAgilityPack.HtmlEntity.DeEntitize(tds[1].InnerText),
				                                         tds.Count>2? HtmlAgilityPack.HtmlEntity.DeEntitize(tds[2].InnerText)+Environment.NewLine.Repeat(2):""
			                                         //+HtmlAgilityPack.HtmlEntity.DeEntitize(tds[3].InnerText)+Environment.NewLine.Repeat(2)
			                                      //  +HtmlAgilityPack.HtmlEntity.DeEntitize(tds[4].InnerText)+Environment.NewLine.Repeat(2)
//				                                        +HtmlAgilityPack.HtmlEntity.DeEntitize(tds[5].InnerText)+Environment.NewLine.Repeat(2)
//					
				                                       ));
			}
			return ls.OrderBy(i=>i.Item1).Select(i=>"### "+ i.Item1+Environment.NewLine.Repeat(2)
			                                     +i.Item2+Environment.NewLine.Repeat(2)
			                                     +i.Item3+Environment.NewLine
			                                     
			                                    ).ConcatenateLines();
		}
		public static string FormatDevdocsTable2(string s)
		{
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(s);
			var nodes = hd.DocumentNode.SelectNodes("//tbody/tr");
			var ls = new List<Tuple<string,string>>();
			if(nodes!=null){
			foreach (HtmlAgilityPack.HtmlNode n in nodes) {
				var tds =	n.SelectNodes(".//td");
				try{
				ls.Add(new Tuple<string, string>(tds[0].InnerText, HtmlAgilityPack.HtmlEntity.DeEntitize(tds[1].InnerText)));
				}catch{}
			   	}}
			return ls.OrderBy(i=>i.Item1).Select(i=>"### "+ i.Item1+Environment.NewLine.Repeat(2)+i.Item2+Environment.NewLine).ConcatenateLines();
		}
		public static void ReplaceTextInDirectory(string dir, string find, string replace)
		{
			var files = Directory.GetFiles(dir, "*").Where(i => Regex.IsMatch(i, "\\.(?:html)$"));
			foreach (var f in files) {
				f.WriteAllText(f.ReadAllText().Replace(find, replace));
			}
		}
		
		public static void ToggleCase(TextBox textBox)
		{
			var text = textBox.SelectedText;
			var hasUpper = false;
			foreach (var element in text) {
				if (char.IsUpper(element)) {
					hasUpper = true;
					break;
				}
			}
			textBox.SelectedText = hasUpper ? text.ToLower() : text.ToUpper();
		}
		
		public static  List<Tuple<string,int>> SearchInDirectory(string sourceDirectoy, string pattern)
		{
			
			var files =
				Directory
				.GetFiles(sourceDirectoy)
				.Where(i => Regex.IsMatch(i, "\\.(?:html|go|js)$"));
			var tuple = new List<Tuple<string,int>>();
			foreach (var element in files) {
				var matched = Regex.Match(element.ReadAllText(), pattern);
				if (matched.Success) {
					tuple.Add(new Tuple<string, int>(element, matched.Index));
				}
			}
			return tuple;
			
		}
		
		//		public static string SortString(string str){
		//		return	string.Join(Environment.NewLine , str
		//			                         .Split(Environment.NewLine.ToArray(),
		//			                          StringSplitOptions.RemoveEmptyEntries)
		//			                   .Select(i=>i.Trim())
		//			                   .Distinct()
		//			                   .OrderBy(i => i));
		//
		//		}
		
		public static string CamelCase(string s)
		{
			var p = s.Split("_-".ToArray(), System.StringSplitOptions.RemoveEmptyEntries);
		
			var sb = new StringBuilder();
			
			for (int i = 0; i < p.Length; i++) {
				var ss = p[i].Substring(0, 1);
				var se = p[i].Substring(1);
				if (i == 0) {
					sb.Append(ss.ToLower()).Append(se);
					continue;
				}
				
				sb.Append(ss.ToUpper()).Append(se);
			}
			return sb.ToString();
			
		}
		public static string StringTemplateUpperCaseSerial(string text, string matchPattern, string pattern)
		{
			if (string.IsNullOrWhiteSpace(text)
			    || string.IsNullOrWhiteSpace(matchPattern)
			    || string.IsNullOrWhiteSpace(pattern)) {
				return string.Empty;
			}
			var patternCount = Regex.Matches(pattern, "(?<!\\{)\\{\\d+\\}(?!\\})").Count;
			var matches = Regex.Matches(text, matchPattern).Cast<Match>().Select(i => i.Value);
			var ls = new List<string>();
			var index = 0;
			foreach (var element in matches) {
				var elements = new string[patternCount];
				for (int i = 0; i < patternCount; i++) {
					if (i == patternCount - 1) {
						continue;
					}
					elements[i] = element.ToUpper();
				}
				elements[elements.Length - 1] = (index++).ToString();
				ls.Add(string.Format(pattern.Replace("\\n", "\n"), elements));
			}
			var result = ls.ConcatenateLines();
				
			return result;
		}
		public static string StringTemplateAndroid(string text, string matchPattern, string pattern)
		{
			if (string.IsNullOrWhiteSpace(text)
			    || string.IsNullOrWhiteSpace(matchPattern)
			    || string.IsNullOrWhiteSpace(pattern)) {
				return string.Empty;
			}
			var patternCount = Regex.Matches(pattern, "(?<!\\{)\\{\\d+\\}(?!\\})").Cast<Match>().Select(i => i.Value).Distinct().Count();
			var matches = Regex.Matches(text, matchPattern).Cast<Match>().Select(i => i.Value);
			var ls = new List<string>();
			var index = 0;
			foreach (var element in matches) {
				var elements = new string[patternCount];
				for (int i = 0; i < patternCount; i++) {
					if (i == patternCount - 1) {
						continue;
					}
					elements[i] = "m" + CamelCase(element).CapitalizeOnlyFirst();
				}
				elements[elements.Length - 1] = element;
					
				ls.AddRange(string.Format(pattern.Replace("\\n", "\n"), elements).Split('\n'));
			}
			var result = ls.OrderBy(i => i.Trim()).ConcatenateLines();
				
			return result;
		}
		
//		public static void CompressScripts(string sourceDirectory, string destinationDirectory,
//			string templateFile,
//			bool isJavaScript = true)
//		{
//			var searchPattern = "*.js";
//			if (!isJavaScript) {
//				searchPattern = "*.css";
//			}
//				
//			Directory.GetFiles(destinationDirectory, searchPattern)
//				.Where(i => i.Contains("app_v_"))
//				.AsParallel().ForAll(File.Delete);
//			var files = Directory.GetFiles(sourceDirectory, searchPattern).OrderBy(i => i.GetFileNameWithoutExtension());
//			var sb = new StringBuilder();
//			foreach (var file in files) {
//				var n = file.GetFileName();
//				if (n.StartsWith(".")) {
//					if (isJavaScript)
//						CompressScript(file, destinationDirectory, @"C:\Users\psycho\go\src\psycho\templates\_footer.html");
//					else
//						CompressScript(file, destinationDirectory, @"C:\Users\psycho\go\src\psycho\templates\_header.html", false);
//						
//					continue;
//				}
//				if (n.StartsWith("$"))
//					continue;
//				sb.AppendLine(file.ReadAllText());
//			}
//			var fileName = "app_v_" + Cryptos.GetHashForString(sb.ToString()) + (isJavaScript ? ".js" : ".css");
//			var min = new  Minifier();
//			if (isJavaScript) {
//				var r =	min.MinifyJavaScript(sb.ToString());
//				Path.Combine(destinationDirectory, fileName).WriteAllText(r);
//				
//				var str = Regex.Replace(templateFile.ReadAllText(), 
//					          "(?<=src=\"/static/)"
//					          + "app"
//					          + "[\\.\\w\\-]+(?=\">)", fileName);
//				templateFile.WriteAllText(str);
//				
//				
//				
//			} else {
//				var r =	min.MinifyStyleSheet(sb.ToString());
//				Path.Combine(destinationDirectory, fileName).WriteAllText(r); 
//		 
//				var str = Regex.Replace(templateFile.ReadAllText(), 
//					          "(?<=rel=\"stylesheet\" href=\"/static/)" + "app" + "[\\.\\-\\w]+(?=\">)", fileName);
//				templateFile.WriteAllText(str);
//			}
//			
//		}
		public static void CombineCompressScript(string sourceDirectory, string destinationDirectory,
			string templateFile,
			bool isJavaScript = true)
		{
			var searchPattern = "*.js";
			if (!isJavaScript) {
				searchPattern = "*.css";
			}
				
			Directory.GetFiles(destinationDirectory, searchPattern)
				.Where(i => i.Contains("app_v_"))
				.AsParallel().ForAll(File.Delete);
			var files = Directory.GetFiles(sourceDirectory, searchPattern).OrderBy(i => i.GetFileNameWithoutExtension());
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
			var fileName = "app_v_" + Cryptos.GetHashForString(sb.ToString()) + (isJavaScript ? ".js" : ".css");
			var min = new  Minifier();
			if (isJavaScript) {
				var r =	min.MinifyJavaScript(sb.ToString());
				Path.Combine(destinationDirectory, fileName).WriteAllText(r);
				
				var str = Regex.Replace(templateFile.ReadAllText(), 
					          "(?<=src=\"/static/)"
					          + "app"
					          + "[\\.\\w\\-]+(?=\">)", fileName);
				templateFile.WriteAllText(str);
				
				
				
			} else {
				var r =	min.MinifyStyleSheet(sb.ToString());
				Path.Combine(destinationDirectory, fileName).WriteAllText(r); 
		 
				var str = Regex.Replace(templateFile.ReadAllText(), 
					          "(?<=rel=\"stylesheet\" href=\"/static/)" + "app" + "[\\.\\-\\w]+(?=\">)", fileName);
				templateFile.WriteAllText(str);
			}
			
		}
		
//		public static void CompressScript(string source
//		                                      , string destinationDirectory
//		                                     , string templateFile
//		                                    , bool isJavaScript = true)
//		{
//			
//			var fileName = Cryptos.GenerateFileVersion(source);
//			
//			var searchPattern = "*.js";
//			if (!isJavaScript) {
//				searchPattern = "*.css";
//			}
//
//			var matched = source.GetFileNameWithoutExtension().TrimStart('.') + "_v";
////			var xxx=Directory.GetFiles(destinationDirectory, searchPattern)
////				.Where(i => i.StartsWith(source.GetFileNameWithoutExtension().TrimStart('.') + "_v_")).ToArray();
//			//	var files=Directory.GetFiles(destinationDirectory, searchPattern);
////				.Where(i => i.StartsWith(matched))
////				.AsParallel().ToArray();
//			Directory.GetFiles(destinationDirectory, searchPattern)
//				.Where(i => i.GetFileName().StartsWith(matched))
//				.AsParallel().ForAll(File.Delete);
//			
//			if (isJavaScript) {
//				JavaScripts.CompressJavaScriptFile(source, destinationDirectory, fileName);
//		 
//				var str = Regex.Replace(templateFile.ReadAllText(), 
//					          "(?<=src=\"/static/)"
//					          + source.GetFileNameWithoutExtension().TrimStart('.')
//					          + "[\\.\\w\\-]+(?=\">)", fileName);
//				templateFile.WriteAllText(str);
//			} else {
//				JavaScripts.CompressStyleScriptFile(source, destinationDirectory, fileName);
//		 
//				var str = Regex.Replace(templateFile.ReadAllText(), 
//					          "(?<=rel=\"stylesheet\" href=\"/static/)" + source.GetFileNameWithoutExtension().TrimStart('.') + "[\\.\\-\\w]+(?=\">)", fileName);
//				templateFile.WriteAllText(str);
//			}
//			
//		
//		}
		
		
	}
}