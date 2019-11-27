using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Common;

namespace Helpers
{
	public static class Strings
	{public static string Repeat(this string value, int count)
		{
			if (count <= 0)
				return string.Empty;
			return	string.Concat(Enumerable.Repeat(value, count));
			
		}
		public static string FormatNginxConf(this string value)
		{
			var sb = new StringBuilder();
			var count = 0;
			value=System.Text.RegularExpressions.Regex.Replace(value,"\\s{2,}"," " );
			var length=value.Length;
			for (int i = 0; i < length; i++) {
				var item=value[i];
				if (item == '{') {
					count++;
					while(i+1<length && char.IsWhiteSpace(value[i])){
						i++;
					}
					sb.AppendLine( "{").Append("\t".Repeat(count) );
				} else if (item == '}') {
					count--;
					while(i+1<length && char.IsWhiteSpace(value[i])){
						i++;
					}
					sb.AppendLine("\t".Repeat(count) + "}").Append("\t".Repeat(count));

				} else if (item == ';') {
					while(i+1<length && char.IsWhiteSpace(value[i])){
						i++;
					}
					sb.AppendLine(";");
					sb.Append("\t".Repeat(count));
				} else if (item == '\r' || item == '\n' || item == '\t') {

					continue;
				}else if(item=='#'){
					while(i+1<length && value[i]!='\n'){
						i++;
					}
				//sb.Append("\n"+"\t".Repeat(count) + "#");

				} else {
					sb.Append(item);
				}

			}
		 
			return sb.ToString();
		}
		
		public static string GetCommandPath(this string fileName)
		{
			return Environment.GetCommandLineArgs()[0].GetDirectoryName().Combine(fileName);
		}


		
		public static string Combine(this string path, string fileName)
		{
			return Path.Combine(path, fileName);
		}

	
		
		public static bool CharRequiresJavaScriptEncoding(char c)
		{
			return c < 0x20// control chars always have to be encoded
			|| c == '\"'// chars which must be encoded per JSON spec
			|| c == '\\'
			|| c == '\''// HTML-sensitive chars encoded for safety
			|| c == '<'
			|| c == '>'
				//|| (c == '&' && JavaScriptEncodeAmpersand) // Bug Dev11 #133237. Encode '&' to provide additional security for people who incorrectly call the encoding methods (unless turned off by backcompat switch)
			|| c == '\u0085'// newline chars (see Unicode 6.2, Table 5-1 [http://www.unicode.org/versions/Unicode6.2.0/ch05.pdf]) have to be encoded (DevDiv #663531)
			|| c == '\u2028'
			|| c == '\u2029';
		}
		public static void AppendCharAsUnicodeJavaScript(StringBuilder builder, char c)
		{
			builder.Append("\\u");
			builder.Append(((int)c).ToString("x4", CultureInfo.InvariantCulture));
		}
		
