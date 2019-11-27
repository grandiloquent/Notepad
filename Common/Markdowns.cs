using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common
{
	public static class Markdowns
	{
		public static string Sub(this string value)
		{
			var s = Regex.Replace(value, "<sub[^>]*?>", "△", RegexOptions.IgnoreCase);
			s = Regex.Replace(s, "</sub>", "▲", RegexOptions.IgnoreCase);
			
			s = Regex.Replace(s, "<sup[^>]*?>", "●", RegexOptions.IgnoreCase);
			s = Regex.Replace(s, "</sup>", "◎", RegexOptions.IgnoreCase);
			return s;
		}
			
		public static string FromSub(this string value)
		{
			var s = value.Replace("△", "<sub>")
				.Replace("▲", "</sub>")
				.Replace("●", "<sup>")
				.Replace("◎", "</sup>");
			
		
			return s;
		}
			static string TranslateChineseNumber(string s)
		{
			
			const string chinese = "一二三四五六七八九零";
			var numbers = "1234567890";
			foreach (var element in chinese) {
				
				s = s.Replace(element, numbers[chinese.IndexOf(element)]);
				
			}
			s = s.Replace("十", "10");
			return s;
		}
		static string TranslateChineseNumberSymbol(string s)
		{
			
			const string chinese = "①②③④⑤⑥⑦⑧⑨";
			var numbers = "123456789";
			foreach (var element in chinese) {
				
				s = s.Replace(element, numbers[chinese.IndexOf(element)]);
				
			}
			
			return s;
		}
		public static string RemoveRedundancyWhiteSpace(this string str)
		{
			var lines = str.Trim().Split('\n');
			StringBuilder sb = new StringBuilder();
			
			for (int i = 0, j = lines.Count(); i < j; i++) {
				string line = lines[i];
				
				sb.Append(line).Append('\n');
				
				if (string.IsNullOrWhiteSpace(lines[i])) {
					while (i + 1 < j && string.IsNullOrWhiteSpace(lines[i + 1])) {
						i++;
					}
				}
			}
			return sb.ToString();
		}
		
	}
}