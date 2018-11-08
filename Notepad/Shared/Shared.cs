
#define ENTITY_ENCODE_HIGH_ASCII_CHARS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;


namespace Shared
{


  
     
	public static class StringExtensions
	{
		public static string GetDesktopPath(this string f)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), f);
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
		public static string EscapeString(this string s)
		{
			char[] cs = new []{ '\\', '"', '\'', '<', '>' };
			string[] ss = cs.Select(i => "\\u" + ((int)i).ToString("x4")).ToArray();
			for (int i = 0; i < cs.Length; i++) {
				s = s.Replace(cs[i].ToString(), ss[i]);
			}
			return s;
		}
		public static string SubstringAfterLast(this string value, char delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
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
		public static string SubstringAfter(this string value, string delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + delimiter.Length);
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
		public static string Capitalize(this string value)
		{
			//  && char.IsLower(value[0])
			if (!string.IsNullOrEmpty(value)) {
				return value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
			}
			return value;
		}
        
		public static IEnumerable<string> Matches(this string value, string pattern)
		{
			var match = Regex.Match(value, pattern);

			while (match.Success) {

				yield return match.Value;
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
		public static IEnumerable<string> MatchesByGroup(this string value, string pattern)
		{
			var match = Regex.Match(value, pattern);

			while (match.Success) {

				yield return match.Groups[1].Value;
				match = match.NextMatch();
			}
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
		public static int ConvertToInt(this string value, int defaultValue = 0)
		{
			var match = Regex.Match(value, "[0-9]+");
			if (match.Success) {
				return int.Parse(match.Value);
			}
			return defaultValue;
		}
		public static string GetRandomString(this int length)
		{
			Random s_nameRand = new Random();//new Random((int)(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds()));

			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
			return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[s_nameRand.Next(s.Length)]).ToArray());
		}
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
			return  new StringBuilder().AppendLine(value);		}
		public static bool IsVacuum(this string value)
		{
			return  string.IsNullOrWhiteSpace(value);		}
		public static bool IsReadable(this string value)
		{
			return  !string.IsNullOrWhiteSpace(value);		}
		public static string GetFirstReadable(this string value)
		{
			return  value.TrimStart().Split(new char[] { '\n' }, 2).First().Trim();		}
		public static IEnumerable<string> ToLines(this string value)
		{
			return  value.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim());		}

	}

	public static class GenericExtensions
	{
		public static IEnumerable<T> Distinct<T, U>(
			this IEnumerable<T> seq, Func<T, U> getKey)
		{
			return
                from item in seq
			             group item by getKey(item) into gp
			             select gp.First();
		}
	}

	public static class NetHttpExtensions
	{
		public static HttpClient GetHttpClient(this string url)
		{
			var client = new HttpClient(new HttpClientHandler {
				AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
				UseProxy = false,
			});
			client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
			client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 9_1 like Mac OS X) AppleWebKit/601.1.46 (KHTML, like Gecko) Version/9.0 Mobile/13B143 Safari/601.1");
			return client;
		}
	}

	public static class FileExtensions
	{
		private static readonly char[] InvalidFileNameChars = {
			'\"',
			'<',
			'>',
			'|',
			'\0',
			':',
			'*',
			'?',
			'\\',
			'/'
		};



		public static string GetDirectoryFileName(this string v)
		{
			return Path.GetFileName(Path.GetDirectoryName(v));
		}

		public static string GetApplicationPath(this string v)
		{
			return Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), v);
		}

		public static string GetUniqueFileName(this String v)
		{
			int i = 1;
			Regex regex = new Regex(" \\- [0-9]+");
			String t = Path.Combine(Path.GetDirectoryName(v),
				           regex.Split(Path.GetFileNameWithoutExtension(v), 2).First() + " - " + i.ToString().PadLeft(3, '0') +
				           Path.GetExtension(v));

			while (File.Exists(t)) {
				i++;
				t = Path.Combine(Path.GetDirectoryName(v),
					regex.Split(Path.GetFileNameWithoutExtension(v), 2).First() + " - " + i.ToString().PadLeft(3, '0') +
					Path.GetExtension(v));
			}
			return t;
		}

		public static string GetValidFileName(this String v)
		{
			if (v == null)
				return null;
			// (Char -> Int) 1-31 Invalid;
			List<char> chars = new List<char>(v.Length);

			for (int i = 0; i < v.Length; i++) {
				if (InvalidFileNameChars.Contains(v[i])) {
					chars.Add(' ');
				} else {
					chars.Add(v[i]);
				}
			}

			return new String(chars.ToArray());
		}
		public static string GetFileSha1(this string path)
		{
			using (FileStream fs = new FileStream(path, FileMode.Open))
			using (BufferedStream bs = new BufferedStream(fs))
			using (var reader = new StreamReader(bs)) {
				using (System.Security.Cryptography.SHA1Managed sha1 = new System.Security.Cryptography.SHA1Managed()) {
					byte[] hash = sha1.ComputeHash(bs);
					StringBuilder formatted = new StringBuilder(2 * hash.Length);
					foreach (byte b in hash) {
						formatted.AppendFormat("{0:X2}", b);
					}
				}
				return reader.ReadToEnd();
			}
		}
		public static void FileCopy(this string path, string dstPath)
		{
			File.Copy(path, dstPath, false);
		}
		public static string GetUniqueImageRandomFileName(this string path)
		{
			var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Select(i => i.GetFileNameWithoutExtension());
			var fileName = 8.GetRandomString();
			while (files.Contains(fileName))
				fileName = 8.GetRandomString();

			return fileName;
		}
		public static String GetCommandPath(this string fileName)
		{
			var dir = System.Reflection.Assembly.GetEntryAssembly().Location.GetDirectoryName();
			return dir.Combine(fileName);
		}
		public static UTF8Encoding sUTF8Encoding = new UTF8Encoding(false);

		public static IEnumerable<string> GetFiles(this String path, string pattern = "*")
		{
			foreach (var item in Directory.GetFiles(path, pattern)) {
				yield return item;
			}
		}
		public static DirectoryInfo GetParent(this String path)
		{
			return  Directory.GetParent(path);		}
		public static void CreateDirectoryIfNotExists(this String path)
		{
			if (Directory.Exists(path))
				return;
			Directory.CreateDirectory(path);
		}
		public static bool DirectoryExists(this String path)
		{
			return  Directory.Exists(path);		}
		public static string ChangeFileName(this string path, string fileNameWithoutExtension)
		{
			return Path.Combine(Path.GetDirectoryName(path), fileNameWithoutExtension + Path.GetExtension(path));
		}
		public static void DirectoryDelete(this String path, bool recursive = false)
		{
			if (recursive) {
				Directory.Delete(path, true);
			} else {
				Directory.Delete(path);
			}
		}
		public static void WriteAllText(this String path, String contents)
		{
			File.WriteAllText(path, contents, sUTF8Encoding);		}
		public static void WriteAllBytes(this String path, byte[] bytes)
		{
			File.WriteAllBytes(path, bytes);		}
		public static void WriteAllLines(this String path, String[] contents)
		{
			File.WriteAllLines(path, contents, sUTF8Encoding);		}
		public static void AppendAllText(this String path, String contents)
		{
			File.AppendAllText(path, contents, sUTF8Encoding);		}
		public static void AppendAllLines(this String path, IEnumerable<String> contents, Encoding encoding)
		{
			File.AppendAllLines(path, contents, sUTF8Encoding);		}
		public static void FileMove(this String sourceFileName, String destFileName)
		{
			File.Move(sourceFileName, destFileName);		}
		public static void FileReplace(this String sourceFileName, String destinationFileName, String destinationBackupFileName)
		{
			File.Replace(sourceFileName, destinationFileName, destinationBackupFileName);		}
		public static StreamReader OpenText(this String path)
		{
			return  File.OpenText(path);		}
		public static StreamWriter CreateText(this String path)
		{
			return  File.CreateText(path);		}
		public static StreamWriter AppendText(this String path)
		{
			return  File.AppendText(path);		}
		public static FileStream Create(this String path)
		{
			return  File.Create(path);		}
		public static FileStream Open(this String path, FileMode mode, FileAccess access)
		{
			return  File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);		}
		public static DateTime FileGetCreationTimeUtc(this String path)
		{
			return  File.GetCreationTimeUtc(path);		}
		public static DateTime FileGetLastAccessTimeUtc(this String path)
		{
			return  File.GetLastAccessTimeUtc(path);		}
		public static DateTime FileGetLastWriteTimeUtc(this String path)
		{
			return  File.GetLastWriteTimeUtc(path);		}
		public static FileStream OpenRead(this String path)
		{
			return  File.OpenRead(path);		}
		public static FileStream OpenWrite(this String path)
		{
			return  File.OpenWrite(path);		}
		public static String ReadAllText(this String path)
		{
			return  File.ReadAllText(path, new UTF8Encoding(false));		}
		public static IEnumerable<string> ReadLines(this String path)
		{
			return  File.ReadLines(path, new UTF8Encoding(false));
		}
		public static byte[] ReadAllBytes(this String path)
		{
			return  File.ReadAllBytes(path);		}
		public static String ChangeExtension(this String path, String extension)
		{
			return  Path.ChangeExtension(path, extension);		}
		public static string GetDirectoryName(this string path)
		{
			return  Path.GetDirectoryName(path);		}
		public static String GetExtension(this String path)
		{
			return  Path.GetExtension(path);		}
		public static String GetFullPath(this String path)
		{
			return  Path.GetFullPath(path);		}
		public static String GetFileName(this String path)
		{
			return  Path.GetFileName(path);		}
		public static String GetFileNameWithoutExtension(this String path)
		{
			return  Path.GetFileNameWithoutExtension(path);		}
		public static String GetPathRoot(this String path)
		{
			return  Path.GetPathRoot(path);		}
		public static String Combine(this String path1, String path2)
		{
			return  Path.Combine(path1, path2);		}
		public static string ToLine(this IEnumerable<string> value, string separator = "\r\n")
		{
			return string.Join(separator, value);
		}
		public static bool FileExists(this String path)
		{
			return  File.Exists(path);		}
		public static string GetValidFileName(this string value, char c)
		{

			var chars = Path.GetInvalidFileNameChars();

			return new string(value.Select<char, char>((i) => {
				if (chars.Contains(i))
					return c;
				return i;
			}).Take(125).ToArray());
		}

	}
}
