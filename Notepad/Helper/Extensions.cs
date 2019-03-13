
using System;
using System.IO;
using System.Text;

namespace Notepad
{
	
	public static class Extensions
	{
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
		
		public static readonly char AltDirectorySeparatorChar = '/';
		public static readonly char VolumeSeparatorChar = ':';
		public static readonly char DirectorySeparatorChar = '\\';
		public static String ReadAllText(this String path)
		{
			using (StreamReader sr = new StreamReader(path, new UTF8Encoding(false), true, 1024))
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
		
		public static string SubstringAfterLast(this string value, char delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			return index == -1 ? value : value.Substring(index + 1);
		}
		public static string SubstringAfterLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			return index == -1 ? value : value.Substring(index + 1);
		}
		
		
	}
}
