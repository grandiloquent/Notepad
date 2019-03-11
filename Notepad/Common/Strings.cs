namespace Common
{
	using System.Globalization;
	using System.Text;
	using System;
	using System.IO;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	
	public static class Strings
	{
		private static readonly CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
public static string SubstringAfter(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string SubstringAfter(this string s1, string s2)
		{
			if (s2.Length == 0) {
				return s1;
			}
			//int idx = collation.IndexOf(s1, s2);
			int idx = compareInfo.IndexOf(s1, s2, CompareOptions.Ordinal);
			return (idx < 0) ? string.Empty : s1.Substring(idx + s2.Length);
		}
		public static string SubstringAfterLast(this string value, char delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string SubstringAfterLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string SubstringBefore(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringBefore(this string s1, string s2)
		{
			if (s2.Length == 0) {
				return s2;
			}
			//int idx = collation.IndexOf(s1, s2);
			int idx = compareInfo.IndexOf(s1, s2, CompareOptions.Ordinal);
			return (idx < 1) ? s1 : s1.Substring(0, idx);
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
		public static Stream ToStream(this string str)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(str);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}
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
		public static string[] ToLines(this string value)
		{
			return value.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
		}
		public static string TrimComments(this string code)
		{
			const string re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
			return Regex.Replace(code, re, "$1");
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
	}
}