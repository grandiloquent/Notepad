namespace Common
{
	using System.Windows.Forms;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.IO;
	
	public static class Forms
	{
		public static void OnClipboardFileDropList(Action<List<string>> action)
		{
			var files = Clipboard.GetFileDropList();
			if (files.Count > 0) {
				var ls = new List<string>();
				for (int i = 0; i < files.Count; i++) {
					ls.Add(files[i]);
				}
					
				action(ls);
			}
		}
		public static void OnClipboardDirectory(Action<string> action)
		{
			
			var dir = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir)) {
				var files = Clipboard.GetFileDropList();
				if (files.Count > 0)
					dir = files[0];
			}
			if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
				return;
			action(dir);
			
		}
		public static void OnClipboardFile(Action<string> action)
		{
			var dir = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(dir) || !File.Exists(dir)) {
				var files = Clipboard.GetFileDropList();
				if (files.Count > 0)
					dir = files[0];
			}
			if (string.IsNullOrWhiteSpace(dir) || !File.Exists(dir))
				return;
			action(dir);
		}
		public static void OnClipboardText(Action<string> action)
		{
			var value = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(value))
				return;
			action(value);
		}
		public static void OnClipboardString(Func<string, string> func)
		{
			var value = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(value))
				return;
			var result = func(value);
			if (string.IsNullOrWhiteSpace(result))
				return;
			Clipboard.SetText(result);
		}
	}
}