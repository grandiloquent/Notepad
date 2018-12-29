
using System;
using HtmlAgilityPack;

namespace Utils
{
	
	public static class HtmlAgilityPackExtensions
	{
		public static string  DeEntitize(this string value)
		{
			return HtmlAgilityPack.HtmlEntity.DeEntitize(value);
		}
		public static HtmlAgilityPack.HtmlNode GetHtmlNode(this string value)
		{
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(value);
			return hd.DocumentNode;
		}
	}
}
