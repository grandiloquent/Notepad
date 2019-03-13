namespace Common{	
	using HtmlAgilityPack;
	using System;
	
	public static  class HtmlAgilityPacks{		public static string EscapeHtml(this string str)
		{
			return HtmlEntity.Entitize(str);
		}
		public static string GetAttribute(this HtmlNode htmlNode,string key, string defaultValue = null)
		{
			return htmlNode.GetAttributeValue(key, defaultValue);
		}
		
		public static string GetHref(this HtmlNode htmlNode, string defaultValue = null)
		{
			return htmlNode.GetAttributeValue("href", defaultValue);
		}
		
		public static HtmlDocument GetHtmlDocument(this string str)
		{
			
			var hd = new HtmlDocument();
			
			hd.LoadHtml(str);
			return hd;
		}
		public static string GetInnerText(this HtmlNode htmlNode)
		{
			return htmlNode.InnerText;
		}
		public static string GetTitle(this HtmlDocument hd)
		{
			
			var titleNode = hd.DocumentNode.SelectSingleNode("//title");
			return titleNode != null ? titleNode.InnerText : null;
		}
		
		public static void ListNodes(this HtmlDocument hd, string xpath, Action<HtmlNode> action)
		{
			var nodes = hd.DocumentNode.SelectNodes(xpath);
			
			if (nodes!=null&& nodes.Count > 0) {
				foreach (var element in nodes) {
					action(element);
					
				}
			}
		}
		public static HtmlNodeCollection SelectNodesByClassName(this HtmlNode htmlNode,string className){
			return htmlNode.SelectNodes("//*[contains(@class,'"+className+"')]");
		}
		public static string UnescapeHtml(this string str)
		{
			return HtmlEntity.DeEntitize(str);
		}
}}