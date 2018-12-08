
using System;
using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;

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
	}
}
