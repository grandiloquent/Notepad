 
using System;
using HtmlAgilityPack;

namespace Shared
{
	public static class HtmlAgilityPacks
	{
		 
		public static HtmlNode GetDocumentNode(string html){
			var hd = new HtmlDocument();
			hd.LoadHtml(html);
			return hd.DocumentNode;
		}
		public static HtmlNodeCollection GetElementsByClass(string html, string className)
		{
			
			var hd = new HtmlDocument();
			hd.LoadHtml(html);
			
			return hd.DocumentNode.SelectNodes(string.Format("//*[contains(@class,'{0}')]", className));
		}
	}
}
