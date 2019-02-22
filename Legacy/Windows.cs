 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Common;
using Utils;
namespace Notepad
{
	public static class WindowsUtilities
	{
		public static void DeleteString(this TextBox textBox){
			var selected=textBox.SelectedText;
			if(selected.IsVacuum())return;
			textBox.Text=textBox.Text.Replace(selected,"");
		}
	
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
		public static void OnClipboardString(this Form _, Func<String,String> func)
		{ 
			var str = Clipboard.GetText().Trim();
			if (str.IsVacuum())
				return;
			str = func(str);
			if (str.IsReadable())
				Clipboard.SetText(str);
			 
		}
		public static void OnClipboardDirectory(this Form _, Action<String> action)
		{
		 
			var dir = Clipboard.GetText().Trim();
			var found = false;
			if (Directory.Exists(dir)) {
				found = true;
			} else {
				var ls = Clipboard.GetFileDropList();
				if (ls.Count > 0) {
					if (Directory.Exists(ls[0])) {
						dir = ls[0];
					}
				}
			}
			if (found) {
				action(dir);
			}
			 
		}
		
	}
}
