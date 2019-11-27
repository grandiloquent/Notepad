namespace Formatter
{
	using System;
	using System.Collections.Generic;
	using HtmlAgilityPack;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Linq;
	using Common;
	public static class HtmlAgilityPacks
	{
		public static void ParseTable(this HtmlNode htmlNode, StringBuilder sb)
		{
			var trs = htmlNode.SelectNodes(".//tr");
			bool first = true;
			sb.AppendLine()
				.Append("<div class=\"table-wrapper table-nowrap\">")
				.AppendLine();
			foreach (var tr in trs) {
				var tds = tr.SelectNodes(".//td");
				var list = new List<string>();
				foreach (var td in tds) {
					list.Add(td.GetInnerText());
				}
				
				sb.AppendLine("|" + string.Join("|", list) + "|");
				if (first) {
					for (int i = 0; i < list.Count - 1; i++) {
						sb.Append("|---");
					}
					sb.AppendLine("|");
					first = false;
				}
			}
			sb.AppendLine("</div>").AppendLine();
		}
		
		public static string GetClass(this HtmlNode htmlNode)
		{
			if (htmlNode == null)
				return string.Empty;
			return htmlNode.GetAttributeValue("class", string.Empty);
		}
		public static string DecodeHtml(this string s)
		{
			return HtmlEntity.DeEntitize(s);
		}
		public static string GetInnerText(this HtmlNode htmlNode)
		{
			return HtmlEntity.DeEntitize(htmlNode.InnerText.Trim());
		}
		public static bool Contains(this HtmlNode n, Func<HtmlNode,bool> func)
		{
			return	n.ChildNodes.Any(i => i.NodeType == HtmlNodeType.Element && func(i));
			
		}
		public static string ParseImage(this HtmlNode n, string prefix, string imageFormat, string sourceDirectory)
		{
			//	var imageFormat = "<div class=\"img-center\"><img alt=\"\" src=\"../static/pictures/{0}\"><div class=\"img-caption\"></div></div>";
			var imageFile = n.GetAttributeValue("src", "").SubstringAtferLast('/');
			var targetFileName = prefix + imageFile;
			var dir = sourceDirectory;
			
			try {
				File.Move(Path.Combine(dir, imageFile),
					Path.Combine(dir, targetFileName));
			} catch {
				
			}
			return string.Format(imageFormat, targetFileName);
		}
	}
	
}