

namespace Shared
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.IO;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using System.Diagnostics;
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
						zip.Save(element + ".epub");
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
			} catch(Exception ingored) {
				
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
	
}
