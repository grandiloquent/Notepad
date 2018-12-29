
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
		private const string Quote = "\"";
		
		public static string StringbuilderizeInCs(this string txt, string sbName = "stringBuilder")
		{
			// https://github.com/martinjw/SmartPaster2013
    	 	
			//sb to work with
			var sb = new StringBuilder(txt);

			//escape \,", and {}
			sb.Replace(Quote, Quote + Quote);

			//process the passed string (txt), one line at a time

			//dump the stringbuilder into a temp string
			string fullString = sb.ToString();
			sb.Clear(); //lovely .net 4 - sb.Remove(0, sb.Length);

			//the original was horrible
			using (var reader = new StringReader(fullString)) {
				string line;
				while ((line = reader.ReadLine()) != null) {
					sb.Append(sbName + ".AppendLine(");
					sb.Append("@" + Quote);
					sb.Append(line);
					sb.AppendLine(Quote + ");");
				}
			}

			//TODO: Better '@"" + ' replacement to not cover inside strings
			sb.Replace("@" + Quote + Quote + " + ", "");

			//add the dec statement
			sb.Insert(0, "var " + sbName + " = new System.Text.StringBuilder(" + txt.Length + ");" + Environment.NewLine);

			//and return
			return sb.ToString();
		}
		public static string SubstringBefore(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringBefore(this string value, string delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringBeforeLast(this string value, char delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringBeforeLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}

		public static IEnumerable<string> ToBlocks(this string value)
		{
			var count = 0;
			var sb = new StringBuilder();
			var ls = new List<string>();
			for (var i = 0; i < value.Length; i++) {
				sb.Append(value[i]);

				if (value[i] == '{') {
					count++;
				} else if (value[i] == '}') {
					count--;
					if (count == 0) {
						ls.Add(sb.ToString());
						sb.Clear();
					}
				}

			}
			return ls;

		}
		public static string ToLine(this IEnumerable<string> value, string separator = "\r\n")
		{
			return string.Join(separator, value);
		}
		public static string[] ToLines(this string value)
		{
			return value.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
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
		public static string TrimComments(this string code)
		{
			const string re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
			return Regex.Replace(code, re, "$1");
		}
		
		public 	 static   bool IsAscii(this char c)
		{
			return c < 0x80;
		}
		
		public static string LiterallyInCs(this string txt)
		{
			//escape appropriately
			//escape the quotes with ""
			txt = txt.Trim() //ignore leading and trailing blank lines
                .Replace("\\", "\\\\") //escape backslashes
                .Replace(Quote, "\\\"") //escape quotes
                .Replace("\t", "\\t") //escape tabs
                .Replace("\r", "\\r") //cr
                .Replace("\n", "\\n") //lf
			//.Replace("\"\" + ", "") //"" +
				.Replace("\\r\\n", "\" + Environment.NewLine + \r\n\""); //escaped crlf to Env.NewLine;

			return Quote + txt + Quote;
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
		
		public static string SubstringAfter(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string SubstringAfter(this string value, string delimiter)
		{
			var index = value.IndexOf(delimiter);
			return index == -1 ? value : value.Substring(index + delimiter.Length);
		}
		public static string SubstringAfterLast(this string value, char delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			return index == -1 ? value : value.Substring(index + 1);
		}
		public static string SubstringAfterLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			return index == -1 ? value : value.Substring(index + 1);
		}
	}
}
