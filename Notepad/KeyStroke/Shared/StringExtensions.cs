
namespace  Shared
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.IO;
	using System.Text.RegularExpressions;

	public static  class StringExtensions
	{
		static sbyte[] unhex_table = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, -1, -1, -1, -1, -1, -1
       , -1, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, 10, 11, 12, 13, 14, 15, -1, -1, -1, -1, -1, -1, -1, -1, -1
       , -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
		};
		//public static string GetRandomStringAlpha(this int length)
		//{

		//    StringBuilder builder = new StringBuilder();
		//    char ch;
		//    for (int i = 0; i < length; i++)
		//    {
		//        ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * s_nameRand.NextDouble() + 65)));
		//        builder.Append(ch);
		//    }
		//    return builder.ToString();
		//}
		public static StringBuilder Append(this String value)
		{
			return new StringBuilder().Append(value);
		}

		public static StringBuilder AppendLine(this String value)
		{
			return  new StringBuilder().AppendLine(value);
		}
		public static string Capitalize(this string value)
		{
			//  && char.IsLower(value[0])
			if (!string.IsNullOrEmpty(value)) {
				return value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
			}
			return value;
		}
		public static int ConvertToInt(this string value, int defaultValue = 0)
		{
			var match = Regex.Match(value, "[0-9]+");
			if (match.Success) {
				return int.Parse(match.Value);
			}
			return defaultValue;
		}

		public static int CountStart(this string value, char c)
		{
			var count = 0;

			foreach (var item in value) {
				if (item == c)
					count++;
				else
					break;
			}
			return count;
		}
		public static string EscapeString(this string s)
		{
			char[] cs = new []{ '\\', '"', '\'', '<', '>' };
			string[] ss = cs.Select(i => "\\u" + ((int)i).ToString("x4")).ToArray();
			for (int i = 0; i < cs.Length; i++) {
				s = s.Replace(cs[i].ToString(), ss[i]);
			}
			return s;
		}
		public static string GetDesktopPath(this string f)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), f);
		}
		public static string GetFirstReadable(this string value)
		{
			return  value.TrimStart().Split(new char[] { '\n' }, 2).First().Trim();
		}
		public static string GetRandomString(this int length)
		{
			Random s_nameRand = new Random();//new Random((int)(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds()));

			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
			return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[s_nameRand.Next(s.Length)]).ToArray());
		}

		public static int HexToInt(this string hexNumber)
		{
			int decValue = unhex_table[(byte)hexNumber[0]];
			for (int i = 1; i < hexNumber.Length; i++) {
				decValue *= 16;
				decValue += unhex_table[(byte)hexNumber[i]];
			}
			return decValue;
		}
		public static bool IsReadable(this string value)
		{
			return  !string.IsNullOrWhiteSpace(value);
		}
		public static bool IsVacuum(this string value)
		{
			return  string.IsNullOrWhiteSpace(value);
		}
	
		public static bool IsWhiteSpace(this String value)
		{
			if (value == null)
				return true;

			for (int i = 0; i < value.Length; i++) {
				if (!Char.IsWhiteSpace(value[i]))
					return false;
			}

			return true;
		}
        
		public static IEnumerable<string> Matches(this string value, string pattern)
		{
			var match = Regex.Match(value, pattern);

			while (match.Success) {

				yield return match.Value;
				match = match.NextMatch();
			}
		}
		public static IEnumerable<string> MatchesByGroup(this string value, string pattern)
		{
			var match = Regex.Match(value, pattern);

			while (match.Success) {

				yield return match.Groups[1].Value;
				match = match.NextMatch();
			}
		}
		public static IEnumerable<string> MatchesMultiline(this string value, string pattern)
		{
			var match = Regex.Match(value, pattern, RegexOptions.Multiline);

			while (match.Success) {

				yield return match.Value;
				match = match.NextMatch();
			}
		}
		public static string SubstringAfter(this string value, string delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + delimiter.Length);
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
		public static string SubstringBefore(this string value, string delimiter)
		{
			var index = value.IndexOf(delimiter);
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
		public static IEnumerable<string> ToLines(this string value)
		{
			return  value.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim());
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
