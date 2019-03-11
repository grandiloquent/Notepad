
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Utils
{
	
	public static class StringExtensions
		
	{	
		
		

		public static string ToLine(this IEnumerable<string> value, string separator = "\r\n")
		{
			return string.Join(separator, value);
		}
	
		public		  static char ToLowerAsciiInvariant(this char c)
		{
			if ('A' <= c && c <= 'Z') {
				c = (char)(c | 0x20);
			}
			return c;
		}
		
		public static string StripHtmlTag(this string value)
		{
			return Regex.Replace(value, "<[^>]*?>", "");
		}
		
		public   static  char ToUpperAsciiInvariant(this char c)
		{
			if ('a' <= c && c <= 'z') {
				c = (char)(c & ~0x20);
			}
			return c;
		}
		
		
		public 	 static   bool IsAscii(this char c)
		{
			return c < 0x80;
		}
		
		

		public static bool IsReadable(this string value)
		{
			return !string.IsNullOrWhiteSpace(value);
		}
		
		public static bool IsVacuum(this string value)
		{
			return string.IsNullOrWhiteSpace(value);
		}
		public static string Capitalize(this string value)
		{
			//  && char.IsLower(value[0])
			if (!string.IsNullOrEmpty(value)) {
				return value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
			}
			return value;
		}
		public static string GetFirstReadable(this string value)
		{
			return  value.TrimStart().Split(new char[] { '\n' }, 2).First().Trim();
		}
		public static string TrimNonLetterOrDigitStart(this string value)
		{
			var len = value.Length;
			var pos = 0;
//			int a='`';
//			int a1='a';
//			int a2='z';
//			int a3='A';
//			int a4='Z';
//			int a5='0';
//			int a6='9';
			
			for (int i = 0; i < len; i++) {
				if (('a' <= value[i] && value[i] <= 'z') ||
				    ('A' <= value[i] && value[i] <= 'Z') ||
				    ('0' <= value[i] && value[i] <= '9'))
					break;
				pos = i;
			}
			if (pos > 0)
				return value.Substring(pos + 1);
			return value;
		}
		
	
	}
}
