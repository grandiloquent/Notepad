
#define ENTITY_ENCODE_HIGH_ASCII_CHARS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net;

using System.Windows.Forms;
using System.Diagnostics;

namespace Shared
{
	public static class WinForms
	{
		public static void InvokeWkhtmltopdf(string f)
		{

			if (!File.Exists(Path.Combine(f, "目录.html")))
				return;

			var styleFile = "safari".GetDesktopPath().Combine("style.css");
			if (File.Exists(styleFile)) {
				var targetStyleFile = Path.Combine(f, "style.css");
				if (File.Exists(targetStyleFile))
					File.Delete(targetStyleFile);
				File.Copy(styleFile, targetStyleFile);
			}
			var hd = new HtmlAgilityPack.HtmlDocument();
			hd.LoadHtml(Path.Combine(f, "目录.html").ReadAllText());
			var nodes = hd.DocumentNode.SelectNodes("//a");
			var ls = new List<string>();
			foreach (var item in nodes) {
				var href = item.GetAttributeValue("href", "").Split('#').First();

				if (ls.Contains(href))
					continue;
				ls.Add(href);
			}

			var str = "\"C:\\wkhtmltox\\wkhtmltopdf.exe\"";
			var arg = "--footer-center [page] -s Letter " + string.Join(" ", ls.Select(i => string.Format("\"{0}\"", i))) + string.Format("  \"{0}.pdf\"", f);

			var p = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo {
				FileName = "wkhtmltopdf.exe",
				Arguments = arg,
				WorkingDirectory = f
			});
			p.WaitForExit();
		}
		public static void CleanHtmls()
		{
			OnClipboardDirectory((dir) => {
				var diretories = Directory.GetDirectories(dir);
				foreach (var r in diretories) {
					const string str = "<div><div><img src=\"./images/\"><div><div><div><button><svg><g><g><g><rect></rect><title>Playlists</title><path></path><circle></circle><circle></circle><rect></rect><rect></rect><rect></rect></g></g></g></svg><div>Add&nbsp;To</div></button></div></div></div></div></div>";
					const string str1 = "<div><div><img><div><div><div><button><svg><g><g><g><rect></rect><title>Playlists</title><path></path><circle></circle><circle></circle><rect></rect><rect></rect><rect></rect></g></g></g></svg><div>Add&nbsp;To</div></button></div></div></div></div></div>";
					
					var files = Directory.GetFiles(r, "*.html", SearchOption.TopDirectoryOnly);
					foreach (var element in files) {
						var sv = Regex.Replace(element.ReadAllText().Replace(str, "").Replace(str1, ""), "(style|width|height)=\"[^\"]*?\"", "");
					
						
						element.WriteAllText(sv);
					}
				}
			});
			
			
		}
		public static void RemoveAria2File()
		{
			OnClipboardDirectory((dir) => {
				var directories = Directory.GetDirectories(dir);
				foreach (var element in directories) {
					var files = Directory.GetFiles(element, "*", SearchOption.AllDirectories);
					var filesAria2 = files.Where(i => i.EndsWith(".aria2")).Select(i => i.GetFileNameWithoutExtension()).OrderBy(i => i).Distinct().ToArray();
					foreach (var f in files) {
						if (filesAria2.Contains(f.GetFileName()) || f.EndsWith(".aria2")) {
							File.Delete(f);
						}
					}
				}
			});
		}
	
		
		public static void ZipDirectories()
		{
			OnClipboardDirectory((dir) => {
				var directories = Directory.GetDirectories(dir);
				foreach (var element in directories) {
					using (var zip = new Ionic.Zip.ZipFile(Encoding.GetEncoding("gbk"))) {
						zip.AddDirectory(element);
						zip.Save(element + ".zip");
					}
					
				}
			});
		}
		
