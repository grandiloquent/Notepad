namespace StringCompare
{
	using StringCompare;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using Microsoft.Ajax.Utilities;
	using Common;
	
	public static class CSSDelegate
	{
			
		private static string MinifyCss(string s)
		{
			var min = new Minifier();
			// https://github.com/microsoft/ajaxmin/blob/master/AjaxMinDll/Css/CssSettings.cs
			
			var r = min.MinifyStyleSheet(s);
			return r;
		}
		[BindMenuItem(Name = "从HTML合并文件", Control = "compressCssSplitButton", Toolbar = "toolstrip1")]
		public static void CombineAndCompressCSSFromHTML()
		{
			Methods.OnClipboardFile(file => {
				var htmlStr = file.ReadAllText();
				var src = Regex.Matches(htmlStr, "(?<=\\<link rel=\"stylesheet\" href=\")[^\"]+(?=\">)")
			                        		.Cast<Match>()
			                        		.Select(i => i.Value)
					.Where(i => !i.Contains("/app_v"));
				var sb = new StringBuilder();
				var dir = Path.GetDirectoryName(file);
				foreach (var s in src) {
					var f = Path.Combine(dir, s);
					if (File.Exists(f)) {
						sb.AppendLine(f.ReadAllText().Replace("url(../", "url("));
					}
				}
				int number = Regex.Match(htmlStr, "app_v\\d+.min.css").Value.ConvertToInt();
				number++;
					
				var sf = Path.Combine(dir, src.First());
				sf = Path.GetDirectoryName(sf);
				var dst = Path.Combine(Path.GetDirectoryName(sf), "app_v" + number + ".min.css");
				dst.WriteAllText(MinifyCss(sb.ToString()));
			
				
				file.WriteAllText(Regex.Replace(htmlStr, "app_v\\d+.min.css", "app_v" + number + ".min.css"));
			                        	             
			});
			
		}
//		[BindMenuItem(Name = "移除重复属性", Control = "compressCssSplitButton", Toolbar = "toolstrip1")]
//		public static void Remove()
//		{
//			Wins.OnClipboardString(s => {
//			                       	var lines=s.Split('\n')
//			                       		.Where(i=>!string.IsNullOrWhiteSpace(i))
//			                       		.Select(i=>i.Trim())
//			                       		.Reverse()
//			                       		.Select(i=> Tuple.Create(i.SubstringBefore(':').Trim(),i.SubstringAfter(':').Trim()))
//			                       		.DistinctBy(i=>i.Item1).Reverse();
//			                       	StringBuilder sb=new StringBuilder();
//			                       	foreach (var element in lines) {
//			                       		sb.Append(element.Item1).Append(':').Append(element.Item2);
//			                       	}
//			                       	return sb.ToString();
//			});
//			
		//}
		[BindMenuItem(Name = "创建", Control = "fileCssSplitButton", Toolbar = "toolstrip1")]
		public static void CreateFile()
		{
			//     font: 300 24px/32px -apple-system-font,BlinkMacSystemFont,"Helvetica Neue","PingFang SC","Hiragino Sans GB","Microsoft YaHei UI","Microsoft YaHei",Arial,sans-serif;

			Methods.OnClipboardString(v => {

				const string dir = @"C:\Users\psycho\go\src\euphoria\static\styles";
				var fileName = Methods.WriteFile(v, dir, ".css");
				return string.Format("<link rel=\"stylesheet\" href=\"../static/styles/{0}\">", fileName);
					                                   
			});
			
		}
		
		//cssCodeButton
		[BindMenuItem(Name = "font", Control = "cssCodeButton", Toolbar = "toolstrip1")]
		public static void ConvertFont()
		{
			//     font: 300 24px/32px -apple-system-font,BlinkMacSystemFont,"Helvetica Neue","PingFang SC","Hiragino Sans GB","Microsoft YaHei UI","Microsoft YaHei",Arial,sans-serif;

			Methods.OnClipboardString(v => {
				var weight = Regex.Match(v, "\\d+");
				var fontSize = Regex.Match(v, "(?<= )[\\da-z]+(?=/)");
				var lineHeight = Regex.Match(v, "(?<=/)[\\da-z]+(?= )");
				var fontFamily = Regex.Split(v, "(?<=/)[\\da-z]+(?= )").Last();
				
				var sb = new StringBuilder();
				sb.AppendFormat("font-weight: {0};\r\n", weight);
				sb.AppendFormat("font-size: {0};\r\n", fontSize);
				sb.AppendFormat("line-height: {0};\r\n", lineHeight);
				sb.AppendFormat("font-family: {0}\r\n", fontFamily);
				return sb.ToString();
				
			                          	
			});
		}
	}
}