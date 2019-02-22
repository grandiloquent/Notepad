namespace Common
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	
	public static  class Utilities
	{
		 private static readonly char[] InvalidFileNameChars = {'\"', '<', '>', '|', '\0', ':', '*', '?', '\\', '/'};

		public static bool IsWhiteSpace(this String value)
        {
            if (value == null) return true;

            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsWhiteSpace(value[i])) return false;
            }

            return true;
        }
		  public static string GetValidFileName(this String v)
        {
            if (v == null) return null;
            // (Char -> Int) 1-31 Invalid;
            List<char> chars = new List<char>(v.Length);

            for (int i = 0; i < v.Length; i++)
            {
                if (InvalidFileNameChars.Contains(v[i]))
                {
                    chars.Add(' ');
                }
                else
                {
                    chars.Add(v[i]);
                }
            }

            return new String(chars.ToArray());
        }
		public static void CreateDirectoryIfNotExists(this String path)
		{
			if (Directory.Exists(path))
				return;
			Directory.CreateDirectory(path);
		}
		
        public static string GetDirectoryFileName(this string v)
        {
            return Path.GetFileName(Path.GetDirectoryName(v));
        }
		public static string GetDesktopPath(this string f)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), f);
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
		
		public static string GetValidFileName(this string value, char c)
		{

			var chars = Path.GetInvalidFileNameChars();

			return new string(value.Select<char, char>((i) => {
				if (chars.Contains(i))
					return c;
				return i;
			}).Take(125).ToArray());
		}
		public static bool IsReadable(this string value)
		{
			return  !string.IsNullOrWhiteSpace(value);
		}
		public static bool IsVacuum(this string value)
		{
			return  string.IsNullOrWhiteSpace(value);
		}
		
		public static String[] ReadAllLines(this String path)
		{
          
 
			String line;
			List<String> lines = new List<String>();
 
			using (StreamReader sr = new StreamReader(path, new UTF8Encoding(false)))
				while ((line = sr.ReadLine()) != null)
					lines.Add(line);
 
			return lines.ToArray();
		}
		public static String ReadAllText(this String path)
		{
			var encoding = new UTF8Encoding(false);
			
			using (StreamReader sr = new StreamReader(path, encoding, true))
				return sr.ReadToEnd();
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
		public static string ToLine(this IEnumerable<string> value, string separator = "\r\n")
		{
			return string.Join(separator, value);
		}
		public	static string TrimComments(this string code)
		{
			var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
			return Regex.Replace(code, re, "$1");
		}
		public static void WriteAllLines(this String path, IEnumerable<String> contents)
		{
		
				
			using (var writer =	new StreamWriter(path, false, new UTF8Encoding(false))) {
				foreach (String line in contents) {
					writer.WriteLine(line);
				}
			}
		}
		public static void WriteAllText(this String path, String contents)
		{
			using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(false)))
				sw.Write(contents);
		}
	}
}