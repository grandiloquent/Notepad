
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Common;

namespace Formatter
{
	class Program
	{
		// 维生素·矿物质全典 (生活中来系列丛书) - 王士钊
		public static void Main(string[] args)
		{
			var dir = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\维生素和矿物质健康食典 (健康食典系列) - 宋攀\text\1";
			foreach (var f in Directory.GetFiles(dir,"*.html")) {
			ConvertToMarkdowns(f);
				
			}
			//Console.ReadKey(true);
		}
		private static void Visitor(StringBuilder sb, HtmlNode htmlNode)
		{
			
			
			if (htmlNode.NodeType == HtmlNodeType.Text) {
				var text = htmlNode.GetInnerText();
				if (Regex.IsMatch(text, "^\\[\\w+]$")) {
					sb.Append("## ").Append(Regex.Match(text, "^\\[(\\w+)]$").Groups[1].Value).AppendLine().AppendLine();
					return;
				}
				
				text = Regex.Replace(text, "(?<=[\u4e00-\u9fa5])维生素(?=[A-Z] \\d)", "\n\n* 维生素");
				
				sb.Append(text);
				return;
			}
			const string imageFormat = "<div class=\"img-center\"><img alt=\"\" src=\"../static/pictures/{0}\"><div class=\"img-caption\"></div></div>";
			const string imagePrefix = "wsshkwzjksd_";
			const string sourceImagesDirectory = @"C:\Users\psycho\Desktop\新建文件夹\新建文件夹\维生素和矿物质健康食典 (健康食典系列) - 宋攀\images";
			
			var tagName = htmlNode.Name;
			
			if (tagName == "img") {
				
				if(sb.ToString().Contains("<img"))return;
				sb.Append(htmlNode.ParseImage(imagePrefix, imageFormat, sourceImagesDirectory));
				return;
			}
			
			switch (tagName) {
				case "h1":
					sb.AppendLine().AppendLine().Append("# ");
					break;
				case "h2":
					sb.AppendLine().AppendLine().Append("# ");
					break;
				case "h3":
					sb.AppendLine().AppendLine().Append("## ");
					break;
				case "h4":
					sb.AppendLine().AppendLine().Append("#### ");
					break;
				case "h5":
					sb.AppendLine().AppendLine().Append("##### ");
					break;
				case "h6":
					sb.AppendLine().AppendLine().Append("# ");
					break;
				case "p":
					if (htmlNode.GetClass() == "contenttitle1") {
						var text = htmlNode.GetInnerText();
						sb.AppendLine().AppendLine().Append("# ").AppendLine(text).AppendLine();
				
						return;
					}
					sb.AppendLine().AppendLine();
					break;
				case "b":
					if (htmlNode.GetClass() == "calibre4") {
						sb.AppendLine().AppendLine().Append("## ").AppendLine(Regex.Match(htmlNode.GetInnerText(), "【(\\w+)】").Groups[1].Value.RemoveWhiteSpace()).AppendLine();
						return;
					}
					sb.AppendLine().AppendLine();
					break;
			 
				case "table":
					htmlNode.ParseTable(sb);
					return;
				case "span":
					if(htmlNode.GetClass()=="math-sub")
						sb.Append("<sub>").Append(htmlNode.GetInnerText()).Append("</sub>");
						return;
					break;
//				case "div":
//					if (htmlNode.GetClass() == "kindle-cn-bodycontent-div-alone")
//						return;
//					break;
							
			}
		
			

			if (tagName == "body"
			    || tagName == "div"
			    || tagName == "a"
			    || tagName == "p"
			    || tagName == "b"
			    || tagName == "font") {
				
				var children = htmlNode.ChildNodes;
				
				foreach (var child in children) {
					Visitor(sb,	child);
				}
				return;
			} else {
				var text = htmlNode.GetInnerText();
				if (!string.IsNullOrWhiteSpace(text))
					sb.AppendLine(text).AppendLine();
			}
			
		}
	
		public static void ConvertToMarkdowns(string path)
		{
			var doc = new HtmlDocument();
			var text = File.ReadAllText(path, new UTF8Encoding(false));
			
			doc.LoadHtml(text.Sub());
			
			var sb = new StringBuilder();
			Visitor(sb, doc.DocumentNode.SelectSingleNode("//body"));
			
			sb.AppendLine();
			sb.AppendLine(">====>");
			sb.AppendLine("id:");
			sb.AppendLine("toc:营养");
			sb.AppendLine("tags:调料");
			sb.AppendLine(">====>");
			
			
			var str = sb.ToString().FromSub();
			
			str = str.RemoveRedundancyWhiteSpace();
			
			
			File.WriteAllText(Path.ChangeExtension(path, ".txt"), str.Replace("\r", ""), new UTF8Encoding(false));
		}
	}
}