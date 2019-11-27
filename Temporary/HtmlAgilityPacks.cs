namespace Temporary
{
	using System;
	using HtmlAgilityPack;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Linq;
	public static class HtmlAgilityPacks
	{
		
		public static string GetClass(this HtmlNode htmlNode)
		{
			if(htmlNode==null)return string.Empty;
			return htmlNode.GetAttributeValue("class", string.Empty);
		}
		public static string DecodeHtml(this string s){
			return HtmlEntity.DeEntitize(s);
		}
		public static string GetInnerText(this HtmlNode htmlNode){
			return HtmlEntity.DeEntitize(htmlNode.InnerText.Trim());
		}
		public static bool Contains(this HtmlNode n,Func<HtmlNode,bool> func){
			return	n.ChildNodes.Any(i => i.NodeType == HtmlNodeType.Element && func(i));
			
		}
	}
	
}