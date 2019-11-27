
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common
{
	public static class Strings
	{
		
		public static readonly char VolumeSeparatorChar = ':';
		public static readonly char DirectorySeparatorChar = '\\';
		internal const string DirectorySeparatorCharAsString = "\\";
		public static readonly char AltDirectorySeparatorChar = '/';
		public static string Decapitalize(this string value)
		{
			//  && char.IsLower(value[0])
			if (!string.IsNullOrEmpty(value)) {
				return value.Substring(0, 1).ToLower() + value.Substring(1);
			}
			return value;
		}
		public static string Capitalize(this string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return text;
			if (char.IsUpper(text[0]))
				return text;
			return string.Format("{0}{1}", char.ToUpper(text[0]), text.Substring(1));
		}
		
		public static byte[] ConvertToBytes(this int i)
		{
			byte[] intBytes = BitConverter.GetBytes(i);
			if (BitConverter.IsLittleEndian)
				Array.Reverse(intBytes);
			return intBytes;
		}
		
		public static IEnumerable<string> Lines(this string s)
		{
			var lines = s.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			
			return lines.Select(i => i.Trim()).OrderBy(i => i);
		}
		public static string SubstringBeforeLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		
		
		public static string SubstringBefore(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringAfterLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		
		public static string ConvertToCamelCase(this string text)
		{
			String[] strings = text.Split(new char[]{ '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
			if (strings.Length > 0) {
				StringBuilder buf = new StringBuilder();
				buf.Append(strings[0].ToLower());
				for (int i = 1; i < strings.Length; i++) {
					String s = strings[i];
					buf.Append(s.ToLower().Capitalize());
				}
				return	buf.ToString();
			}
			return string.Empty;
		}
		public static void ReadStream(this string path, Action<Stream> action)
		{
			using (var s = File.OpenRead(path)) {
				action(s);
			}
		}
		
		public static string SubstringAfter(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string FormatString(this string s)
		{
			
			s =	Regex.Replace(s, "[\r\n]+", "");
			s = Regex.Replace(s, "\\s+", " ");
			s = s.Replace(";", ";" + Environment.NewLine);
			return s;
		}
		public static string GetEntryPath(this string fileName)
		{
			return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), fileName);
		}
		public static string GetDesktopPath(this string f)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), f);
		}
		
		public static string GetValidFileName(this string value, char c = ' ')
		{
			var chars = Path.GetInvalidFileNameChars();
			return new string(value.Select<char, char>((i) => {
				if (chars.Contains(i))
					return c;
				return i;
			}).Take(125).ToArray()).Trim();
		}
		
		
		
		
		public static string AppendFileName(this string f, string n)
		{
			return	Path.Combine(Path.GetDirectoryName(f), Path.GetFileNameWithoutExtension(f) + n + Path.GetExtension(f));
		}
		public static string GetFileNameWithoutExtension(this string fileName)
		{
			return Path.GetFileNameWithoutExtension(fileName);
		}
		
		public static string GetExtension(this string path)
		{
			return Path.GetExtension(path);
		}
		public static void WriteAllText(this String path, String contents)
		{
			using (var sw = new StreamWriter(path, false, new UTF8Encoding(false)))
				sw.Write(contents);
		}
		internal static bool IsDirectorySeparator(char c)
		{
			return c == '\\' || c == '/';
		}

		public static string ChangeExtension(this string path, string extension)
		{
			if (path != null) {
				string text = path;
				for (int num = path.Length - 1; num >= 0; num--) {
					char c = path[num];
					if (c == '.') {
						text = path.Substring(0, num);
						break;
					}
					if (IsDirectorySeparator(c)) {
						break;
					}
				}
				if (extension != null && path.Length != 0) {
					text = ((extension.Length == 0 || extension[0] != '.') ? (text + "." + extension) : (text + extension));
				}
				return text;
			}
			return null;
		}
		private const string parentSymbol = "..\\";
		private const string absoluteSymbol = "./";
		public static String GetFullPath(this string relativePath,string replacePath=null)
		{
			if(replacePath==null)
			  replacePath = AppDomain.CurrentDomain.BaseDirectory;
			int parentStart = relativePath.IndexOf(parentSymbol);
			int absoluteStart = relativePath.IndexOf(absoluteSymbol);
			if (parentStart >= 0) {
				int parentLength = 0;
				while (relativePath.Substring(parentStart + parentLength).Contains(parentSymbol)) {
					replacePath = new DirectoryInfo(replacePath).Parent.FullName;
					parentLength = parentLength + parentSymbol.Length;
				}
				;
				relativePath = relativePath.Replace(relativePath.Substring(parentStart, parentLength), string.Format("{0}\\", replacePath));
			} else if (absoluteStart >= 0) {
				relativePath = replacePath+relativePath.Substring(1);
			}
			return relativePath;
		}
		public static bool IsReadable(this string value)
		{
			return !string.IsNullOrWhiteSpace(value);
		}
		public static string GetDirectoryName(this string v)
		{
			return Path.GetDirectoryName(v);
		}
		public static string Combine(this string path, string fileName)
		{
			return Path.Combine(path, fileName);
		}
      public static string ChangeFileName(this string path, string fileNameWithoutExtension)
		{
			return Path.Combine(Path.GetDirectoryName(path), fileNameWithoutExtension + Path.GetExtension(path));
		}
		public static void WriteAllLines(this string path, IEnumerable<string> contents)
		{
			using (StreamWriter streamWriter = new StreamWriter(path, false, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false))) {
				foreach (string content in contents) {
					streamWriter.WriteLine(content);
				}
			}
		}

		public static string RemoveWhiteSpaceLines(this string str)
		{
			
			return string.Join(Environment.NewLine, str.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Where(i => !string.IsNullOrWhiteSpace(i)));
		}
		public static string Concatenate(this IEnumerable<string> strings)
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
		public static string Concatenates(this IEnumerable<string> strings)
		{
			return strings.Concatenate((builder, nextValue) => builder.Append(nextValue));
		}
