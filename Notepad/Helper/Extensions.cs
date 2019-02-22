
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
		public static string[] ToLines(this string value)
		{
			return value.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
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
		public static String GetFileNameWithoutExtension(this String path)
		{
			path = GetFileName(path);
			if (path != null) {
				int i;
				if ((i = path.LastIndexOf('.')) == -1)
					return path; // No path extension found
                else
					return path.Substring(0, i);
			}
			return null;
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
			return index == -1 ? value : value.Substring(index + delimiter.Length);
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
		public static String GetExtension(this String path)
		{
			if (path == null)
				return null;
 
			int length = path.Length;
			for (int i = length; --i >= 0;) {
				char ch = path[i];
				if (ch == '.') {
					if (i != length - 1)
						return path.Substring(i, length - i);
					else
						return String.Empty;
				}
				if (ch == DirectorySeparatorChar || ch == AltDirectorySeparatorChar || ch == VolumeSeparatorChar)
					break;
			}
			return String.Empty;
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
	}
}
