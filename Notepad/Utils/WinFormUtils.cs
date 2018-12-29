
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

namespace Utils
{
	
	public static class WinFormUtils
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
		
		public static void ImportSingleCodeFile(string path)
		{
//			var str = path.ReadAllText().Trim();
//			while (str.StartsWith("/*")) {
//				str = str.SubstringAfter("*/").Trim();
//			}
//			var language = Path.GetExtension(path).TrimStart('.');
//			if (language == "cs") {
//				language = "csharp";
//			}
//			sb.AppendLine("## " + path.GetFileNameWithoutExtension())
//			                     			.AppendLine()
//			                     			.AppendLine()
//			                     			.Append("```")
//						.AppendLine(language)
//			                     			.AppendLine()
//			                     			.AppendLine(Regex.Replace(str.Replace("`", j), "[\r\n]+", "\r\n"))
//			                     			.AppendLine("```")
//			                     			.AppendLine();
		}
		
	}
}