//		public static void ForEach<TObject>(this IEnumerable<TObject> collection, Action<TObject> action)
//		{
//			if (action == null) {
//				throw new ArgumentNullException("action");
//			}
//			if (collection != null) {
//				foreach (TObject current in collection) {
//					if (current != null) {
//						action(current);
//					}
//				}
//			}
//		}
		public static IEnumerable<FObject> ForEach<TObject,FObject>(this IEnumerable<TObject> collection, Func<TObject,FObject> action)
		{
			if (action == null) {
				throw new ArgumentNullException("action");
			}
			if (collection != null) {
				foreach (TObject current in collection) {
					if (current != null) {
						yield return action(current);
					}
				}
			}
		}
//		public static IEnumerable<TSource> DistinctBy<TSource, TKey>
//			(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
//		{
//			HashSet<TKey> seenKeys = new HashSet<TKey>();
//			foreach (TSource element in source) {
//				if (seenKeys.Add(keySelector(element))) {
//					yield return element;
//				}
//			}
//		}
		public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
		{
			int num = 0;
			foreach (T item in items) {
				action(item, num);
				num++;
			}
		}
		public static String GetFileName(this String path)
		{
			if (path != null) {
				
				int length = path.Length;
				for (int i = length; --i >= 0;) {
					char ch = path[i];
					if (ch == DirectorySeparatorChar || ch == AltDirectorySeparatorChar || ch == VolumeSeparatorChar)
						return path.Substring(i + 1, length - i - 1);
					
				}
			}
			return path;
		}
		public static bool CreateDirectoryIfNotExists(this string dir)
		{
			if (Directory.Exists(dir)) {
				return true;
			}
			return Directory.CreateDirectory(dir).Exists;
		}
		public static int ConvertToInt(this string s, int v = 0)
		{
			StringBuilder sb = null;
			
			var x = s.Length;
			var i = 0;
			while (i < x && !char.IsDigit(s[i])) {
				i++;
			}
			
			while (i < x && char.IsDigit(s[i])) {
				if (sb == null)
					sb = new StringBuilder();
				sb.Append(s[i]);
				i++;
			}
			
			if (sb != null)
				return int.Parse(sb.ToString());
			return v;
		}
		
		public static String ReadAllText(this String path)
		{
			using (StreamReader sr = new StreamReader(path, new UTF8Encoding(false), true, 1024))
				return sr.ReadToEnd();
		}
		public static string SubstringAfterLast(this string value, char delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			return index == -1 ? value : value.Substring(index + 1);
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
		public static string SubstringAfter(this string value, string delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + delimiter.Length);
		}
		public static string RemoveWhiteSpace(this string value)
		{
			return Regex.Replace(value, "\\s+", "");
		}
		public	static String FormatFileSize(this ulong bytes)
		{
			string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
			if (bytes == 0)
				return "0" + suf[0];
			int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
			double num = Math.Round(bytes / Math.Pow(1024, place), 1);
			return (Math.Sign((float)bytes) * num).ToString() + suf[place];
		}
		public static string SubstringAtferLast(this string value, char separator)
		{
			var index = value.LastIndexOf(separator);
			return index == -1 ? value : value.Substring(index + 1);
		}
		
	}
}