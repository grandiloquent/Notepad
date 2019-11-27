namespace StringCompare
{

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using Common;
	public static class StringDelegate
	{
		private const string Quote = "\"";
		
		
		public static string StringbuilderizeInCsInternal(string txt)
		{
			StringBuilder stringBuilder = new StringBuilder(txt);
			stringBuilder.Replace("\"", "\"\"");
			string s = stringBuilder.ToString();
			stringBuilder.Clear();
			using (StringReader stringReader = new StringReader(s)) {
				string value;
				while ((value = stringReader.ReadLine()) != null) {
					stringBuilder.Append("sb.AppendLine(");
					stringBuilder.Append("@\"");
					stringBuilder.Append(value);
					stringBuilder.AppendLine("\");");
				}
			}
			stringBuilder.Replace("@\"\" + ", "");
			stringBuilder.Insert(0, "var sb = new System.Text.StringBuilder(" + txt.Length + ");" + Environment.NewLine);
			return stringBuilder.ToString();
		}


		public static string ToSingleLineInternal(string s)
		{
			s = Regex.Replace(s, "[\t\r\n]+", " ");
			s = Regex.Replace(s, "\\s{2,}", " ");
			return s;
		}

		

		[BindMenuItem(Control = "单行Button", Toolbar = "toolbar2")]
		public static void ToSingleLine()
		{
			Methods.OnClipboardString(ToSingleLineInternal);
		}

		[BindMenuItem(Control = "StringBuilderButton", Toolbar = "toolbar2")]
		public static void StringbuilderizeInCs()
		{
			Methods.OnClipboardString(StringbuilderizeInCsInternal);
		}
		[BindMenuItem(Control = "倒序Button", Toolbar = "toolbar2")]
		public static void ReverseLines()
		{
			Methods.OnClipboardString((string v) => (from i in v.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries)
			                                         select i.Trim()).Reverse().ConcatenateLines());
		}

		[BindMenuItem(Control = "排序Button", Toolbar = "toolbar2")]
		public static void SortLines()
		{
			Methods.OnClipboardString((string v) => (from i in (from i in v.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries)
			                                                    select i.Trim()).Distinct()
			                                         orderby i
			                                         select i).ConcatenateLines());
		}

		[BindMenuItem(Control = "stringCommentButton", Toolbar = "toolbar2")]
		public static void FormatComment()
		{
			Methods.OnClipboardString(delegate(string s) {
				IEnumerable<string> enumerable = from i in Regex.Split(s, "\\s+")
				                                 where !string.IsNullOrWhiteSpace(i)
				                                 select i;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("//");
				List<string> list = new List<string>();
				foreach (string item in enumerable) {
					if (stringBuilder.Length > 50) {
						list.Add(stringBuilder.ToString());
						stringBuilder.Clear();
						stringBuilder.Append("//");
					}
					stringBuilder.Append(' ').Append(item);
				}
				if (stringBuilder.Length > 0) {
					list.Add(stringBuilder.ToString());
				}
				return list.ConcatenateLines();
			});
		}
	}
}
