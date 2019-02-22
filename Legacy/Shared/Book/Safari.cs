
using System;
using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common;

namespace Shared
{

	public static class Safari
	{
		private static bool  CheckFisrtChildInnerText(HtmlNode htmlNode, String  name, string text)
		{
			try {
				
				foreach (var element in htmlNode.ChildNodes) {
					if (element.NodeType == HtmlNodeType.Element && element.Name == name && element.InnerText == text) {
						return true;
					}
				}
				
			} catch (Exception e) {
				return false;
			}
			return false;
		}
		private static HtmlNode FindFisrtChild(HtmlNode htmlNode, String  name, string className)
		{
			try {
				
				foreach (var element in htmlNode.ChildNodes) {
					if (element.NodeType == HtmlNodeType.Element && element.Name == name && element.GetAttributeValue("class", "").Contains(className)) {
						return element;
					}
				}
				
			} catch (Exception e) {
				return null;
			}
			return null;
		}
		// 提取出版页面HTML源代码中的书籍链接
		public static string[] ParsePublisher(string value)
		{
			var hd = new HtmlDocument();
			hd.LoadHtml(value);
	
			var nodes = hd.DocumentNode.SelectNodes("//article[contains(@class,'card')]")
				.Select(i => FindFisrtChild(i, "div", "card-info-container"))
				.Where(i => CheckFisrtChildInnerText(i, "p", "book"))
				.Select(i => FindFisrtChild(i, "h1", "card-title"))
				.Select(i => FindFisrtChild(i, "a", "js-title"))
				.Select(i => i.GetAttributeValue("href", ""))
				.Where(i => i.IsReadable())
				.Distinct()
				.Select(i => "https://www.safaribooksonline.com" + i)
				.ToArray();
			return nodes;
		}
		public static string[] ParseSearch(string value){
			
			var hd=new HtmlDocument();
			hd.LoadHtml(value);
			var nodes=hd.DocumentNode.SelectNodes("//a[contains(@class,'js-book-title')]");
			if(nodes.Any()){
				return nodes.Select(i => i.GetAttributeValue("href", ""))
				.Where(i => i.IsReadable())
				.Distinct()
				.Select(i => "https://www.safaribooksonline.com" + i)
				.ToArray();
			}
			return null;
		}
		
		public static void CombineBook(string dir){
			var toc=Path.Combine(dir,"目录.html");
			if(toc.FileExists()){
				var hd=new HtmlAgilityPack.HtmlDocument();
				hd.LoadHtml(toc.ReadAllText());
				File.Delete(toc);
				var sb=new StringBuilder();
				sb.AppendLine("<!DOCTYPE html> <html lang=\"en\"> <head> <meta charset=\"utf-8\"> <meta content=\"IE=edge\" http-equiv=\"X-UA-Compatible\"> <meta content=\"width=device-width,initial-scale=1\" name=\"viewport\"><link href=\"style.css\" rel=\"stylesheet\"></head><body>");
				
				sb.Append(hd.DocumentNode.SelectSingleNode("//ol").InnerHtml.Replace(".html\"","\"").Replace("href=\"","href=\"#"));
				var links=hd.DocumentNode.SelectNodes("//a").Select(i=>i.GetAttributeValue("href","")).ToArray();
				foreach (var element in links) {
					try{
						var file=Path.Combine(dir,element);
					hd.LoadHtml(file.ReadAllText());
					File.Delete(file);
					sb.AppendFormat("<div id=\"{0}\">",element.GetFileNameWithoutExtension());
					sb.Append(hd.DocumentNode.SelectSingleNode("//body").InnerHtml);
					sb.AppendLine("</div>");
					}catch{}
				}
				sb.AppendLine("</body></html>");
				Path.Combine(dir,dir.GetFileName()+".html").WriteAllText(sb.ToString());
			}
			
		}
	}
}