		public static string JavaScriptStringEncode(string value)
		{
			if (String.IsNullOrEmpty(value)) {
				return String.Empty;
			}
			
			StringBuilder b = null;
			int startIndex = 0;
			int count = 0;
			for (int i = 0; i < value.Length; i++) {
				char c = value[i];
				
				// Append the unhandled characters (that do not require special treament)
				// to the string builder when special characters are detected.
				if (CharRequiresJavaScriptEncoding(c)) {
					if (b == null) {
						b = new StringBuilder(value.Length + 5);
					}
					
					if (count > 0) {
						b.Append(value, startIndex, count);
					}
					
					startIndex = i + 1;
					count = 0;
				}
				
				switch (c) {
					case '\r':
						b.Append("\\r");
						break;
					case '\t':
						b.Append("\\t");
						break;
					case '\"':
						b.Append("\\\"");
						break;
					case '\\':
						b.Append("\\\\");
						break;
					case '\n':
						b.Append("\\n");
						break;
					case '\b':
						b.Append("\\b");
						break;
					case '\f':
						b.Append("\\f");
						break;
					default:
						if (CharRequiresJavaScriptEncoding(c)) {
							AppendCharAsUnicodeJavaScript(b, c);
						} else {
							count++;
						}
						break;
				}
			}
			
			if (b == null) {
				return value;
			}
			
			if (count > 0) {
				b.Append(value, startIndex, count);
			}
			
			return b.ToString();
		}
		
		
		public static IEnumerable<T> TakeAllButLast<T>(this IEnumerable<T> source)
		{
			var it = source.GetEnumerator();
			bool hasRemainingItems = false;
			bool isFirst = true;
			T item = default(T);

			do {
				hasRemainingItems = it.MoveNext();
				if (hasRemainingItems) {
					if (!isFirst)
						yield return item;
					item = it.Current;
					isFirst = false;
				}
			} while (hasRemainingItems);
		}

		
		public static Stream OpenAsStream(this string fileName)
		{
			return	File.Open(fileName, FileMode.Open);
		}
		public static List<string> JsonValues(this string s, string key)
		{
			key = "\"" + key + "\"";
			var start = 0;
			var i = 0;
			var l = new List<string>();
			while ((i = s.IndexOf(key, start)) != -1) {
				start = i + key.Length + 1;
				
				var a1 = s.IndexOf('"', start);
				
				if (a1 == -1)
					break;
				a1++;
				start = a1;
				var a2 = s.IndexOf('"', start);
				if (a2 == -1)
					break;
				start = a2 + 1;
				var v = s.Substring(a1, a2 - a1);
				l.Add(v);
				
				
			}
			return l;
		}
		public static bool IsDigit(this string s)
		{
			
			if (string.IsNullOrEmpty(s))
				return false;
			for (int i = 0; i < s.Length; i++) {
				if (!char.IsDigit(s[i]))
					return false;
			}
			return true;
		}
		private static readonly CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		
		
//		public static string SubstringAfter(this string value, char delimiter)
//		{
//			var index = value.IndexOf(delimiter);
//			if (index == -1)
//				return value;
//			else
//				return value.Substring(index + 1);
//		}
//		public static string SubstringAfter(this string s1, string s2)
//		{
//			if (s2.Length == 0) {
//				return s1;
//			}
//			//int idx = collation.IndexOf(s1, s2);
//			int idx = compareInfo.IndexOf(s1, s2, CompareOptions.Ordinal);
//			return (idx < 0) ? string.Empty : s1.Substring(idx + s2.Length);
//		}
//		public static string SubstringAfterLast(this string value, char delimiter)
//		{
//			var index = value.LastIndexOf(delimiter);
//			if (index == -1)
//				return value;
//			else
//				return value.Substring(index + 1);
//		}
//		public static string SubstringAfterLast(this string value, string delimiter)
//		{
//			var index = value.LastIndexOf(delimiter);
//			if (index == -1)
//				return value;
//			else
//				return value.Substring(index + 1);
//		}
//		public static string SubstringBefore(this string value, char delimiter)
//		{
//			var index = value.IndexOf(delimiter);
//			if (index == -1)
//				return value;
//			else
//				return value.Substring(0, index);
//		}
//		public static string SubstringBefore(this string s1, string s2)
//		{
//			if (s2.Length == 0) {
//				return s2;
//			}
//			//int idx = collation.IndexOf(s1, s2);
//			int idx = compareInfo.IndexOf(s1, s2, CompareOptions.Ordinal);
//			return (idx < 1) ? s1 : s1.Substring(0, idx);
//		}
//		public static string SubstringBeforeLast(this string value, char delimiter)
//		{
//			var index = value.LastIndexOf(delimiter);
//			if (index == -1)
//				return value;
//			else
//				return value.Substring(0, index);
//		}
//		public static string SubstringBeforeLast(this string value, string delimiter)
//		{
//			var index = value.LastIndexOf(delimiter);
//			if (index == -1)
//				return value;
//			else
//				return value.Substring(0, index);
//		}
//		public static string RemoveWhiteSpaceLines(this string str)
//		{
//			
//			return string.Join(Environment.NewLine, str.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Where(i => !string.IsNullOrWhiteSpace(i)));
//		}
		
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
		
//		public static IEnumerable<string> ToBlocks(this string value)
//		{
//			var count = 0;
//			var sb = new StringBuilder();
//			var ls = new List<string>();
//			for (var i = 0; i < value.Length; i++) {
//				sb.Append(value[i]);
//
//				if (value[i] == '{') {
//					count++;
//				} else if (value[i] == '}') {
//					count--;
//					if (count == 0) {
//						ls.Add(sb.ToString());
//						sb.Clear();
//					}
//				}
//
//			}
//			return ls;
//
//		}
		public	static string String2Unicode(this string source)
		{
			var bytes = Encoding.Unicode.GetBytes(source);
			var stringBuilder = new StringBuilder();
			for (var i = 0; i < bytes.Length; i += 2) {
				stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
			}
			return stringBuilder.ToString();
		}
		public	static string Unicode2String(this string source)
		{
			return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, x => Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)).ToString());
		}
		
		
		public static string Crc32(this string value)
		{
			var buffer = Encoding.UTF8.GetBytes(value);
			var r = Crc32Helper.CalculateHash(Crc32Helper.DefaultSeed, buffer, 0, buffer.Length);
			return r.ToString();
		}
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (T item in items) {
				action(item);
			}
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

		public static string StringInterpolationToFormat(this string value)
		{
			var matched = new List<string>();
			var index = -1;
			var r =	Regex.Replace(value, "(?<!{){[^{}\n]*?}", new MatchEvaluator((m) => {
				matched.Add(m.Value.Trim("{}".ToArray()));
				index++;
				return "{" + index + "}";
			}));
			
			r += "," + string.Join(",", matched);
			return "string.Format(" + r.TrimStart('$') + ")";
		}
		private const string INDENT_STRING = "    ";
		public static string FormatJson(this string str)
		{
			var indent = 0;
			var quoted = false;
			var sb = new StringBuilder();
			for (var i = 0; i < str.Length; i++) {
				var ch = str[i];
				switch (ch) {
					case '{':
					case '[':
						sb.Append(ch);
						if (!quoted) {
							sb.AppendLine();
							Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
						}
						break;
					case '}':
					case ']':
						if (!quoted) {
							sb.AppendLine();
							Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
						}
						sb.Append(ch);
						break;
					case '"':
						sb.Append(ch);
						bool escaped = false;
						var index = i;
						while (index > 0 && str[--index] == '\\')
							escaped = !escaped;
						if (!escaped)
							quoted = !quoted;
						break;
					case ',':
						sb.Append(ch);
						if (!quoted) {
							sb.AppendLine();
							Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
						}
						break;
					case ':':
						sb.Append(ch);
						if (!quoted)
							sb.Append(" ");
						break;
					default:
						sb.Append(ch);
						break;
				}
			}
			return sb.ToString();
		}

		public static string AppendFileName(this string f, string n)
		{
			return	Path.Combine(Path.GetDirectoryName(f), Path.GetFileNameWithoutExtension(f) + n + Path.GetExtension(f));
		}
		
		
		
		
		public static void WriteAllText(this String path, String contents)
		{
			using (var sw = new StreamWriter(path, false, new UTF8Encoding(false)))
				sw.Write(contents);
		}
//		public static string Capitalize(this string value)
//		{
//			//  && char.IsLower(value[0])
//			if (!string.IsNullOrEmpty(value)) {
//				return value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
//			}
//			return value;
//		}
		
		public static string CapitalizeOnlyFirst(this string value)
		{
			//  && char.IsLower(value[0])
			if (!string.IsNullOrEmpty(value)) {
				return value.Substring(0, 1).ToUpper() + value.Substring(1);
			}
			return value;
		}
		public static string StripHtmlTag(this string value)
		{
			return Regex.Replace(value, "<[^>]*?>", "");
		}
		
		public static string Base64(this string s)
		{
			return	Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
		}
	}
}