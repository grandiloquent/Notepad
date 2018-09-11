using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Shared;

namespace Notepad
{

	public static class Helper
	{
		
		public static void KotlinFormatJavaMethodParameters()
		{
			OnClipboardString((v) => {
				var ls = v.Split(new []{ ',' }, StringSplitOptions.RemoveEmptyEntries);
				var r=new List<String>();
				foreach (var element in ls) {
					var s=Regex.Split(element.Trim(),"\\s+");
					r.Add(string.Format("var {0}:{1}",s.Last(),s.First().Capitalize()));
				}
				
				return string.Join(",\n",r);
			});
		}
		
		public static void KotlinExtractParameters()
		{
			OnClipboardString((v) => {
				var ls = v.Split(new []{ ':' }, StringSplitOptions.RemoveEmptyEntries);
				var r=new List<String>();
				foreach (var element in ls) {
					var s=Regex.Split(element.Trim(),"\\s+");
					r.Add(s.Last());
				}
				
				return string.Join(",",r);
			});
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
			//if (ls.Any())
			//{
			//    var firstLine = ls[0];
			//    ls.RemoveAt(0);
			//    ls.Add(firstLine.)

			//}
			return ls;
			//return ls.Select(i => i.Split(new char[] { '{' }, 2).First().Trim() + ";").OrderBy(i => i.Trim());

		}
	}
}
