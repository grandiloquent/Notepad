using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Notepad
{
	public static class Shared
	{
		public static readonly char VolumeSeparatorChar = ':';
		
		public static readonly char DirectorySeparatorChar = '\\';
		internal const string DirectorySeparatorCharAsString = "\\";
		public static readonly char AltDirectorySeparatorChar = '/';
		
		
//		public static String GetExtension(this String path)
//		{
//			if (path == null)
//				return null;
//			
//			int length = path.Length;
//			for (int i = length; --i >= 0;) {
//				char ch = path[i];
//				if (ch == '.') {
//					if (i != length - 1)
//						return path.Substring(i, length - i);
//					else
//						return String.Empty;
//				}
//				if (ch == DirectorySeparatorChar || ch == AltDirectorySeparatorChar || ch == VolumeSeparatorChar)
//					break;
//			}
//			return String.Empty;
//		}
//		
		public static string ChangeExtension(this string path, string extension)
		{
			if (path != null) {
				string s = path;
				for (int i = path.Length - 1; i >= 0; i--) {
					char ch = path[i];
					if (ch == '.') {
						s = path.Substring(0, i);
						break;
					}
					if (IsDirectorySeparator(ch))
						break;
				}

				if (extension != null && path.Length != 0) {
					s = (extension.Length == 0 || extension[0] != '.') ?
                        s + "." + extension :
                        s + extension;
				}

				return s;
			}
			return null;
		}
		
//		public static String GetFileNameWithoutExtension(this String path)
//		{
//			path = GetFileName(path);
//			if (path != null) {
//				int i;
//				if ((i = path.LastIndexOf('.')) == -1)
//					return path; // No path extension found
//				else
//					return path.Substring(0, i);
//			}
//			return null;
//		}
		internal static bool IsDirectorySeparator(char c)
		{
			return c == DirectorySeparatorChar || c == AltDirectorySeparatorChar;
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
		
		public static void ReadStream(this string path, Action<Stream> action)
		{
			using (var s = File.OpenRead(path)) {
				action(s);
			}
		}
		public static byte[] GenerateSalt()
		{
			using (var randomNumberGenerator = new RNGCryptoServiceProvider()) {
				var randomNumber = new byte[32];
				randomNumberGenerator.GetBytes(randomNumber);

				return randomNumber;
			}
		}
		
		public static string Md5(this string str)
		{

			var sb = new StringBuilder();
			var data = Encoding.GetEncoding("utf-8").GetBytes(str);
			MD5 md5 = new MD5CryptoServiceProvider();
			var bytes = md5.ComputeHash(data);
			for (int i = 0; i < bytes.Length; i++) {
				sb.Append(bytes[i].ToString("x2"));
			}
			return sb.ToString();
		}
		public static async Task<string> PostWithParameters(this HttpClient httpClient, string url, Dictionary<string,string> parameters)
		{
			
			using (var content = new FormUrlEncodedContent(parameters)) {
			
				var response = await httpClient.PostAsync(url, content);
				return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : response.StatusCode.ToString();
			}
			
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
//		public static String ReadAllText(this String path)
//		{
//			using (StreamReader sr = new StreamReader(path, new UTF8Encoding(false), true, 1024))
//				return sr.ReadToEnd();
//		}
		private static HttpRequestMessage GetHttpRequestMessage(HttpMethod method, string url)
		{
			//var ip = "220.181.100." + new Random().Next(1, 255);
			var ip = "149.129.88.215";
			var httpMessage = new HttpRequestMessage(method, url);
			httpMessage.Headers.Add("Accept", "image/webp,image/apng,image/*,*/*;q=0.8");
			httpMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br");
			httpMessage.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
			httpMessage.Headers.Add("Connection", "keep-alive");
			httpMessage.Headers.Add("User-Agent",
				"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.92 Safari/537.36");
			httpMessage.Headers.Add("Client-IP", ip);
			httpMessage.Headers.Add("X-Forwarded-For", ip);
			return httpMessage;
		}
		public static async Task<string> ReadStringWithCookie(this HttpClient httpClient, string url,
			string referrer = null, string cookie = null)
		{
			var httpMessage = GetHttpRequestMessage(HttpMethod.Get, url);


			if (!string.IsNullOrWhiteSpace(referrer))
				httpMessage.Headers.Add("Referer", referrer);

			httpMessage.Headers.Add("Cookie", cookie);
			var response = await httpClient.SendAsync(httpMessage).ConfigureAwait(false);
			var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);


			return Encoding.UTF8.GetString(bytes);
		}
		
		public static bool FileExists(this string path)
		{
			return File.Exists(path);
		}
		public static string GetExecutingPath(this string f)
		{
			return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), f);
		}
		public static string GetHref(this HtmlAgilityPack.HtmlNode htmlNode, string defaultValue = null)
		{
			return htmlNode.GetAttributeValue("href", defaultValue);
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
		
		public static void Format(this TextBox textBox)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < textBox.Lines.Length; i++) {
				if (string.IsNullOrWhiteSpace(textBox.Lines[i])) {
					while (i + 1 < textBox.Lines.Length && string.IsNullOrWhiteSpace(textBox.Lines[i + 1])) {
						i++;
					}
					sb.AppendLine();
				} else {
					sb.AppendLine(textBox.Lines[i]);
				}
			}
			textBox.Text = sb.ToString();

		}
		public static void SelectLine(this TextBox textBox, bool trimEnd = false)
		{

			var start = textBox.SelectionStart;

			var length = textBox.Text.Length;
			var end = textBox.SelectionStart;
			var value = textBox.Text;
			while (start - 1 > -1 && value[start - 1] != '\n') {
				start--;
			}
			while (end + 1 < length && value[end + 1] != '\n') {
				end++;
			}
			if (trimEnd) {
				if (start == value.Length) {
					start--;
				}
				while (char.IsWhiteSpace(value[start])) {
					start++;
				}
				if (end == value.Length)
					end--;
				while (char.IsWhiteSpace(value[end])) {
					end--;
				}
			}

			textBox.SelectionStart = start;
			if (end > start)
				textBox.SelectionLength = end - start + 1;




		}
		public static bool CreateDirectoryIfNotExists(this string dir)
		{
			if (Directory.Exists(dir)) {
				return true;
			}
			return Directory.CreateDirectory(dir).Exists;
		}
		public static string GetDesktopPath(this string f)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), f);
		}
	}
}