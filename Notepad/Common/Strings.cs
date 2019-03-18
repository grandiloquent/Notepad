namespace Common
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Linq;
	public static  class Strings
	{
		private static readonly CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		private const string Quote = "\"";
		public static string Decapitalize(this string value)
		{
			//  && char.IsLower(value[0])
			if (!string.IsNullOrEmpty(value)) {
				return value.Substring(0, 1).ToLower() + value.Substring(1);
			}
			return value;
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
		public static string RemoveWhiteSpaceLines(this string str)
		{
			
			return string.Join(Environment.NewLine, str.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Where(i=>!string.IsNullOrWhiteSpace(i)));
		}public static string Concatenate(this IEnumerable<string> strings)
		{
			return strings.Concatenate((builder, nextValue) => builder.Append(nextValue));
		}
		private static string Concatenate(this IEnumerable<string> strings,
			Func<StringBuilder, string, StringBuilder> builderFunc)
		{
			return strings.Aggregate(new StringBuilder(), builderFunc).ToString();
		}
		public static string ConcatenateLines(this IEnumerable<string> strings)
		{
			return strings.Concatenate((builder, nextValue) => builder.AppendLine(nextValue));
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
		public static string[] ToLines(this string value)
		{
			return value.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
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
		public static string TrimComments(this string code)
		{
			const string re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
			return Regex.Replace(code, re, "$1");
		}
		public static string UpperCase(this string value)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < value.Length; i++) {
				if (i != 0 && char.IsUpper(value[i])) {
					sb.Append('_').Append(char.ToUpper(value[i]));
					
				} else {
					sb.Append(char.ToUpper(value[i]));
				}
				
			}
			return sb.ToString();
		}
	}
}