		public static void OnClipboardDirectory(Action<String> action)
		{
			try {
				var dir = Clipboard.GetText().Trim();
				var found = false;
				if (Directory.Exists(dir)) {
					found = true;
				} else {
					var ls = Clipboard.GetFileDropList();
					if (ls.Count > 0) {
						if (Directory.Exists(ls[0])) {
							dir = ls[0];
						}
					}
				}
				if (found) {
					action(dir);
				}
			} catch {
				
			}
		}
		public static void ZipAndroidProject(string dir)
		{
			 
			using (var zip = new Ionic.Zip.ZipFile(Encoding.GetEncoding("gbk"))) {

				zip.AddFiles(Directory.GetFiles(dir).Where(i => !i.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase)).ToArray(), "");
				zip.AddFiles(Directory.GetFiles(Path.Combine(dir, "app")), "app");
				zip.AddDirectory(Path.Combine(Path.Combine(dir, "app"), "src"), "app/src");
				zip.AddDirectory(Path.Combine(dir, "gradle"), "gradle");
				var targetFileName = Path.Combine(dir, Path.GetFileName(dir) + ".zip");
				var count = 0;
				while (File.Exists(targetFileName)) {
					targetFileName = Path.Combine(dir, string.Format("{0} {1:000}.zip", Path.GetFileName(dir), ++count));
				}
				zip.Save(targetFileName);
			}
		}
		public static void OnClipboardString(Func<String,String> func)
		{
			try {
				var str = Clipboard.GetText().Trim();
				if (str.IsVacuum())
					return;
				str = func(str);
				if (str.IsReadable())
					Clipboard.SetText(str);
			} catch {
				
			}
		}
		
	
		public static void OnClipboardFile(Action<String> action)
		{
			try {
				var dir = Clipboard.GetText().Trim();
				var found = false;
				if (File.Exists(dir)) {
					found = true;
				} else {
					var ls = Clipboard.GetFileDropList();
					if (ls.Count > 0) {
						if (File.Exists(ls[0])) {
							dir = ls[0];
						}
					}
				}
				if (found) {
					action(dir);
				}
			} catch {
				
			}
		}
	}
	
	public static class Codes
	{
		public static IEnumerable<string> FormatMethodList(string value)
		{
			var count = 0;
			var sb = new StringBuilder();
			var ls = new List<string>();
			for (int i = 0; i < value.Length; i++) {
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
		public static void FormatWithClangFormat(string fileName)
		{
			Process.Start(new ProcessStartInfo() {
				FileName = "clang-format",
				Arguments = string.Format("\"{0}\" -i -style=llvm -sort-includes=false", fileName)
			});
		}
		
		public static void FormatVSCTypeDef()
		{
			WinForms.OnClipboardString((str) => {
				var result = str.Trim();
				var obj = new Dictionary<string,dynamic>();
				obj.Add("prefix",result.SubstringAfterLast(' ').TrimEnd(';'));
				//obj.Add("prefix", string.Join("", matches).ToLower());
				obj.Add("body", result.SubstringAfterLast(' ').TrimEnd(';')+" $0");// changed
				
		 
				var r = new Dictionary<string,dynamic>();
				r.Add(result, obj);
				var sr = Newtonsoft.Json.JsonConvert.SerializeObject(r).Replace("\\\\u", "\\u");
				return	sr.Substring(1, sr.Length - 2) + ",";
			                         
			});
		}
	}


	public static class Win32
	{
		public static void OnClipboardDirectory(Action<String> action)
		{
			try {
				var dir = Clipboard.GetText().Trim();
				var found = false;
				if (Directory.Exists(dir)) {
					found = true;
				} else {
					var ls = Clipboard.GetFileDropList();
					if (ls.Count > 0) {
						if (Directory.Exists(ls[0])) {
							dir = ls[0];
						}
					}
				}
				if (found) {
					action(dir);
				}
			} catch {
				
			}
		}
	}
  
     
	public static class StringExtensions
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

		public static int HexToInt(this string hexNumber)
		{
			int decValue = unhex_table[(byte)hexNumber[0]];
			for (int i = 1; i < hexNumber.Length; i++) {
				decValue *= 16;
				decValue += unhex_table[(byte)hexNumber[i]];
			}
			return decValue;
		}
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
			return  new StringBuilder().AppendLine(value);
		}
		public static bool IsVacuum(this string value)
		{
			return  string.IsNullOrWhiteSpace(value);
		}
		public static bool IsReadable(this string value)
		{
			return  !string.IsNullOrWhiteSpace(value);
		}
		public static string GetFirstReadable(this string value)
		{
			return  value.TrimStart().Split(new char[] { '\n' }, 2).First().Trim();
		}
		public static IEnumerable<string> ToLines(this string value)
		{
			return  value.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim());
		}

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

		public static void UTF8ToGbk(this string path)
		{
			var content = File.ReadAllText(path, new UTF8Encoding(false));
			File.WriteAllText(path, content, Encoding.GetEncoding("gbk"));
		}
		public static void GbkToUTF8(this string path)
		{
			var content = File.ReadAllText(path, Encoding.GetEncoding("gbk"));
			File.WriteAllText(path, content, new UTF8Encoding(false));
		}
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
		public static IEnumerable<string> GetFiles(this string dir, string pattern, bool bExclude = false)
		{
			if (bExclude)
				return Directory.GetFiles(dir).Where(i => !Regex.IsMatch(i, "\\.(?:" + pattern + ")$"));
			else
				return Directory.GetFiles(dir).Where(i => Regex.IsMatch(i, "\\.(?:" + pattern + ")$"));
			
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

		//		public static IEnumerable<string> GetFiles(this String path, string pattern = "*")
		//		{
		//			foreach (var item in Directory.GetFiles(path, pattern)) {
		//				yield return item;
		//			}
		//		}
		public static DirectoryInfo GetParent(this String path)
		{
			return  Directory.GetParent(path);
		}
		public static void CreateDirectoryIfNotExists(this String path)
		{
			if (Directory.Exists(path))
				return;
			Directory.CreateDirectory(path);
		}
		public static bool DirectoryExists(this String path)
		{
			return  Directory.Exists(path);
		}
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
			File.WriteAllText(path, contents, sUTF8Encoding);
		}
		public static void WriteAllBytes(this String path, byte[] bytes)
		{
			File.WriteAllBytes(path, bytes);
		}
		public static void WriteAllLines(this String path, String[] contents)
		{
			File.WriteAllLines(path, contents, sUTF8Encoding);
		}
		public static void AppendAllText(this String path, String contents)
		{
			File.AppendAllText(path, contents, sUTF8Encoding);
		}
		public static void AppendAllLines(this String path, IEnumerable<String> contents, Encoding encoding)
		{
			File.AppendAllLines(path, contents, sUTF8Encoding);
		}
		public static void FileMove(this String sourceFileName, String destFileName)
		{
			File.Move(sourceFileName, destFileName);
		}
		public static void FileReplace(this String sourceFileName, String destinationFileName, String destinationBackupFileName)
		{
			File.Replace(sourceFileName, destinationFileName, destinationBackupFileName);
		}
		public static StreamReader OpenText(this String path)
		{
			return  File.OpenText(path);
		}
		public static StreamWriter CreateText(this String path)
		{
			return  File.CreateText(path);
		}
		public static StreamWriter AppendText(this String path)
		{
			return  File.AppendText(path);
		}
		public static FileStream Create(this String path)
		{
			return  File.Create(path);
		}
		public static FileStream Open(this String path, FileMode mode, FileAccess access)
		{
			return  File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		}
		public static DateTime FileGetCreationTimeUtc(this String path)
		{
			return  File.GetCreationTimeUtc(path);
		}
		public static DateTime FileGetLastAccessTimeUtc(this String path)
		{
			return  File.GetLastAccessTimeUtc(path);
		}
		public static DateTime FileGetLastWriteTimeUtc(this String path)
		{
			return  File.GetLastWriteTimeUtc(path);
		}
		public static FileStream OpenRead(this String path)
		{
			return  File.OpenRead(path);
		}
		public static FileStream OpenWrite(this String path)
		{
			return  File.OpenWrite(path);
		}
		public static String ReadAllText(this String path)
		{
			return  File.ReadAllText(path, new UTF8Encoding(false));
		}
		public static IEnumerable<string> ReadLines(this String path)
		{
			return  File.ReadLines(path, new UTF8Encoding(false));
		}
		public static byte[] ReadAllBytes(this String path)
		{
			return  File.ReadAllBytes(path);
		}
		public static String ChangeExtension(this String path, String extension)
		{
			return  Path.ChangeExtension(path, extension);
		}
		public static string GetDirectoryName(this string path)
		{
			return  Path.GetDirectoryName(path);
		}
		public static String GetExtension(this String path)
		{
			return  Path.GetExtension(path);
		}
		public static String GetFullPath(this String path)
		{
			return  Path.GetFullPath(path);
		}
		public static String GetFileName(this String path)
		{
			return  Path.GetFileName(path);
		}
		public static String GetFileNameWithoutExtension(this String path)
		{
			return  Path.GetFileNameWithoutExtension(path);
		}
		public static String GetPathRoot(this String path)
		{
			return  Path.GetPathRoot(path);
		}
		public static String Combine(this String path1, String path2)
		{
			return  Path.Combine(path1, path2);
		}
		public static string ToLine(this IEnumerable<string> value, string separator = "\r\n")
		{
			return string.Join(separator, value);
		}
		public static bool FileExists(this String path)
		{
			return  File.Exists(path);
		}
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
