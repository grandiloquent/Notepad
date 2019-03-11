namespace Common
{
	using System.IO;
	using System;
	using System.IO;
	using System.Text;
	
	public static class Files
	{
		public static void WriteAllText(this String path, String contents)
		{
			using (var sw = new StreamWriter(path, false, new UTF8Encoding(false)))
				sw.Write(contents);
		}
		public static string GetDesktopPath(this string f)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), f);
		}
		
		public static bool CreateIfNotExists(this string dir){
			if (Directory.Exists(dir)) {
				return true;
			}
			return Directory.CreateDirectory(dir).Exists;
		}
	}
}