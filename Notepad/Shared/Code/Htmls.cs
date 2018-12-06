 
using System;

namespace Shared
{
	 
	public static class Htmls
	{
		public static string DeEntitize(string text)
		{
			return HtmlAgilityPack.HtmlEntity.DeEntitize(text);
		}
		public static string Entitize(string text)
		{
			return HtmlAgilityPack.HtmlEntity.Entitize(text);
		}
	}
}
