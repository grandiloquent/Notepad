
using System;
using System.Text;

namespace Notepad
{
	public static class TOCHelper
	{
		
		public static string GenerateTOC(string value){
			var lines = value.ToLines();
			var index = 0;
			var prefix = "#section-";
			var sb = new StringBuilder();
			foreach (var element in lines) {
				if (element.StartsWith("## ")) {
					sb.AppendFormat(string.Format("- [{0}](#{1})\r\n", element.SubstringAfter(" "), GetId(element.SubstringAfter(" ").Trim())));
				} else if (element.StartsWith("### ")) {
					sb.AppendFormat(string.Format("\t- [{0}](#{1})\r\n", element.SubstringAfter(" "), GetId(element.SubstringAfter(" ").Trim())));
				}
			}
			return sb.ToString();
		}
		public static String GetId(string value)
		{
				return UrilizeAsGfm(value);
		   	
		}
		public static string UrilizeAsGfm(string headingText)
		{
			// Following https://github.com/jch/html-pipeline/blob/master/lib/html/pipeline/toc_filter.rb
			var headingBuffer = new StringBuilder();
			for (int i = 0; i < headingText.Length; i++) {
				var c = char.ToLowerInvariant(headingText[i]);
				if (char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c == '_'||c=='.') {
					headingBuffer.Append(c == ' ' ? '-' : c);
				}
			}
			var result = headingBuffer.ToString();
			headingBuffer.Length = 0;
			return result;
		}
	}
}
