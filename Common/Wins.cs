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
namespace Common
{
	public static class Wins
	{
		
		public static void OnClipboardFiles(Action<IEnumerable<string>>f)
		{
			var list = new List<string>();
			
			var s = Clipboard.GetFileDropList();
			if (s.Count == 0) {
				var file=Clipboard.GetText().Trim();
				if(File.Exists(file)){
					list.Add(file);
					f(list);
				}
				return;
			}
			foreach (var element in s) {
				list.Add(element);
			}
			f(list);
			
			
		}
		public static void OnClipboardFileSystem(Action<IEnumerable<string>>f)
		{
			var list = new List<string>();
			
			var s = Clipboard.GetFileDropList();
			if (s.Count == 0) {
				var file=Clipboard.GetText().Trim();
				if(File.Exists(file)||Directory.Exists(file)){
					list.Add(file);
					f(list);
				}
				return;
			}
			foreach (var element in s) {
				list.Add(element);
			}
			f(list);
			
			
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
		public static void OnClipboardText(Action<string> func)
		{
			var value = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(value))
				return;
			func(value);
			
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
	}
}