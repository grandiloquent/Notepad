 
using System;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace Shared
{
 
	public static class Android
	{
		public static string ConvertToGradle(string value){
			// https://developer.android.com/jetpack/androidx/migrate
			var node=HtmlAgilityPacks.GetDocumentNode(value);
			
			var n=node.SelectNodes("//div[@class='devsite-table-wrapper']/table/tbody/tr").ToArray();
			var ls=new List<String>();
			if(n.Any())
			{
				foreach (var element in n) {
					var c=element.ChildNodes.ToArray()[3].InnerText;
					var str="// implementation \"{0}\"";
					ls.AddIfNotExist(string.Format(str,c));
				}
				var result=ls.Joining();
				return result;
			}
			return string.Empty;
		}
	}
}
